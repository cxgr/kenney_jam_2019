using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class SessionManager : MonoBehaviour
{
    public MapHolder map;
    public VehicleSpawner[] spawners;

    public GameObject[] carPrefabs;

    private UiManager ui => SingletonUtils<UiManager>.Instance;

    public GameObject GetCarPrefab(bool random, int idx = 0)
    {
        return random ? carPrefabs[Random.Range(0, carPrefabs.Length)] : carPrefabs[idx];
    }

    private void Awake()
    {
        DOTween.SetTweensCapacity(7000, 100);
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
