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
	
	void Awake()
	{
		right = (Vector3.back + Vector3.right).normalized;
		forward = (Vector3.forward + Vector3.right).normalized;
	}
	
	void LateUpdate()
	{
		if (Input.GetKey(KeyCode.Q))
			transform.Rotate(Vector3.up * rotSens * Time.deltaTime, Space.World);
		if (Input.GetKey(KeyCode.E))
			transform.Rotate(-Vector3.up * rotSens * Time.deltaTime, Space.World);
		
		right = Vector3.ProjectOnPlane(camT.right, Vector3.up);
		forward = Vector3.ProjectOnPlane(camT.forward, Vector3.up);
		
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
	}
}
