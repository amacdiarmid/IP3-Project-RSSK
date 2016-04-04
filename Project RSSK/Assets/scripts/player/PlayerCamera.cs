using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public enum camPos
{
	left, 
	right, 
	sprint, 
	run,
}


public class PlayerCamera : NetworkBehaviour
{
	private Transform playerTran;
	private Transform playerCam;

	public float camSensativity = 1;

	public float lookUpLim = 320;
	public float lookDownLim = 45;

	public float defaultFOV = 60;
	public float aimFOV = 30;

	public float shakeSpeed = 0.25f;
	public float distanceClamp = 0.5f;
	public float sprintSpeedMult = 3;
	public float sprintClampMult = 1;
	public float aimSpeedMult = 0.5f;
	public float aimClampMult = 0.5f;
	public bool camShake = true;
	private Vector2 shakeVals = new Vector2(0, 0);
	private float PerlinTime = 0;
	private float curDamageShake = 0;
	public float damageShaceVal = 3;
	public float damageShakeDep = 1;

	private Vector3 startingPos;
	public float sprintPos;
	private Vector3 newPos;
	public float speed = 1;
	private float startTime;
	private float journeyLength;
	private camPos curCamSide;
	private camPos curCamFor;

	private bool move;

	// Use this for initialization
	void Start()
	{
		if (!isLocalPlayer)
			return;
		playerTran = this.transform;
		playerCam = transform.FindChild("camera");

		curCamSide = camPos.right;
		curCamFor = camPos.run;
		move = false;
		startingPos = playerCam.localPosition;

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
		checkInput();
		if (move)
			moveCam();
		else if (camShake)
			shakeCam();

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

	void checkInput()
	{
		if (Input.GetButtonDown("Aim"))
		{
			Camera.main.fieldOfView = aimFOV;
		}
		else if (Input.GetButtonUp("Aim"))
		{
			Camera.main.fieldOfView = defaultFOV;
		}


		if (Input.GetButtonDown("ChangeCam"))
		{
			if (curCamSide == camPos.right)
				changeSide(camPos.left);
			else
				changeSide(camPos.right);

		}
	}

	public void changeSide(camPos pos)
	{
		move = true;
		PerlinTime = 0;
		Vector3 curPos = playerCam.transform.localPosition;
		startTime = Time.time;
		switch (pos)
		{
			case camPos.left:
				newPos = new Vector3(-startingPos.x, curPos.y, curPos.z);
				curCamSide = camPos.left;
				break;
			case camPos.right:
				newPos = new Vector3(startingPos.x, curPos.y, curPos.z);
				curCamSide = camPos.right;
				break;
			case camPos.sprint:
				newPos = new Vector3(curPos.x, curPos.y, sprintPos);
				curCamFor = camPos.sprint;
				break;
			case camPos.run:
				newPos = new Vector3(curPos.x, curPos.y, startingPos.z);
				curCamFor = camPos.run;
				break;
			default:
				break;
		}
		journeyLength = Vector3.Distance(curPos, newPos);
	}

	void moveCam()
	{
		float distCovered = (Time.time - startTime) * speed;
		float fracJourney = distCovered / journeyLength;
		playerCam.localPosition = Vector3.Lerp(playerCam.transform.localPosition, newPos, fracJourney);
		//Debug.Log(fracJourney);
		if (fracJourney >= 0.2f)
		{
			move = false;
		}
	}

	public camPos getCamFor()
	{
		return curCamFor;
	}

	public camPos getCamSide()
	{
		return curCamSide;
	}

	public Vector3 getCamPos()
	{
		Vector3 pos = startingPos;

		if(curCamFor == camPos.sprint)
			pos = new Vector3(pos.x, pos.y, sprintPos);
		if(curCamSide == camPos.left)
			pos = new Vector3(-pos.x, pos.y, pos.z);
		return pos;
	}

	public void shakeCam()
	{
		float curClamp = distanceClamp;
		float curSpeed = shakeSpeed + curDamageShake;

		if (Input.GetButton("Aim"))
		{
			curClamp = curClamp * aimClampMult;
			curSpeed = curSpeed * aimSpeedMult;
		}
		if (Input.GetButton("Sprint"))
		{
			curClamp = curClamp * sprintClampMult;
			curSpeed = curSpeed * sprintSpeedMult;
		}


		shakeVals.x = curClamp * (Mathf.PerlinNoise(PerlinTime * curSpeed, 0.0f) * 2 - 1);
		shakeVals.y = curClamp * (Mathf.PerlinNoise(0.0f, PerlinTime * curSpeed) * 2 - 1);
		//Debug.Log("shake " + shakeVals);
		PerlinTime = PerlinTime + Time.deltaTime;

		Vector3 pos = startingPos;

		if (curCamFor == camPos.sprint)
			pos.z = sprintPos;
		if (curCamSide == camPos.left)
			pos.x = -pos.x;

		playerCam.transform.localPosition = pos + (Vector3)shakeVals;

		if (curDamageShake != 0)
			depShakeSpeed();
	}

	public Vector2 getShakeVals()
	{
		return shakeVals;
	}

	public void damShake()
	{
		curDamageShake = damageShaceVal;
	}

	void depShakeSpeed()
	{
		curDamageShake = curDamageShake - (damageShakeDep * Time.deltaTime);
		if (curDamageShake < 0)
			curDamageShake = 0;
	}
}

