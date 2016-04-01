using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerCamera : NetworkBehaviour
{
	private Transform playerTran;
	private Transform playerCam;

	public float camSensativity = 1;

	public float lookUpLim = 320;
	public float lookDownLim = 45;

	public float defaultFOV = 60;
	public float aimFOV = 30;

	// Use this for initialization
	void Start()
	{
		if (!isLocalPlayer)
			return;
		playerTran = this.transform;
		playerCam = transform.FindChild("camera");

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	// Update is called once per frame
	void Update()
	{
		if (!isLocalPlayer)
			return;
			
		rotateCamera();
		lockMouse();
		checkAim();
	}

	void rotateCamera()
	{
		playerTran.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * camSensativity, 0), Space.World);
		float delta = Input.GetAxis("Mouse Y") * camSensativity;

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

	void checkAim()
	{
		if (Input.GetButtonDown("Aim"))
		{
			Camera.main.fieldOfView = aimFOV;
		}
		else if (Input.GetButtonUp("Aim"))
		{
			Camera.main.fieldOfView = defaultFOV;
		}
	}
}

