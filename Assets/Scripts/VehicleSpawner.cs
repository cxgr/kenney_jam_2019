using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class VehicleSpawner : MonoBehaviour
{
    public GameObject[] vehiclePrefabs;
    public TileRoad tileSrc;
    public TileRoad tileDst;

    public List<VehicleController> liveVehicles;

    public bool isLive;

    public float spawnCooldown = 10f;
    public float spawnTimer = 5f;

    private MapHolder map => SingletonUtils<MapHolder>.Instance;

    private void Awake()
    {
        if (null == tileSrc)
        {
            tileSrc = FindObjectsOfType<TileRoad>()
                .OrderBy(tr => Vector3.Distance(transform.position, tr.transform.position)).FirstOrDefault();
        }

        if (null == tileDst)
        {
            tileDst = FindObjectsOfType<TileRoad>()
                .OrderByDescending(tr => Vector3.Distance(transform.position, tr.transform.position)).FirstOrDefault(); 
        }
    }

    void Update()
    {
        if (isLive)
        {
            if (spawnTimer <= 0f)
            {
                Spawn();
                spawnTimer = Mathf.Max(2f,spawnCooldown * Random.value * 1.5f);
            }
            else
                spawnTimer -= Time.deltaTime;
        }
    }
    
    public void Spawn(bool allowFlip = true)
    {
        if (allowFlip && liveVehicles.Count == 0)
        {
            if (Random.value < .5f)
            {
                var tmp = tileSrc;
                tileSrc = tileDst;
                tileDst = tmp;
            }   
        }

        var vehicle = Instantiate(vehiclePrefabs[Random.Range(0, vehiclePrefabs.Length)],
            tileSrc.GetMovementPos(), Quaternion.identity).GetComponent<VehicleController>();
        vehicle.Go(map.pathing.FindPath(tileSrc, tileDst), this);
        liveVehicles.Add(vehicle);
    }
    
    public void RemoveMe(VehicleController vc)
    {
        if (liveVehicles.Contains(vc))
            liveVehicles.Remove(vc);
    }
}
