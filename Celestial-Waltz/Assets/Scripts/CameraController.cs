using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float smoothTime = 0.1f;
    public float zDistance = 28;

    Transform tr;
    Transform player;

    Vector3 vel;

    void Start()
    {
        tr = transform;
        player = PlayerController.instance.transform;
    }

    void LateUpdate ()
    {
        Vector3 target = new Vector3(player.position.x, player.position.y, -zDistance);
        tr.position = Vector3.SmoothDamp(tr.position, target, ref vel, smoothTime);
	}
}
