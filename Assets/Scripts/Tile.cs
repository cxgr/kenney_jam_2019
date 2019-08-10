using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tile : SerializedMonoBehaviour
{
    public int row;
    public int col;
    
    public bool randomizeRot;

    public bool randomizeContent;
    public GameObject[] randomContent;

    public bool isTraversible = false;
    
    public Tuple<int, int> coordTuple => new Tuple<int, int>(row, col);

    public override string ToString() => $"r: {row} c: {col} t: {isTraversible}";

    protected MapHolder map => SingletonUtils<MapHolder>.Instance;

    public virtual Vector3 GetMovementPos()
    {
        return transform.position + new Vector3(.5f, .2f, -.5f);
    }

    public virtual void EditorInit(int row, int col)
    {
        this.row = row;
        this.col = col;

        if (randomizeRot)
            transform.GetChild(0).rotation = Quaternion.Euler(90f * Random.Range(0, 4) * Vector3.up);

        if (randomizeContent)
        {
            foreach (var c in randomContent)
                c.gameObject.SetActive(false);
            randomContent[Random.Range(0, randomContent.Length)].SetActive(true);
        }
    }
}
