using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    void Update()
    {
        if (isLive)
        {
            if (spawnTimer <= 0f)
            {
                Spawn();
                spawnTimer = spawnCooldown;
            }
            else
                spawnTimer -= Time.deltaTime;
        }
    }
    
    public void Spawn()
    {
        if (Random.value < .5f)
        {
            var tmp = tileSrc;
            tileSrc = tileDst;
            tileDst = tmp;
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
