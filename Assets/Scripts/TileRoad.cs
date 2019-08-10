using System.Collections;
using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;

public class TileRoad : Tile
{
    [OdinSerialize]
    public Dictionary<string, GameObject> roadPieces = new Dictionary<string, GameObject>();

    public override void EditorInit(int row, int col)
    {
        base.EditorInit(row, col);
        
        roadPieces["straight_hor"].SetActive(true);
    }
}
