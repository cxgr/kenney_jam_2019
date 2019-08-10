using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SessionManager : MonoBehaviour
{
    public MapHolder map;
    public VehicleSpawner[] spawners;

    public GameObject[] carPrefabs;

    private void Start()
    {
        spawners = GetComponentsInChildren<VehicleSpawner>();
        foreach (var s in spawners)
        {
            s.isLive = true;
        }
    }

    public GameObject GetCarPrefab(bool random, int idx = 0)
    {
        return random ? carPrefabs[Random.Range(0, carPrefabs.Length)] : carPrefabs[idx];
    }
}
