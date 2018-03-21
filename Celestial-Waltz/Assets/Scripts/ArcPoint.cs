using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcPoint : MonoBehaviour {

    public int index;
    public GameObject visual;
    public AudioClip click;

    Transform tr;
    GameObject go;

    void OnEnable()
    {
        go = gameObject;
        tr = transform;
    }

    public void Diactivate()
    {
        //AudioSource.PlayClipAtPoint(click, tr.position);
        visual.SetActive(false);
    }

    public void Activate()
    {
        visual.SetActive(true);
    }

    public void SetPosition(Vector3 newPosition)
    {
        tr.position = newPosition;
    }

    public void SetLocalPosition(Vector3 newPosition)
    {
        tr.localPosition = newPosition;
    }

    public Vector3 GetPosition()
    {
        return tr.position;
    }

    public Vector3 GetLocalPosition()
    {
        return tr.localPosition;
    }
}
