using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour {

    public static Options instance;

    public float positionSnapTime = 0.4f;
    public float rotationSnapTime = 0.4f;
    public float maxSnapSpeed = 20;
    public float close_threshold = 0.2f;
    public float gather_threshold;
    public float magnet_threshold;
    public float magnet_velocity;
    public float fail_threshold = 7;
    public float ignore_distance = 30;
    public float ignore_angle = 90;

    public float latency;
    public bool touchControl;

    public void Awake()
    {
        instance = this;
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            touchControl = true;
    }
}
