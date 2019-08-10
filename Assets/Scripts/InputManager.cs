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
            var car = rHit.collider.GetComponent<VehicleController>();
            if (null != car)
            {
                if (Input.GetMouseButtonDown(0))
                    car.HandleTap();
            }
        }
        else
            tileHighlight.transform.position = Vector3.up * 100f;
    }
}
