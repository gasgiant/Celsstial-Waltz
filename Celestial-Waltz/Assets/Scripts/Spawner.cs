using System.Collections.Generic;
using UnityEngine;
using MarchingBytes;

public class Spawner : MonoBehaviour {

    public List<GameObject> arcPrefabs;

    public static Spawner instance;

    [HideInInspector]
    public List<Arc> arcs;

	void Awake()
	{
        instance = this;
	}

    void SpawnArc(GameObject prefab)
    {
        
    }
}
