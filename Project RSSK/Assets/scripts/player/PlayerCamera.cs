using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerCamera : NetworkBehaviour {

	private Transform playerTran;
	public float camSensativity = 1;

	public float lookUpLim = 270;
	public float lookDownLim = 90;

	// Use this for initialization
	void Start ()
	{
		playerTran = this.transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
        if(isLocalPlayer)
		    rotateCam();
	}

	void rotateCam()
	{
		playerTran.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * camSensativity, 0), Space.World);
		float delta = Input.GetAxis("Mouse Y") * camSensativity;
		
		if (playerTran.rotation.eulerAngles.z + delta < lookUpLim || playerTran.rotation.eulerAngles.z + delta > lookDownLim)
			playerTran.Rotate(new Vector3(0, 0, delta));
		//Debug.Log(playerTran.rotation.eulerAngles.z);
		
		//else if (playerTran.rotation.x < lookDownLim)
		//{
		//	playerTran.rotation.Set(lookDownLim, playerTran.rotation.y, playerTran.rotation.z, playerTran.rotation.w);
		//}
	}

}
