using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    public float moveSpd;
    
    public List<Tile> path;
    public VehicleSpawner owner;
    
    private Sequence movementSeq;

    private MapHolder map => SingletonUtils<MapHolder>.Instance;

    public void Go(List<Tile> path, VehicleSpawner owner)
    {
        this.path = path;
        this.owner = owner;

        transform.DOPath(
            path.Select(p => map.mapGraph[p.coordTuple].GetMovementPos()).ToArray(),
            moveSpd, PathType.Linear, PathMode.Full3D, 10)
            .SetOptions(false, AxisConstraint.None, AxisConstraint.X | AxisConstraint.Z)
            .SetLookAt(0.001f, null, Vector3.up)
            .SetSpeedBased().SetEase(Ease.Linear);
    }
    
    public void Despawn()
    {
        owner.RemoveMe(this);
    }
}
