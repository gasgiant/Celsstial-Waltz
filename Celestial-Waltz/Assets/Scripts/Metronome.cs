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
     
    void Awake()
    {
        instance = this;
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            start = true;
            audioSource.Play();
        }

        for (int i = current_bar; i < tempotrack.times.Count; i++)
        {
            if (tempotrack.times[i] > audioSource.time)
            {
                current_bar = i;
                break;
            }
        }
        currentFreq = tempotrack.bpms[current_bar - 1] / 60 / signature;
    }
}
