﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public enum PlayerState : byte
{
	idle,
	run,
	jump,
	falling, 
	roll,
	climb,
	wallRun
}

public enum PlayerTeam : byte
{
    NotPicked,
    TeamYellow,
    TeamBlue
}

public class PlayerController : NetworkBehaviour
{
    public static PlayerController localInstance = null;

    [HideInInspector, SyncVar]
    public PlayerTeam team = PlayerTeam.NotPicked;
    [HideInInspector, SyncVar]
    public short id = -1;

    public bool controllable = true;

	//enum and components
	PlayerState curState = PlayerState.idle;
	Transform playerTran;
	CharacterController charContr;
	PlayerCamera playerCam;
	PlayerAudioController playerAudio;
	float timer;
	Vector3 inputHeading;
	Vector3 curVel = Vector3.zero;
	Vector3 curPos;

	public Animator playerAni;

	//general state info
	bool touchingWall;
	bool canDoubleJump;
	bool transitioned;
	Vector3 wallNormal = Vector3.zero;

	//jumping
	public float jumpHeight = 10;

	//moving 
	public float runSpeed = 5;
	public float sprintSpeed = 10;
	public float airStrafe = 10;
	public float decelRate = 4;

	//rolling
	public float rollTimer = 1;
	public float rollSpeed = 5;

	//wall running
	public float wallrunSpeed = 10;
	public float angleToWallrun = 10;
	public float maxDistanceToWall = 2;
	
	//climbing
	public float climbSpeed = 5;
	public float climbTimer = 2;


	// Use this for initialization
	void Start ()
	{
		playerTran = transform;
		charContr = GetComponent<CharacterController>();
		playerAudio = GetComponent<PlayerAudioController>();
		playerCam = GetComponent<PlayerCamera>();

		playerCam.setSway(PlayerState.idle);

        if(controllable)
        {
            playerTran.FindChild("camera").gameObject.GetComponent<Camera>().enabled = isLocalPlayer;
            playerTran.FindChild("camera").gameObject.GetComponent<AudioListener>().enabled = isLocalPlayer;
        }

        if (team != PlayerTeam.NotPicked)
            GetComponent<Renderer>().material.color = team == PlayerTeam.TeamYellow ? Color.yellow : Color.blue;

        if (isLocalPlayer)
            localInstance = this;
    }

    // Update is called once per frame
    void Update()
	{
		if (!isLocalPlayer || !controllable)
			return;

		//Debug.LogWarning(curState);
		//collecting general info required for state decision making
		curPos = transform.TransformPoint(charContr.center);
		transitioned = false;
		inputHeading = playerTran.right * Input.GetAxisRaw("Horizontal") + playerTran.forward * Input.GetAxisRaw("Vertical");
		inputHeading.Normalize();
		touchingWall = checkWall();
		if (charContr.isGrounded) //reset double jump on ground touch
		{
			canDoubleJump = true;
			curVel.y = 0;
		}

		setState(curState);
		curVel += Physics.gravity * Time.deltaTime;
		if(inputHeading == Vector3.zero)
		{
			Vector3 curVelXZ = new Vector3(curVel.x, 0, curVel.z);
			curVel -= curVelXZ.normalized * decelRate;
		}
		charContr.Move(curVel * Time.deltaTime);
	}

	bool checkWallAngleForClimb()
	{
		Vector2 lookDir = new Vector2(playerTran.forward.x, playerTran.forward.z);
		float angleToWall = Mathf.Rad2Deg * Mathf.Acos(Vector2.Dot(lookDir, wallNormal));
		Debug.Log(angleToWall);
		return angleToWall > 180 - angleToWallrun;
	}
	
