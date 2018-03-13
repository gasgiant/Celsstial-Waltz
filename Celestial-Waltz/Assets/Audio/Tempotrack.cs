using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New tempotrack", menuName = "Tempotrack")]
public class Tempotrack : ScriptableObject {

    public float[] times;
    public float[] bpms;
}
