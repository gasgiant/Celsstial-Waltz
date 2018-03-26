using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(Arc))]
[CanEditMultipleObjects]
public class ArcEditor : Editor
{
    Arc arc;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Setup points", GUILayout.Height(40)))
        {
            SetupPoints();
        }
    }

    private void OnEnable()
    {
        arc = target as Arc;
        arc.metr = GameObject.Find("Metronome").GetComponent<Metronome>();
    }

    public void SetupPoints()
    {
        if (arc.points.Count != 0)
        {
            while (arc.points.Count > 0)
            {
                GameObject go = arc.points[0].gameObject;
                arc.points.Remove(arc.points[0]);
                DestroyImmediate(go);
            }
        }

        arc.bars.Clear();
        Arc.Direction drc = Arc.Direction.Up;

        for (int i = 0; i < arc.bars_directions.Count; i++)
        {
            arc.bars.Add(arc.GetBars(drc, arc.bars_directions[i], ref drc));
        }

        Vector3 nextStartPoint = Vector3.zero;
        int pointsCount = 0;

        for (int i = 0; i < arc.bars.Count; i++)
        {
            List<Vector3> pos = new List<Vector3>();
            pos = arc.GetBarsPointsPositions(arc.bars[i].type, arc.metr.scale, arc.bars[i].reverse);

            for (int j = 0; j < pos.Count - 1; j++)
            {
                GameObject go = PrefabUtility.InstantiatePrefab(arc.pointPrefab) as GameObject;
                ArcPoint ap = go.GetComponent<ArcPoint>();
                Transform tr = go.GetComponent<Transform>();

                arc.points.Add(ap);
                ap.index = pointsCount;
                pointsCount++;

                tr.parent = arc.transform;
                tr.localPosition = nextStartPoint + pos[j];

                if (j % arc.metr.signature != 0)
                {
                    tr.localScale = Vector3.one * 0.25f;
                }
                else
                {
                    tr.localScale = Vector3.one * 0.5f;
                }
            }

            nextStartPoint = nextStartPoint + pos[pos.Count - 1];
        }

        if (!arc.closed)
        {
            GameObject go = PrefabUtility.InstantiatePrefab(arc.pointPrefab) as GameObject;
            ArcPoint ap = go.GetComponent<ArcPoint>();
            Transform tr = go.GetComponent<Transform>();

            arc.points.Add(ap);
            ap.index = pointsCount;
            pointsCount++;

            tr.parent = arc.transform;
            tr.localPosition = nextStartPoint;
            tr.localScale = Vector3.one * 0.5f;
        }
    }
}
