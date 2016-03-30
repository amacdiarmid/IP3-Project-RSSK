﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerCamera : NetworkBehaviour
{
	private Transform playerTran;
	private Transform playerCam;

	public float camSensativity = 1;

	public float lookUpLim = 320;
	public float lookDownLim = 45;

	// Use this for initialization
	void Start()
	{
		playerTran = this.transform;
		playerCam = transform.FindChild("camera");

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	// Update is called once per frame
	void Update()
	{
		if (isLocalPlayer)
			rotateCamera();

		lockMouse();
	}

	void rotateCamera()
	{
		playerTran.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * camSensativity, 0), Space.World);
		float delta = Input.GetAxis("Mouse Y") * camSensativity;

		Debug.Log((playerCam.rotation.eulerAngles.x + delta));

		if (playerCam.rotation.eulerAngles.x - delta > lookUpLim || playerCam.rotation.eulerAngles.x - delta < lookDownLim)
			playerCam.Rotate(new Vector3(-delta, 0, 0));

	}

	void lockMouse()
	{
		if (Input.GetKeyUp(KeyCode.BackQuote))
		{
			if (Cursor.visible)
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
			else
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}
	}
}

