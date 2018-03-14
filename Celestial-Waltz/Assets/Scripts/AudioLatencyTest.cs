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
    float lastAudioTime;

    List<float> lat = new List<float>();

    void OnEnable()
    {
        source.Play();
    }

    void FixedUpdate()
    {
        float time = (float)source.timeSamples / discr;
        if (time - lastAudioTime > 0.6f)
        {
            gotBeat = false;
            beatNum++;
            lastAudioTime = time;
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
