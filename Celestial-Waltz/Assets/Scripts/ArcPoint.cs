using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcPoint : MonoBehaviour {

    public Arc arc;
    public int index;
    public AudioClip click;

    void OnTriggerEnter2D(Collider2D collision)
    {
        arc.DiactivatePoint(index);
        //AudioSource.PlayClipAtPoint(click, transform.position);
    }
}