	bool checkWall()
	{
#warning Redo this using the OverlapSphere
		RaycastHit hit;
		Ray ray = new Ray(curPos, playerTran.forward);
		if (Physics.Raycast(ray, out hit, maxDistanceToWall))
		{
			Debug.DrawLine(curPos, hit.point, Color.magenta, 15);
			Debug.DrawLine(curPos + (hit.point - curPos) * 0.9f, hit.point, Color.blue, 15);
			Debug.DrawLine(hit.point, hit.point + hit.normal, Color.red, 15);
			Debug.DrawLine(hit.point + hit.normal, hit.point + hit.normal * 0.9f, Color.blue, 15);
			wallNormal = hit.normal;
			return true;
		}
		ray = new Ray(curPos, playerTran.right);
		if (Physics.Raycast(ray, out hit, maxDistanceToWall))
		{
			Debug.DrawLine(curPos, hit.point, Color.cyan, 15);
			Debug.DrawLine(curPos + (hit.point - curPos) * 0.9f, hit.point, Color.blue, 15);
			wallNormal = hit.normal;
			return true;
		}
		ray = new Ray(curPos, -playerTran.right);
		if (Physics.Raycast(ray, out hit, maxDistanceToWall))
		{
			Debug.DrawLine(curPos, hit.point, Color.cyan, 15);
			Debug.DrawLine(curPos + (hit.point - curPos) * 0.9f, hit.point, Color.blue, 15);
			wallNormal = hit.normal;
			return true;
		}
		wallNormal = Vector3.zero;
		return false;
	}

	void setState(PlayerState tempState)
	{
		if (tempState != curState) //can only transition once
		{
			if (!transitioned)
				transitioned = true;
			else
				return;
		}
		
		switch (tempState)
		{
			case PlayerState.idle:
				Idle();
				break;
			case PlayerState.run:
				Run();
				break;
			case PlayerState.jump:
				Jump();
				break;
			case PlayerState.falling:
				Falling();
				break;
			case PlayerState.roll:
				Roll();
				break;
			case PlayerState.climb:
				Climb();
				break;
			case PlayerState.wallRun:
				Wallrun();
				break;
			default:
				break;
		}
	}

	//not doing anything 
	void Idle()
	{
		if (curState != PlayerState.idle)
		{
			playerAni.SetTrigger("startIdle");
			curState = PlayerState.idle;
			playerAudio.setAudio(PlayerState.idle);
			playerCam.setSway(PlayerState.idle);
		}

		curVel = new Vector3(0, curVel.y, 0);

		if (inputHeading != Vector3.zero)
			setState(PlayerState.run);
		if (Input.GetButtonDown("Jump"))
			setState(PlayerState.jump);
		if (Input.GetButtonDown("Slide"))
			setState(PlayerState.roll);
	}

	//one off jumps that add instant force - transitionary state
	void Jump()
	{
		if(curState != PlayerState.jump)
		{
			playerAni.SetTrigger("startJump");
			curVel.y = jumpHeight;
			playerAudio.setAudio(PlayerState.jump);
			curState = PlayerState.jump;
			playerCam.setSway(PlayerState.jump);
		}

		//if there's a wall next to us
		if(Input.GetButton("Sprint") && touchingWall)
		{
			if (checkWallAngleForClimb()) //then if we're looking at it - climb it
				setState(PlayerState.climb);
			else // else start wall running
				setState(PlayerState.wallRun);
		}
		else //otherwise, transition to falling
			setState(PlayerState.falling);
	}

	//setting the players current speed
	void Run()
	{
		//transitioning in
		if(curState != PlayerState.run)
		{
			playerAni.SetTrigger(Input.GetButton("Sprint") ? "startSprint" : "startRun");
			curState = PlayerState.run;
			playerAudio.setAudio(PlayerState.run);
			playerCam.setSway(PlayerState.run);
		}

		float speed = Input.GetButton("Sprint") ? sprintSpeed : runSpeed;
		float yVel = curVel.y;
		curVel = inputHeading * speed;
		curVel.y = yVel;

		//transitioning out
		if (inputHeading == Vector3.zero)
			setState(PlayerState.idle);
		else if (Input.GetButtonDown("Slide"))
			setState(PlayerState.roll);
		else if (Input.GetButtonDown("Jump"))
			setState(PlayerState.jump);
		else if (!charContr.isGrounded)
			setState(PlayerState.falling);
	}

