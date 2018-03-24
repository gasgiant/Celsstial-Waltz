using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour {

    public static Score instance;

    public int scr;

	void Awake () 
    {
        instance = this;
        scr = 0;
	}
	
	public void IncrementScore(int inc = 1)
    {
        scr += inc;
    }
}
