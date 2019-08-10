using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public Camera rayCam;
    public LayerMask rayMask;

    public Transform tileHighlight;

    private Tile t1;
    private Tile t2;
    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            tileHighlight.transform.position = Vector3.up * 100f;
            return;
        }

        var ray = rayCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var rHit, Mathf.Infinity, rayMask))
        {
            var tile = rHit.collider.GetComponent<Tile>();
            if (null != tile)
            {
                tileHighlight.transform.position = rHit.collider.transform.position;

                if (Input.GetMouseButtonDown(0))
                {
                    if (null == t1)
                        t1 = tile;
                    else
                    {
                        t2 = tile;

                        var path = SingletonUtils<MapHolder>.Instance.pathing.FindPath(t1, t2);
                        if (null != path)
                            foreach (var p in path)
                                Debug.Log(p);
                    }
                }
            }
        }
        else
            tileHighlight.transform.position = Vector3.up * 100f;
    }
}
