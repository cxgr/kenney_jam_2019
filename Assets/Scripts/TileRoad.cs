using System;
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

        foreach (var rp in roadPieces.Values)
        {
            rp.SetActive(false);
        }
        //roadPieces["straight_hor"].SetActive(true);
    }

    public void ResolveGraphics(Dictionary<Tuple<int, int>, Tile> graph)
    {
        var tLeft = new Tuple<int, int>(row, col - 1);
        var tRight = new Tuple<int, int>(row, col + 1);
        var tUp = new Tuple<int, int>(row - 1, col);
        var tDown = new Tuple<int, int>(row + 1, col);

        var hasNbrLeft = graph.ContainsKey(tLeft) && graph[tLeft].isTraversible;
        var hasNbrRight = graph.ContainsKey(tRight) && graph[tRight].isTraversible;
        var hasNbrUp = graph.ContainsKey(tUp) && graph[tUp].isTraversible;
        var hasNbrDown = graph.ContainsKey(tDown) && graph[tDown].isTraversible;

        var nbrsCount = 0;
        if (hasNbrLeft) nbrsCount++; 
        if (hasNbrRight) nbrsCount++;
        if (hasNbrUp) nbrsCount++;
        if (hasNbrDown) nbrsCount++;

        if (nbrsCount == 4)
        {
            roadPieces["crossroads"].SetActive(true);
        }
        else if (nbrsCount == 2)
        {
            if (hasNbrLeft && hasNbrRight && !hasNbrUp && !hasNbrDown)
                roadPieces["straight_hor"].SetActive(true);
            else if (hasNbrUp && hasNbrDown && !hasNbrLeft && !hasNbrRight)
                roadPieces["straight_ver"].SetActive(true);
            
            else if (hasNbrLeft && hasNbrUp && !hasNbrRight && !hasNbrDown)
                roadPieces["turn_LU"].SetActive(true);
            else if (hasNbrLeft && hasNbrDown && !hasNbrRight && !hasNbrUp)
                roadPieces["turn_LD"].SetActive(true);
            
            else if (hasNbrRight && hasNbrUp && !hasNbrLeft && !hasNbrDown)
                roadPieces["turn_RU"].SetActive(true);
            else if (hasNbrRight && hasNbrDown && !hasNbrLeft && !hasNbrUp)
                roadPieces["turn_RD"].SetActive(true);
        }
        
        else if (nbrsCount == 1)
        {
            if (hasNbrLeft)
                roadPieces["deadend_LR"].SetActive(true);
            else if (hasNbrUp)
                roadPieces["deadend_UD"].SetActive(true);
            else if (hasNbrRight)
                roadPieces["deadend_RL"].SetActive(true);
            else if (hasNbrDown)
                roadPieces["deadend_DU"].SetActive(true);
        }
        else
        {
            Debug.Log("hui");
        }
    }

    private Transform cachedCurvePoint;
    private void Awake()
    {
        var ccp = GetComponentInChildren<RoadCurveMarker>(false);
        if (null != ccp)
            cachedCurvePoint = ccp.transform;
    }

    public override Vector3 GetMovementPos()
    {
        if (null != cachedCurvePoint)
            return cachedCurvePoint.position + Vector3.up * .2f;
        else
            return base.GetMovementPos();
    }
}
