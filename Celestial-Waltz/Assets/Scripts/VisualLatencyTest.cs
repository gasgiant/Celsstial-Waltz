using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualLatencyTest : MonoBehaviour {

    public Transform jumper;
    public Text txt;

    public float averageLatency;
    public int count;
    public float height;
    public float bpm = 120;
    public AnimationCurve curve;

    bool up = true;

    float t = 0;
    float freq = 2;

    float lastBeat;
    bool gotBeat;
    List<float> lat = new List<float>();

    void OnEnable()
    {
        freq = bpm / 60;
    }

    void FixedUpdate()
    {
        float dt = up ? 1 : -1;
        t += dt * Time.fixedDeltaTime * freq * 2;

        if (t < 0)
        {
            t = 0;
            up = true;
            gotBeat = false;
            lastBeat = Time.time;
        }
        if (t > 1) up = false;

        if (Input.GetKeyDown(KeyCode.Space) && !gotBeat)
        {
            lat.Add(Time.time - lastBeat);
            gotBeat = true;
        }

        jumper.localPosition = Vector3.Lerp(Vector3.zero, Vector3.up * height, curve.Evaluate(t));
    }

    void Update()
    {
        float sum = 0;

        foreach (var dt in lat)
        {
            sum += dt;
        }

        averageLatency = sum / lat.Count;
        count = lat.Count;

        txt.text = "Visual latency: " + averageLatency + "  count: " + count;
    }
}
