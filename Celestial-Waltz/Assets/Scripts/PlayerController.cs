using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public PlayerState state = PlayerState.Stoped;
    
    public Vector3 direction;
    bool touchControl;

    float speed;
    float rotationSpeed;
    bool needsExtrapolation = true;

    public static PlayerController instance;

    Metronome metr;

    float audioTimeStutter;

    [HideInInspector]
    public Transform targetArc;

    [HideInInspector]
    public Rigidbody2D rb;
    [HideInInspector]
    public Transform tr;

    public int extrapBars = 3;
    [HideInInspector]
    public Vector3[] trajectExtrapolation;

    void Awake()
    {
        trajectExtrapolation = new Vector3[extrapBars];
        rb = GetComponent<Rigidbody2D>();
        tr = transform;
        metr = Metronome.instance;
        touchControl = Options.instance.touchControl;
        instance = this;
    }

    void FixedUpdate ()
    {
        if (metr.start)
            MovementUpdate();
        //if (needsExtrapolation && state == PlayerState.MovingForvard)
        if (!targetArc || needsExtrapolation)
            ExtrapolateTrajectory();
        metr.deltaTime = 0;
    }

    void MovementUpdate()
    {
        speed = metr.scale * metr.currentFreq;
        rotationSpeed = metr.currentFreq * 90;

        state = PlayerState.MovingForvard;

        HandleControls(touchControl);

        direction = new Vector2(-Mathf.Sin(Mathf.Deg2Rad * rb.rotation), Mathf.Cos(Mathf.Deg2Rad * rb.rotation));
        rb.position = rb.position + speed * (Vector2)direction * metr.deltaTime;
    }

    void HandleControls(bool touchControl)
    {
        if (touchControl && Input.touchCount > 0 && Input.touches[0].position.y < Screen.height / 2)
        {
            if ((Input.touches[0].position.x < Screen.width / 2))
            {
                rb.rotation = rb.rotation + metr.deltaTime * rotationSpeed;
                state = PlayerState.Rotating;
                needsExtrapolation = true;
            }

            if ((Input.touches[0].position.x > Screen.width / 2))
            {
                rb.rotation = rb.rotation - metr.deltaTime * rotationSpeed;
                state = PlayerState.Rotating;
                needsExtrapolation = true;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.A))
            {
                rb.rotation = rb.rotation + metr.deltaTime * rotationSpeed;
                state = PlayerState.Rotating;
                needsExtrapolation = true;
            }

            if (Input.GetKey(KeyCode.D))
            {
                rb.rotation = rb.rotation - metr.deltaTime * rotationSpeed;
                state = PlayerState.Rotating;
                needsExtrapolation = true;
            }
        }
        
    }

    void ExtrapolateTrajectory()
    {
        Vector3 pos = tr.position;
        for (int i = 0; i < trajectExtrapolation.Length; i++)
        {
            trajectExtrapolation[i] = pos + metr.scale * (i + 1 + Options.instance.latency / metr.currentFreq - metr.positionInBar) *
                new Vector3(-Mathf.Sin(Mathf.Deg2Rad * rb.rotation), Mathf.Cos(Mathf.Deg2Rad * rb.rotation));
        }
        needsExtrapolation = false;
    }

    void OnDrawGizmos()
    {
        if (state == PlayerState.MovingForvard)
        {
            if (targetArc)
            {
                Gizmos.color = Color.yellow;
            }
            else
            {
                Gizmos.color = Color.red;
            }
            
            Vector3 prevPosition = trajectExtrapolation[0];
            for (int i = 0; i < trajectExtrapolation.Length; i++)
            {
                Gizmos.DrawLine(prevPosition, trajectExtrapolation[i]);
                Gizmos.DrawSphere(trajectExtrapolation[i], 0.3f);
                prevPosition = trajectExtrapolation[i];
            }
        }
    }

    public enum PlayerState { MovingForvard, Rotating, Stoped}
}
