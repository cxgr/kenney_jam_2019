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

    private UiManager ui => SingletonUtils<UiManager>.Instance;

    public int arrived = 0;
    public int dead = 0;
    public int annoyed = 0;

    public GameObject GetCarPrefab(bool random, int idx = 0)
    {
        return random ? carPrefabs[Random.Range(0, carPrefabs.Length)] : carPrefabs[idx];
    }

    public void StartNewGame()
    {
        spawners = GetComponentsInChildren<VehicleSpawner>();
        foreach (var s in spawners)
        {
            s.isLive = true;
        }
        
        SingletonUtils<TimeManager>.Instance.StartTimer();
    }

    public void FinishByTimeOut()
    {
        SingletonUtils<TimeManager>.Instance.SetPaused(true);
        ui.ShowGameoverScreen();
    }
}
