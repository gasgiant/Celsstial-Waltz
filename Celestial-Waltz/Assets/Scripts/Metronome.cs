using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metronome : MonoBehaviour {

    public static Metronome instance;

    public Tempotrack tempotrack;
    public AudioSource audioSource;

    public float scale = 5; // units per bar
    public float tempo = 170; // in bpm
    public int signature; // in quarters

    //[HideInInspector]
    public float currentFreq = 1; // bars per second
    public bool start;

    public int current_bar;
    public float positionInBar;
    public float deltaTime;
    public float audioTime;

    float prevAudioTime;
    float audioTimeStutter;

    public float dif;
    float strt;
     
    void Awake()
    {
        instance = this;
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            start = true;
            strt = Time.time;
            audioSource.Play();
        }

        audioTime = (float)audioSource.timeSamples / 44100;
        float audioDeltaTime = audioTime - prevAudioTime;

        if (audioDeltaTime != 0)
        {
            deltaTime = Mathf.Clamp01(audioDeltaTime - audioTimeStutter);
            audioTimeStutter = 0;
        }
        else
        {
            deltaTime = Time.fixedDeltaTime / 2f;
            audioTimeStutter += deltaTime;
        }

        prevAudioTime = audioTime;

        dif = Time.time - strt - audioTime;

        for (int i = current_bar; i < tempotrack.times.Length; i++)
        {
            if (tempotrack.times[i] > audioTime)
            {
                current_bar = i;
                break;
            }
        }
        currentFreq = tempotrack.bpms[current_bar - 1] / 60 / signature;
        positionInBar = (audioTime - tempotrack.times[current_bar - 1]) * currentFreq;
    }
}
