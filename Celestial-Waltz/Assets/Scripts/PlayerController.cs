using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public PlayerState state = PlayerState.Stoped;

    public Joystick joystick;
    
    public Vector3 direction;
    bool touchControl;

    float speed;
    float rotationSpeed;
    bool needsExtrapolation = true;

    public static PlayerController instance;

    Metronome metr;

    float audioTimeStutter;

    [HideInInspector]
    public Arc targetArc;

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
        if (GameManager.gameState == GameManager.GameState.Normal)
            MovementUpdate();
        //if (needsExtrapolation && state == PlayerState.MovingForvard)
        if (!targetArc || needsExtrapolation)
            ExtrapolateTrajectory();
        metr.deltaTime = 0;
    }

    public void Reset()
    {
        direction = Vector3.up;
        rb.rotation = 0;
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
        if (joystick.touched)
        {
            float angle = Vector3.Angle(direction, -joystick.direction);
            float cross = Vector3.Cross(-joystick.direction, direction).z;

            if (Mathf.Abs(cross) > 0.1f)
            {
                rb.rotation = rb.rotation + metr.deltaTime * Mathf.Clamp(cross * 5 * rotationSpeed, -rotationSpeed, rotationSpeed) ;
                state = PlayerState.Rotating;
                needsExtrapolation = true;

                /*
                if (cross > 0)
                {
                    
                }

                if (cross < 0)
                {
                    rb.rotation = rb.rotation - metr.deltaTime * rotationSpeed * 1.2f;
                    state = PlayerState.Rotating;
                    needsExtrapolation = true;
                }
                */

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
