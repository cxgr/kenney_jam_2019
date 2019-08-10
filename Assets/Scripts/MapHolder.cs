﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MapHolder : MonoBehaviour
{
    public string mapFilePath;
    public Transform mapRoot;
    public MapAssetsBindings bindings;
    public Dictionary<Tuple<int, int>, Tile> mapGraph = new Dictionary<Tuple<int, int>, Tile>();

    public Pathing pathing;

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
                        newTile.gameObject.name = newTile.gameObject.name.Replace("TILE", $"{row}:{col}");
                        newTile.EditorInit(row, col);
                    } 
                    col++;
                }

                row++;
                col = 0;
            }
        }
    }

    private void Awake()
    {
        var tiles = GetComponentsInChildren<Tile>(true);
        foreach (var t in tiles)
            mapGraph.Add(new Tuple<int, int>(t.row, t.col), t);
        
        pathing = new Pathing();
    }
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