using System;
using System.Collections;
using System.Collections.Generic;
using AStar;
using UnityEngine;

public class Pathing : AStarPathfinder<Tile>
{
    protected override void FindNeighbors(Tile cell)
    {
        var map = SingletonUtils<MapHolder>.Instance.mapGraph;
        
        var nbrCoords = new List<Tuple<int, int>>();
        nbrCoords.Add(new Tuple<int, int>(cell.row - 1, cell.col));
        nbrCoords.Add(new Tuple<int, int>(cell.row + 1, cell.col));
        nbrCoords.Add(new Tuple<int, int>(cell.row, cell.col - 1));
        nbrCoords.Add(new Tuple<int, int>(cell.row, cell.col + 1));

        foreach (var c in nbrCoords)
            if (map.ContainsKey(c) && map[c].isTraversible)
                AddNeighbor(map[c]);
    }
}
