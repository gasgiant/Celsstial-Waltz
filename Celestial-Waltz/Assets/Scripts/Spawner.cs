﻿using System.Collections.Generic;
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
        Vector3 playerDirection = PlayerController.instance.tr.rotation * Vector3Int.up;
        Vector3 pos = Quaternion.Euler(0, 0, angle * 2 * (Random.value - 0.5f)) 
                                * playerDirection * Metronome.instance.scale * (3 + Random.value);
        float rnd = Random.value;
        if (rnd > 0.5f)
            EasyObjectPool.instance.GetObjectFromPool("L8S", pos + PlayerController.instance.tr.position, Quaternion.identity);
        else
            EasyObjectPool.instance.GetObjectFromPool("R8S", pos + PlayerController.instance.tr.position, Quaternion.identity);
        /*

        if (rnd < 0.33f)
        {
            EasyObjectPool.instance.GetObjectFromPool("LeftCircle", pos + PlayerController.instance.tr.position, Quaternion.identity);
        }
        else
        {
            if (rnd < 0.667f)
                EasyObjectPool.instance.GetObjectFromPool("RightCircle", pos + PlayerController.instance.tr.position, Quaternion.identity);
                
        }
        */


    }
}
