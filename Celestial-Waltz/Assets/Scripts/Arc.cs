using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Arc : MonoBehaviour {

    public GameObject pointPrefab;
    public List<ArcPoint> points = new List<ArcPoint>();

    public void SetupPoints()
    {
        points.Clear();

        float r = 7 / Mathf.PI * 2 * 1.2f;
        for (int i = 0; i < 12; i++)
        {
            GameObject go = PrefabUtility.InstantiatePrefab(pointPrefab) as GameObject;
            ArcPoint ap = go.GetComponent<ArcPoint>();
            Transform tr = go.GetComponent<Transform>();

            ap.index = i;
            ap.arc = this;
            tr.position = transform.position + new Vector3(r * Mathf.Cos((float)i / 6 * Mathf.PI), r * Mathf.Sin((float)i / 6 * Mathf.PI));
            Debug.Log(i % 3);
            if (i % 3 != 0)
            {
                
                tr.localScale = Vector3.one * 0.25f;
            }
            tr.parent = transform;
            points.Add(ap);
        }
    }

    public void DiactivatePoint(int index)
    {
        points[index].gameObject.SetActive(false);
    }
}
