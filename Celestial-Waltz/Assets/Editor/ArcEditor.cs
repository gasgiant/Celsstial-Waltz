using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Arc))]
public class ArcEditor : Editor
{
    Arc arc;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Setup points", GUILayout.Height(40)))
        {
            arc.SetupPoints();
        }
    }

    private void OnEnable()
    {
        arc = target as Arc;
    }
}
