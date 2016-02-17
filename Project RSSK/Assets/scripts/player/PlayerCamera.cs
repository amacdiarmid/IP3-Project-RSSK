using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerCamera : NetworkBehaviour {

	private Transform playerTran;
	private Transform playerCam;

	private bool isQuickTurn;
	private Quaternion turnFrom;
	private Quaternion turnTo;

	public float camSensativity = 1;

	public float lookUpLim = 270;
	public float lookDownLim = 90;

	public float rotationSpeed = 50;

	// Use this for initialization
	void Start ()
	{
		isQuickTurn = false;
		playerTran = this.transform;
		playerCam = transform.FindChild("camera");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(isLocalPlayer)
			rotateCam();

		if (Input.GetButtonDown("QuickTurn"))
		{
			Debug.Log("quck turn down");
			this.transform.Rotate(new Vector3(0, 180, 0));
			//isQuickTurn = true;
			//turnFrom = playerTran.rotation;
			//turnTo = new Quaternion(playerTran.rotation.x, playerTran.rotation.y + 180, playerTran.rotation.z, playerTran.rotation.w);
		}

		//if (isQuickTurn)
		//{
		//	playerTran.rotation = Quaternion.Lerp(turnFrom, turnTo, Time.deltaTime * rotationSpeed);
		//	if (playerTran.rotation == turnTo)
		//	{
		//		isQuickTurn = false;
		//	}
		//}
	}

	void rotateCam()
	{
		playerTran.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * camSensativity, 0), Space.World);
		float delta = Input.GetAxis("Mouse Y") * camSensativity;
		
		if (playerTran.rotation.eulerAngles.z + delta < lookUpLim || playerTran.rotation.eulerAngles.z + delta > lookDownLim)
			playerCam.Rotate(new Vector3(-delta, 0, 0));
		//Debug.Log(playerTran.rotation.eulerAngles.z);
		
		//else if (playerTran.rotation.x < lookDownLim)
		//{
		//	playerTran.rotation.Set(lookDownLim, playerTran.rotation.y, playerTran.rotation.z, playerTran.rotation.w);
		//}
	}

	public void setCamera(float angle)
	{
		this.transform.Rotate(new Vector3(0, angle, 0));
	}

}
