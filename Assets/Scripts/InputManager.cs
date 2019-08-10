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

    public bool isHolding = false;
    public float holdingTimer;
    public float evacTime = 1f;
    
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
                {
                    var result = car.ToggleEngine();
                    if (result)
                        isHolding = true;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    StopHolding();
                }

                if (isHolding)
                {
                    if (holdingTimer >= evacTime)
                    {
                        car.HandleEvac();
                        StopHolding();
                    }
                    else
                        holdingTimer += Time.deltaTime;
                }
            }
            else
            {
                StopHolding();
            }
        }
        else
        {
            StopHolding();
            tileHighlight.transform.position = Vector3.up * 100f;
        }
    }

    void StopHolding()
    {
        holdingTimer = 0f;
        isHolding = false;
        SingletonUtils<UiManager>.Instance.UpdateEvac(0f);
    }
}
