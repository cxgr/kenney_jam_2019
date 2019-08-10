using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class VehicleSpawner : MonoBehaviour
{
    public TileRoad tileSrc;
    public TileRoad tileDst;

    public List<VehicleController> liveVehicles;

    public bool isLive;

    public float spawnCooldown = 10f;
    public float spawnTimer = 5f;

    private MapHolder map => SingletonUtils<MapHolder>.Instance;
    private SessionManager session => SingletonUtils<SessionManager>.Instance;

    public GameObject connectedTile;

    private void Awake()
    {
        var tmp = transform.position;
        tmp.y = 1.5f;
        transform.position = tmp;

        tmp = connectedTile.transform.position;
        tmp.y = 1.5f;
        connectedTile.transform.position = tmp;

        if (null == tileSrc)
        {
            if (Physics.Raycast(transform.position, Vector3.down, out var rHit, Mathf.Infinity,
                1 << LayerMask.NameToLayer("GroundPlane")))
            {
                tileSrc = rHit.collider.GetComponent<TileRoad>();
            }
        }

        if (null == tileDst)
        {
            if (Physics.Raycast(connectedTile.transform.position, Vector3.down, out var rHit, Mathf.Infinity,
                1 << LayerMask.NameToLayer("GroundPlane")))
            {
                tileDst = rHit.collider.GetComponent<TileRoad>();
            }
        }

        /*
        if (null == tileSrc)
        {
            tileSrc = FindObjectsOfType<TileRoad>()
                .OrderBy(tr => Vector3.Distance(transform.position, tr.transform.position)).FirstOrDefault();
        }

        if (null == tileDst)
        {
            tileDst = FindObjectsOfType<TileRoad>()
                .OrderBy(tr => Vector3.Distance(connectedTile.transform.position, tr.transform.position)).FirstOrDefault(); 
        }
        */
    }

    void Update()
    {
        if (isLive)
        {
            if (spawnTimer <= 0f)
            {
                StartCoroutine(Spawn());
                spawnTimer = Mathf.Max(4f,spawnCooldown * Random.value * 2) + 4f;
            }
            else
                spawnTimer -= Time.deltaTime;
        }
    }
    
    IEnumerator Spawn(bool allowFlip = true)
    {
        var bubble = SingletonUtils<FxController>.Instance.GetSpeechBubble();
        if (allowFlip && liveVehicles.Count == 0)
        {
            if (Random.value < .5f)
            {
                var tmp = tileSrc;
                tileSrc = tileDst;
                tileDst = tmp;
            }   
        }
        
        bubble.PlayClip(tileSrc.GetMovementPos() + Vector3.up * .25f, "alert");
        yield return new WaitForSeconds(4f);
        bubble.Release();

        var vehicle = Instantiate(session.GetCarPrefab(true),
            tileSrc.GetMovementPos(), Quaternion.identity).GetComponent<VehicleController>();
        vehicle.Go(map.pathing.FindPath(tileSrc, tileDst), this);
        liveVehicles.Add(vehicle);
    }
    
    public void RemoveMe(VehicleController vc)
    {
        if (liveVehicles.Contains(vc))
            liveVehicles.Remove(vc);
        spawnTimer = spawnCooldown;
    }

    void OnDrawGizmos()
    {
        if (null != connectedTile)
        {
            Gizmos.DrawSphere(transform.position, .11111f);
            Gizmos.DrawWireSphere(transform.position, .3333f);
            Gizmos.DrawSphere(connectedTile.transform.position, .3333f);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, connectedTile.transform.position);
        }
        else
        {
            connectedTile = transform.GetChild(0).gameObject;
        }
    }
}
