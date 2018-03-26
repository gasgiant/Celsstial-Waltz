using System.Collections.Generic;
using UnityEngine;
using MarchingBytes;

public class Spawner : MonoBehaviour {

    public static Spawner instance;

    public float minDistance;
    public float angle;

    public bool spawnObjects;

    [HideInInspector]
    public List<Arc> arcs;

	void Awake()
	{
        instance = this;
	}

    void Update()
    {
        if (spawnObjects && arcs.Count < 1)
        {
            SpawnArc();
        }
        //if (Input.GetKeyDown(KeyCode.E))
        //    SpawnArc();
    }

    void SpawnArc()
    {
        if (EasyObjectPool.instance.poolInfo.Length > 0)
        {
            Vector3 playerDirection = PlayerController.instance.tr.rotation * Vector3Int.up;
            Vector3 pos = Quaternion.Euler(0, 0, angle * 2 * (Random.value - 0.5f))
                                   * playerDirection * Metronome.instance.scale * (3 + Random.value);
            int rnd = Random.Range(0, EasyObjectPool.instance.poolInfo.Length);
            EasyObjectPool.instance.GetObjectFromPool(EasyObjectPool.instance.poolInfo[rnd].poolName, 
                pos + PlayerController.instance.tr.position, Quaternion.identity);
        }
    }
}
