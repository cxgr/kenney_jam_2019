using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadCurveMarker : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position + Vector3.up * .18f, .05f);
    }
}
