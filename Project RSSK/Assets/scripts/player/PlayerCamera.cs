using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerCamera : NetworkBehaviour
{

	private Transform playerTran;
	private Transform playerCam;

	private Quaternion turnFrom;
	private Quaternion turnTo;

	private Vector3 startCam;

	public float camSensativity = 1;

	public float lookUpLim = 270;
	public float lookDownLim = 90;

	public float rotationSpeed = 50;

	private float heightScale = 1;
	private float speed = 1;

	//vector2 contains the height sway at .x and speed as .y
	public Vector2 idleSway, runSway, jumpSway, fallingSway, rollSway, climbSway, wallRunSway;

	// Use this for initialization
	void Start()
	{
		playerTran = transform;
		playerCam = transform.FindChild("camera");

		startCam = playerCam.transform.localPosition;
	}

	// Update is called once per frame
	void Update()
	{
		if (!isLocalPlayer)
			return;

		rotateCam();
		if (Input.GetButtonDown("QuickTurn"))
			playerTran.Rotate(new Vector3(0, 180, 0));

		float height = heightScale * Mathf.PerlinNoise(Time.time * speed, 0.0F);
		Vector3 pos = playerCam.transform.localPosition;
		pos.y = height;
		playerCam.transform.localPosition = pos;
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

	public void setSway(PlayerState state)
	{
		switch (state)
		{
			case PlayerState.idle:
				heightScale = idleSway.x;
				speed = idleSway.y;
				break;
			case PlayerState.run:
				heightScale = runSway.x;
				speed = runSway.y;
				break;
			case PlayerState.jump:
				heightScale = jumpSway.x;
				speed = jumpSway.y;
				break;
			case PlayerState.falling:
				heightScale = fallingSway.x;
				speed = fallingSway.y;
				break;
			case PlayerState.roll:
				heightScale = rollSway.x;
				speed = rollSway.y;
				break;
			case PlayerState.climb:
				heightScale = climbSway.x;
				speed = climbSway.y;
				break;
			case PlayerState.wallRun:
				heightScale = wallRunSway.x;
				speed = wallRunSway.y;
				break;
			default:
				break;
		}
	}

	public void setSway(float heightScale, float speed)
	{
		this.heightScale = heightScale;
		this.speed = speed;
	}
}