	void Falling()
	{
		
		if(curState != PlayerState.falling)
		{
			Debug.Log("falling trigger");
			playerAni.SetTrigger("startFalling");
			curState = PlayerState.falling;
			playerCam.setSway(PlayerState.falling);
		}

		float yVel = curVel.y;
		curVel = Vector3.Lerp(curVel, inputHeading * airStrafe, 0.5f);
		curVel.y = yVel;

		//keep falling until we double jump or land
		if (canDoubleJump && Input.GetButtonDown("Jump"))
		{
			setState(PlayerState.jump);
			canDoubleJump = false;
		}
		else if (charContr.isGrounded)
		{
			if (inputHeading != Vector3.zero)
				setState(PlayerState.run);
			else
				setState(PlayerState.idle);
		}
		else if (touchingWall && Input.GetButton("Sprint"))
			setState(PlayerState.wallRun);
	}

	void Roll()
	{
		//transitioning in
		if (curState != PlayerState.roll)
		{
			playerAni.SetTrigger("startRoll");
			curState = PlayerState.roll;
			timer = rollTimer;
			curVel = inputHeading * rollSpeed;
			playerCam.setSway(PlayerState.roll);
		}

		Debug.LogWarning("Roll");
		timer -= Time.deltaTime;
		if (timer <= 0)
			setState(PlayerState.idle);
		else if (!charContr.isGrounded)
			setState(PlayerState.falling);
	}

	void Wallrun()
	{
		if(curState != PlayerState.wallRun)
		{
			playerAni.SetTrigger("startWallRun");
			curState = PlayerState.wallRun;
			if(curVel.y < 0)
				curVel.y = 0; //lose the fall speed
			playerCam.setSway(PlayerState.wallRun);
		}

		Vector3 dir1 = Vector3.Cross(Vector3.up, wallNormal); //dir1 and dir2 are both orthogonal to the normal
		Vector3 dir2 = Vector3.Cross(wallNormal, Vector3.up); //but we need to pick one which is the closest to our forward
		float mag1 = (dir1 - playerTran.forward).sqrMagnitude; //we do it by comparing their differences
		float mag2 = (dir2 - playerTran.forward).sqrMagnitude; //the one with a smaller difference is our new "forward"
		Vector3 wallrunDir = mag1 < mag2 ? dir1 : dir2;
		float yVel = curVel.y;
		curVel = wallrunDir * wallrunSpeed;
		curVel.y = yVel;
		Debug.DrawLine(curPos, curPos + wallrunDir, Color.green, 10);

		if (charContr.isGrounded)
		{
			if (inputHeading == Vector3.zero)
				setState(PlayerState.idle);
			else
				setState(PlayerState.run);
		}
		else if (Input.GetButtonDown("Jump"))
		{
			curVel = wallNormal * jumpHeight / 2;
			setState(PlayerState.jump);
		}
		else if (!Input.GetButton("Sprint") || !touchingWall || Input.GetAxisRaw("Horizontal") != 0)
			setState(PlayerState.falling);
	}

	void Climb()
	{
        if(curState != PlayerState.climb)
        {
            curState = PlayerState.climb;
            playerAni.SetTrigger("startClimb");
            timer = climbTimer;
		playerCam.setSway(PlayerState.climb);
        }

        curVel = playerTran.up * climbSpeed * timer / climbTimer;
        timer -= Time.deltaTime;
        if (Input.GetButton("Sprint") || timer < 0 || Input.GetAxisRaw("Vertical") < 0)
            setState(PlayerState.falling);
        else if (Input.GetButtonDown("Jump"))
        {
            curVel = wallNormal * jumpHeight / 2;
            setState(PlayerState.jump);
        }
    }
    
    public void UpdateTeam()
    {
        if(team != PlayerTeam.NotPicked)
            GetComponent<Renderer>().material.color = team == PlayerTeam.TeamYellow ? Color.yellow : Color.blue;
    }

    [Command]
    public void CmdUpdateTeam()
    {
        if (team != PlayerTeam.NotPicked)
            GetComponent<Renderer>().material.color = team == PlayerTeam.TeamYellow ? Color.yellow : Color.blue;
    }
}