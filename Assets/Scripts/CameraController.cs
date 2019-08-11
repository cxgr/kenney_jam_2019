using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public float scrollSens;
	public float btnScrollSens = 25;
	public float rotSens = 25;
	Vector3 right;
	Vector3 forward;
	
	public Transform camT;
	
	public Vector2 zoomConstr;

	private Vector3 startPos;

	private float zoomFactor = .5f;

	public Transform listener;
	
	void Awake()
	{
		//right = (Vector3.back + Vector3.right).normalized;
		//forward = (Vector3.forward + Vector3.right).normalized;
		
		right = Vector3.right;
		forward = Vector3.forward;
		
		startPos = transform.position;
		
		camT.transform.localPosition = -camT.forward * Mathf.Lerp(zoomConstr.x, zoomConstr.y, zoomFactor);
	}
	
	void LateUpdate()
	{
		/*
		if (Input.GetKey(KeyCode.Q))
			transform.Rotate(Vector3.up * rotSens * Time.deltaTime, Space.World);
		if (Input.GetKey(KeyCode.E))
			transform.Rotate(-Vector3.up * rotSens * Time.deltaTime, Space.World);
			*/
		
		//right = Vector3.ProjectOnPlane(camT.right, Vector3.up);
		//forward = Vector3.ProjectOnPlane(camT.forward, Vector3.up);

		Vector3 btnDelta = Vector3.zero;
		if (Input.GetKey(KeyCode.A))
			btnDelta += -right;
		if (Input.GetKey(KeyCode.D))
			btnDelta += right;
		if (Input.GetKey(KeyCode.W))
			btnDelta += forward;
		if (Input.GetKey(KeyCode.S))
			btnDelta += -forward;
		btnDelta.y = 0f;
		btnDelta.Normalize();
		transform.Translate(btnDelta * scrollSens * Time.deltaTime, Space.World);

		zoomFactor -= Input.GetAxis("Mouse ScrollWheel");
		zoomFactor = Mathf.Clamp01(zoomFactor);
		camT.transform.localPosition = -camT.forward * Mathf.Lerp(zoomConstr.x, zoomConstr.y, zoomFactor);

		listener.transform.localPosition = -camT.forward * Mathf.Lerp(0f, zoomConstr.x, zoomFactor);

		var tmp = transform.position;
		tmp.x = Mathf.Clamp(tmp.x, startPos.x - xz.x, startPos.x + xz.x);
		tmp.z = Mathf.Clamp(tmp.z, startPos.z - xz.y, startPos.z + xz.y);
		transform.position = tmp;
	}

	public Vector2 xz;
	public bool g;
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(transform.position, .2f);
		if (!g) return;
		Gizmos.DrawWireCube(transform.position, new Vector3(xz.x, 1f, xz.y)); 
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
}
