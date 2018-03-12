using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 10;
    public float rotationSpeed = 80;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update ()
    {
        HandleControls();

    }

    void HandleControls()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rb.MoveRotation(rb.rotation + Time.deltaTime * rotationSpeed);
            rb.velocity = speed * new Vector2(-Mathf.Sin(Mathf.Deg2Rad * rb.rotation), Mathf.Cos(Mathf.Deg2Rad * rb.rotation));
        }

        if (Input.GetKey(KeyCode.D))
        {
            rb.MoveRotation(rb.rotation - Time.deltaTime * rotationSpeed);
            rb.velocity = speed * new Vector2(-Mathf.Sin(Mathf.Deg2Rad * rb.rotation), Mathf.Cos(Mathf.Deg2Rad * rb.rotation));
        }
    }
}
