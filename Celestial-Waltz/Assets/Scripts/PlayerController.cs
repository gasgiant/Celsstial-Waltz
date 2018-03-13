using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    float speed = 10;
    float rotationSpeed = 80;

    Metronome metr;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        metr = Metronome.instance;
    }

    void FixedUpdate ()
    {
        if (metr.start)
            HandleControls();
    }

    void HandleControls()
    {
        speed = metr.scale * metr.currentFreq;
        rotationSpeed = metr.currentFreq * 90;

        if (Input.GetKey(KeyCode.A))
        {
            rb.MoveRotation(rb.rotation + Time.fixedDeltaTime * rotationSpeed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            rb.MoveRotation(rb.rotation - Time.fixedDeltaTime * rotationSpeed);
        }

        rb.velocity = speed * new Vector2(-Mathf.Sin(Mathf.Deg2Rad * rb.rotation), Mathf.Cos(Mathf.Deg2Rad * rb.rotation));
    }
}
