using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int row;
    public int col;
    
    public bool randomizeRot;
    
    public void Init(int row, int col)
    {
        this.row = row;
        this.col = col;
        
        if (randomizeRot)
            transform.GetChild(0).rotation = 
                Quaternion.Euler(90f * Random.Range(0, 4) * Vector3.up);
    }
}
