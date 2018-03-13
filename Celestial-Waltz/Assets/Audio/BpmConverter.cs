using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BpmConverter : MonoBehaviour {

    public TextAsset time;
    public TextAsset bpm;

    public List<float> times = new List<float>();
    public List<float> bpms = new List<float>();

    public Tempotrack tempotrack;

    [ContextMenu("readBPM")]
    public void ReadBPM()
    {
        char ch = '\n';
        var t = time.text.Split(ch);
        var b = bpm.text.Split(ch);


        
        //times.Clear();
        //bpms.Clear();

        for (int i = 0; i < t.Length; i++)
        {
           // times.Add(System.Convert.ToSingle(t[i]));
        }

        for (int i = 0; i < b.Length; i++)
        {
            //bpms.Add(System.Convert.ToSingle(b[i]));
        }
      
        tempotrack.times = times.ToArray();
        tempotrack.bpms = bpms.ToArray();

    }
	
}
