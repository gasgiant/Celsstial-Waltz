using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLatencyTest : MonoBehaviour {

    public AudioSource source;
    public float averageLatency;
    public int count;
    public int beatNum;
    int discr = 48000;

    float t = 0;

    float lastBeat;
    bool gotBeat;

    List<float> lat = new List<float>();

    void OnEnable()
    {
        source.Play();
    }

    void FixedUpdate()
    {
        float time = (float)source.timeSamples / discr;
        if (time > (beatNum * 0.6f) + 0.03f)
        {
            gotBeat = false;
            beatNum++;
            lastBeat = Time.time;
        }
        

        if (Input.GetKeyDown(KeyCode.Space) && !gotBeat)
        {
            lat.Add(Time.time - lastBeat);
            gotBeat = true;
        }
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
    }
}
