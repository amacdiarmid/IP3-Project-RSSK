using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour {

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
		rotateCam();
	}

	void rotateCam()
	{
		//if (Input.GetAxis("Mouse X") < 360 && Input.GetAxis("Mouse X") > 270)
		//{
		//	playerTran.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * camSensativity, 0), Space.World);
		//	playerTran.Rotate(new Vector3(-Input.GetAxis("Mouse Y") * camSensativity, 0, 0));
		//}
		//else if (Input.GetAxis("Mouse X") > 0 && Input.GetAxis("Mouse X") < 270)
		//{
		//	playerTran.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * camSensativity, 0), Space.World);
		//	playerTran.Rotate(new Vector3(-Input.GetAxis("Mouse Y") * camSensativity, 0, 0));
		//}
		playerTran.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * camSensativity, 0), Space.World);
		playerTran.Rotate(new Vector3(-Input.GetAxis("Mouse Y") * camSensativity, 0, 0));
	}

}
