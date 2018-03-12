using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metronome : MonoBehaviour {

    public static Metronome instance;

    public float scale = 5; // units per bar
    public float currentFreq = 1; // bars per second
    public int signature; // in quarters

    void Awake()
    {
        instance = this;
    }
}
