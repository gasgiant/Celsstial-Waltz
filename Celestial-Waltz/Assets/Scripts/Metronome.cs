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

    public int current_bar;
    public float positionInBar;
    public float deltaTime;
    public float audioTime;

    float prevAudioTime;
    float audioTimeStutter;

    public float dif;
    float strt;
    bool isPlaing;
     
    void Awake()
    {
        instance = this;
    }

    public void Restart()
    {
        strt = Time.time;
        audioSource.Play();
        current_bar = 0;
    }

    void FixedUpdate()
    {
        if (GameManager.gameState == GameManager.GameState.Normal)
        {


            audioTime = (float)audioSource.timeSamples / 44100;
            float audioDeltaTime = audioTime - prevAudioTime;

            /*
            if (audioDeltaTime > float.Epsilon)
            {
                deltaTime = Mathf.Clamp01(audioDeltaTime - audioTimeStutter);
                audioTimeStutter = 0;
            }
            else
            {
                deltaTime = Time.fixedDeltaTime / 2;
                audioTimeStutter += deltaTime;
            }
            */
            deltaTime = Time.fixedDeltaTime;

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
}
