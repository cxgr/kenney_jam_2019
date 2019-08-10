using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionManager : MonoBehaviour
{
    public MapHolder map;
    public VehicleSpawner[] spawners;

    private void Start()
    {
        spawners = GetComponentsInChildren<VehicleSpawner>();
        foreach (var s in spawners)
        {
            s.isLive = true;
        }
    }
}
