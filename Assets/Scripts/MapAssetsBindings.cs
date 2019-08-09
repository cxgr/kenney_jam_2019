using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MapAssetsBindings : ScriptableObject
{
    [Serializable]
    public class Binding
    {
        public char key;
        public GameObject prefab;
    }

    [SerializeField] private List<Binding> bindings = new List<Binding>();
    Dictionary<char, GameObject> bindingsDict = new Dictionary<char, GameObject>();
    public Dictionary<char, GameObject> prefabDict => bindingsDict;

    public void Init()
    {
        bindingsDict.Clear();
        foreach (var b in bindings)
        {
            if (!prefabDict.ContainsKey(b.key))
                prefabDict.Add(b.key, b.prefab);
            else
                Debug.LogError($"duplicate entry {b.key}");
        }
    }
}
