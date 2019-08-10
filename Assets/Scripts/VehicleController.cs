using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    public float moveSpd;
    
    public List<Tile> path;
    public VehicleSpawner owner;

    private Tween goPathTween;

    private MapHolder map => SingletonUtils<MapHolder>.Instance;

    private bool isStopped = false;

    public void HandleTap()
    {
        isStopped = !isStopped;
        goPathTween.TogglePause();
    }

    public void Go(List<Tile> path, VehicleSpawner owner)
    {
        this.path = path;
        this.owner = owner;
        
        transform.LookAt(path[1].GetMovementPos(), Vector3.up);

        goPathTween = transform.DOPath(
                path.Select(p => map.mapGraph[p.coordTuple].GetMovementPos()).ToArray(),
                path.Count / moveSpd, PathType.Linear, PathMode.Full3D, 10)
            .SetOptions(false, AxisConstraint.None, AxisConstraint.X | AxisConstraint.Z)
            .SetLookAt(0.001f, null, Vector3.up).SetEase(Ease.Linear)
            .OnComplete(() => Despawn(true));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isExploding)
            return;

        var otherCar = other.GetComponent<VehicleController>();
        if (null != otherCar)
        {
            Explode();
            otherCar.Explode();
        }
    }

    private bool isExploding;
    public void Explode()
    {
        if (isExploding)
            return;
        
        isExploding = true;
        transform.DOScale(1.2f, .05f).OnComplete(() =>
        {
            SingletonUtils<FxController>.Instance.PlayExplosion(transform.position);
            Despawn();
        });
    }
    
    public void Despawn(bool success = false)
    {
        owner.RemoveMe(this);
        Destroy(gameObject);
    }
}
