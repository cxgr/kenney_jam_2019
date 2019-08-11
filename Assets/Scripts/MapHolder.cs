using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MapHolder : SerializedMonoBehaviour
{
    public string mapFilePath;
    public Transform mapRoot;
    public MapAssetsBindings bindings;
    
    [OdinSerialize]
    public Dictionary<Tuple<int, int>, Tile> mapGraph = new Dictionary<Tuple<int, int>, Tile>();

    public Pathing pathing;

    public float carSlowdown = 3;
    public float performanceDropPerTick = 0.01f;
    public float performanceDropVIP = 0.05f;

    public float performanceDropPerCrash = 0.15f;
    public float performanceDropPerCrashVIP = 0.3f;

    public float performanceGainPerArrival = 0.1f;
    public float performanceGainPerArrivalVIP = 0.3f;

    public float angeryCooldown = .5f;
    public float angeryPerformanceLossPerTick = 0.01f;

    public float boostDuration = 3f;
    public float boostSpeedMul = 4f;
    public float boostAccelFactor = 8f;

    public void Generate()
    {
        bindings.Init();
        
        while (mapRoot.childCount > 0)
            DestroyImmediate(mapRoot.GetChild(0).gameObject);

        var row = 0;
        var col = 0;

        using (var reader = File.OpenText(mapFilePath))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                foreach (var c in line)
                {
                    if (bindings.prefabDict.ContainsKey(c))
                    {
                        var newTile = Instantiate(bindings.prefabDict[c], new Vector3(col, 0f, -row), Quaternion.identity, mapRoot).GetComponent<Tile>();
                        newTile.gameObject.SetActive(true);
                        newTile.gameObject.name = newTile.gameObject.name.Replace("TILE", $"{row}:{col}");
                        newTile.EditorInit(row, col);
                    } 
                    col++;
                }

                row++;
                col = 0;
            }
        }

        var tmpGraph = new Dictionary<Tuple<int, int>, Tile>();
        var tiles = GetComponentsInChildren<Tile>(true);
        foreach (var t in tiles)
            tmpGraph.Add(new Tuple<int, int>(t.row, t.col), t);
        foreach (var tile in tmpGraph.Values)
            if (tile is TileRoad)
                (tile as TileRoad).ResolveGraphics(tmpGraph);
    }

    private void Awake()
    {
        var tiles = GetComponentsInChildren<Tile>(true);
        foreach (var t in tiles)
            mapGraph.Add(new Tuple<int, int>(t.row, t.col), t);
        
        pathing = new Pathing();
    }

    public LayerMask probeMask;
    public float probeLen;
}

#if UNITY_EDITOR
[CustomEditor(typeof(MapHolder))]
public class Editor_AEO_Waterfall : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MapHolder mh = target as MapHolder;
        if (GUILayout.Button("Generate"))
            mh.Generate();
    }
}
#endif