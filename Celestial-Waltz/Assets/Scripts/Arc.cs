﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Arc : MonoBehaviour {

    public GameObject pointPrefab;
    public List<ArcPoint> points = new List<ArcPoint>();
    public Metronome metr;

    [ContextMenu("SetupPoints")]
    public void SetupPoints()
    {
        float r = metr.scale / Mathf.PI * 2;
        if (points.Count == 0)
        {
            for (int i = 0; i < 12; i++)
            {
                GameObject go = PrefabUtility.InstantiatePrefab(pointPrefab) as GameObject;
                ArcPoint ap = go.GetComponent<ArcPoint>();
                Transform tr = go.GetComponent<Transform>();

                ap.index = i;
                ap.arc = this;
                tr.parent = transform;
                tr.localPosition = new Vector3(r * Mathf.Cos((float)i / 6 * Mathf.PI), r * Mathf.Sin((float)i / 6 * Mathf.PI)); //transform.position + Vector3.up * 7 / 3 * i;
                if (i % 3 != 0)
                {
                    tr.localScale = Vector3.one * 0.25f;
                }
                points.Add(ap);
            }
        }
        else
        {
            for (int i = 0; i < points.Count; i++)
            {
                points[i].transform.position = new Vector3(r * Mathf.Cos((float)i / 6 * Mathf.PI), r * Mathf.Sin((float)i / 6 * Mathf.PI));
            }
        }
        
    }

    public void DiactivatePoint(int index)
    {
        points[index].gameObject.SetActive(false);
    }
}