using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New tempotrack", menuName = "Tempotrack")]
public class Tempotrack : ScriptableObject {

    public List<float> times = new List<float>();
    public List<float> bpms = new List<float>();
}
