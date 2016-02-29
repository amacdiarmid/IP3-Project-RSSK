using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerCamera : NetworkBehaviour {

	private Transform playerTran;
	private Transform playerCam;

	private Quaternion turnFrom;
	private Quaternion turnTo;

	public float camSensativity = 1;

	public float lookUpLim = 270;
	public float lookDownLim = 90;

	public float rotationSpeed = 50;

	// Use this for initialization
	void Start ()
	{
		playerTran = transform;
		playerCam = transform.FindChild("camera");
	}
	
	// Update is called once per frame
	void Update ()
	{
        if (!isLocalPlayer)
            return;

		rotateCam();
		if (Input.GetButtonDown("QuickTurn"))
			playerTran.Rotate(new Vector3(0, 180, 0));
	}

	void rotateCam()
	{
		playerTran.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * camSensativity, 0), Space.World);
		float delta = Input.GetAxis("Mouse Y") * camSensativity;
		
		if (playerTran.rotation.eulerAngles.z + delta < lookUpLim || playerTran.rotation.eulerAngles.z + delta > lookDownLim)
			playerCam.Rotate(new Vector3(-delta, 0, 0));
	}

	public void setCamera(float angle)
	{
		transform.Rotate(new Vector3(0, angle, 0));
	}
}
