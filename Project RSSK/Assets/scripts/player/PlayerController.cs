using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public enum PlayerState
{
	idle, 
	walk,
	run, 
	sprint,
	jump, 
	doubleJump,
	falling, 
	roll, 
	lunge,
	climb,
	backEject,
	wallRun,
	wallJump,
}

public class PlayerController : NetworkBehaviour {

	//enum and components
	public PlayerState curState;
	public Animator playerAni;
	private Transform playerTran;
	private Rigidbody playerRidg;
	private PlayerCamera playerCam;
	private PlayerAudioController playerAudio;
	private bool lockCamera;
	private bool lockMovement;

	//jumping
	private bool canDoubleJump;
	private float previousGroundDis;
	public float jumpHeight = 500;

	//moving 
	private float curSpeed;
	private Vector3 targetVelocity;
	public float walkSpeed = 5;
	public float runSpeed = 10;
	public float sprintSpeed = 25;
	public float fallingSpeed = 0;
	public float maxVelocityChange = 10;

	//sliding
	private float curSlideSpeed;
	public float runRollSpeed = 30;
	public float sprintRollSpeed = 50;
	public float slideDep = 1;

	//lunging
	private Vector3 lungePos;
	private float meleeRange;
	public float lungeSpeed = 25;

	//wall climbing
	private float curClimbSpeed;

	//wall running
	private float curWallSpeed;
	private float curWallRunHeight;
	private float curWallRunLength;
	private bool canWallRun;
	private float wallRunCooldownTime = 0;
	public float wallRunningSpeed = 25;
	public float angleToWallRun = 10;
	public float runWallSpeed = 10;
	public float sprintWallSpeed = 15;
	public float wallHeightDep = 1;
	public float wallLengthDep = 1;
	public float wallRunHeight = 10;
	public float wallRunLength = 5;
	public float maxDistanceToWall = 3;
	public float wallRunCooldown = 1;
	

	//climbing
	public float climbSpeed = 20;
	public float climbDep = 1;
	public float backEjectHeight = 300;
	

	// Use this for initialization
	void Start ()
	{
		curState = PlayerState.idle;

		playerTran = this.transform;
		playerRidg = this.GetComponent<Rigidbody>();
		playerCam = this.GetComponent<PlayerCamera>();
		playerAudio = this.GetComponent<PlayerAudioController>();

		this.transform.FindChild("camera").gameObject.GetComponent<Camera>().enabled = isLocalPlayer;
		this.transform.FindChild("camera").gameObject.GetComponent<AudioListener>().enabled = isLocalPlayer;

		canDoubleJump = false;
		lockCamera = false;
		lockMovement = false;
		canWallRun = true;

		this.gameObject.SetActive(true);

		//remove when we find out spawn points
		transform.position = new Vector3(0, 3, 0);
	}

	// Update is called once per frame
	void Update ()
	{
		if (!isLocalPlayer)
			return;

		if (!lockCamera)
		{
			targetVelocity = new Vector3(Input.GetAxis("Vertical"), 0, -Input.GetAxis("Horizontal")); 
			targetVelocity = playerTran.TransformDirection(targetVelocity);
		}

		if (curState == PlayerState.wallJump && checkWall() && canWallRun)
		{
			setState(PlayerState.wallRun);
		}
		else if (!canWallRun)
		{
			canWallRun = (wallRunCooldownTime += Time.deltaTime) >= wallRunCooldown;
		}
		else if (curState == PlayerState.jump || curState == PlayerState.doubleJump || curState == PlayerState.backEject || curState == PlayerState.wallJump)
		{
			if (previousGroundDis > playerTran.position.y)
			{
				setState(PlayerState.falling);
			}
			else
			{
				previousGroundDis = playerTran.position.y;
			}
		}
		else if (curState == PlayerState.falling)
		{
			if (previousGroundDis == playerTran.position.y)
			{
				setState(PlayerState.idle);
			}
			else
			{
				previousGroundDis = playerTran.position.y;
			}
		}

		if (!lockMovement)
		{
			checkInput();
			if (curState == PlayerState.roll)
			{
				Rolling();
			}
			else if (curState == PlayerState.climb)
			{
				climbing();
			}
			else if (curState == PlayerState.wallRun)
			{
				wallRunning();
			}
			else
			{
				moving();
			}	
		}
		else
		{
			if (curState == PlayerState.lunge)
			{
				lunging();
			}
		}
	}

	void checkInput()
	{
		if (Input.GetButtonDown("Jump"))
		{
			if (curState == PlayerState.wallRun)
			{
				setState(PlayerState.wallJump);
			}
			else if (checkWall() && (curState == PlayerState.run || curState == PlayerState.sprint || curState == PlayerState.jump || curState == PlayerState.doubleJump || curState == PlayerState.wallJump))
			{
				checkWallAngle();
			}
			else if (curState == PlayerState.walk || curState == PlayerState.run || curState == PlayerState.sprint || curState == PlayerState.idle)
			{
				if(checkGround())
				{
					setState(PlayerState.jump);
					previousGroundDis = playerTran.position.y;
				}
			}
			/*
			Debug.Log(curState);
			if (curState == PlayerState.wallRun)
			{
				Debug.Log("set wall jump");
				setState(PlayerState.wallJump);
			}
			else if (curState == PlayerState.climb)
			{
				Debug.Log("set backEject");
				setState(PlayerState.backEject);
			}
			else if (canClimb && curState != PlayerState.idle && Input.GetAxis("Vertical") == 1)
			{
				checkWallAngle();
			}
			else if (canJump)
			{
				setState(PlayerState.jump);
			}
			else if (canDoubleJump)
			{
				setState(PlayerState.doubleJump);
			}
			*/
		}
		else if (Input.GetButtonDown("Walk"))
		{
			if (curState == PlayerState.run || curState == PlayerState.sprint)
			{
				setState(PlayerState.walk);
			}
			else if (curState == PlayerState.walk)
			{
				setState(PlayerState.run);
			}
		}
		else if (Input.GetButtonDown("Sprint"))
		{
			if (curState == PlayerState.walk || curState == PlayerState.run)
			{
				setState(PlayerState.sprint);
			}
			else if (curState == PlayerState.sprint)
			{
				setState(PlayerState.run);
			}
		}
		else if (Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical"))
		{
			if (curState == PlayerState.idle && curState != PlayerState.run)
			{
				setState(PlayerState.run);
			}
		}
		else if (Input.GetButtonUp("Slide"))
		{
			if (curState == PlayerState.run || curState == PlayerState.sprint)
			{
				setState(PlayerState.roll);
			}
		}
		else if (!Input.anyKey)
		{
			if ((curState == PlayerState.walk || curState == PlayerState.run || curState == PlayerState.sprint) && curState != PlayerState.idle)
			{
				setState(PlayerState.idle);
			}
		}
	}

	void checkWallAngle()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
		if (Physics.Raycast(ray, out hit))
		{
			float angleToWall = Mathf.Rad2Deg * Mathf.Acos(Vector2.Dot(new Vector2(ray.direction.x, ray.direction.z), new Vector2(hit.normal.x, hit.normal.z)));
			Debug.Log(angleToWall);
			Debug.DrawLine(ray.origin, hit.point, Color.cyan, 10);
			if (angleToWall > 180 - angleToWallRun)
			{
				Debug.Log("climb");
				setState(PlayerState.climb);
			}
			else// if (curState == PlayerState.run || curState == PlayerState.sprint || curState == PlayerState.wallJump)
			{
				Debug.Log("wall Run");
				setState(PlayerState.wallRun);
			}
		}	
	}

	bool checkGround()
	{
		RaycastHit hit;
		Ray ray = new Ray(transform.position, -transform.up);
		if (Physics.Raycast(ray, out hit))
		{
			Debug.DrawLine(ray.origin, hit.point, Color.blue, 10);
			Debug.Log("box height "+(this.GetComponent<BoxCollider>().size.y / 2));
			if (Vector3.Distance(ray.origin, hit.point) <= this.GetComponent<BoxCollider>().size.y/2)
			{
				return true;
			}
		}
		return false;
	}

	bool checkWall()
	{
		RaycastHit hit;
		Ray ray = new Ray(transform.position, transform.right);
		if (Physics.Raycast(ray, out hit))
		{
			Debug.DrawLine(ray.origin, ray.GetPoint(maxDistanceToWall), Color.black, 10);
			if (Vector3.Distance(ray.origin, hit.point) <= maxDistanceToWall)
			{
				return true;
			}
		}
		ray = new Ray(transform.position, transform.forward);
		if (Physics.Raycast(ray, out hit))
		{
			Debug.DrawLine(ray.origin, ray.GetPoint(maxDistanceToWall), Color.black, 10);
			if (Vector3.Distance(ray.origin, hit.point) <= maxDistanceToWall)
			{
				return true;
			}
		}
		ray = new Ray(transform.position, -transform.forward);
		if (Physics.Raycast(ray, out hit))
		{
			Debug.DrawLine(ray.origin, ray.GetPoint(maxDistanceToWall), Color.black, 10);
			if (Vector3.Distance(ray.origin, hit.point) <= maxDistanceToWall)
			{
				return true;
			}
		}
		return false;
	}

	void setState(PlayerState tempState)
	{
		switch (tempState)
		{
			case PlayerState.idle:
				idle();
				break;
			case PlayerState.walk:
				walk();
				break;
			case PlayerState.run:
				run();
				break;
			case PlayerState.sprint:
				sprint();
				break;
			case PlayerState.jump:
				jump();
				break;
			//case PlayerState.doubleJump:
			//	doubleJump();
			//	break;
			case PlayerState.falling:
				falling();
				break;
			case PlayerState.roll:
				if (curState == PlayerState.run)
				{
					runToRoll();
				}
				else if(curState == PlayerState.sprint)
				{
					sprintToRoll();
				}
				break;
			case PlayerState.lunge:
				lunge();
				break;
			case PlayerState.climb:
				climb();
				break;
			case PlayerState.backEject:
				backEject();
				break;
			case PlayerState.wallRun:
				if (curState == PlayerState.run)
				{
					runToWall();
				}
				else if (curState == PlayerState.sprint)
				{
					sprintToRoll();
				}
				else if (curState == PlayerState.wallJump || curState == PlayerState.jump || curState == PlayerState.doubleJump)
				{
					wallToWall();
				}
				break;
			case PlayerState.wallJump:
				wallJump();
				break;
			default:
				break;
		}
	}

	//not doing anything 
	void idle()
	{
		lockCamera = false;
		playerAni.SetTrigger("startIdle");
		curState = PlayerState.idle;
		playerAudio.setAudio(PlayerState.idle);
	}

	//one off jumps that add instant force
	void jump()
	{
		Debug.Log("jumping");
		lockCamera = false;
		playerAni.SetTrigger("startJump");
		curState = PlayerState.jump;
		//playerRidg.velocity = transform.up * jumpHeight;
		playerRidg.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
		//curSpeed = fallingSpeed;
		playerAudio.setAudio(PlayerState.jump);
	}

	void doubleJump()
	{
		lockCamera = false;
		playerAni.SetTrigger("startDoubleJump");
		curState = PlayerState.doubleJump;
		playerRidg.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
		canDoubleJump = false;
		//curSpeed = fallingSpeed;
		playerAudio.setAudio(PlayerState.jump);
	}

	void backEject()
	{
		Debug.Log("back eject");
		lockCamera = false;
		canDoubleJump = true;
		playerAni.SetTrigger("startBackEject");
		playerRidg.AddForce(-transform.right * backEjectHeight, ForceMode.Impulse);
		canDoubleJump = true;
		playerAudio.setAudio(PlayerState.jump);
	}

	void wallJump()
	{
		lockCamera = false;
		playerAni.SetTrigger("startWallJump");
		curState = PlayerState.wallJump;
		playerRidg.AddForce(transform.up * jumpHeight * 2, ForceMode.Impulse);
		canDoubleJump = false;
		canWallRun = false;
		wallRunCooldownTime = 0;
		playerAudio.setAudio(PlayerState.jump);
	}

	//setting the players current speed
	void run()
	{
		lockCamera = false;
		lockMovement = false;
		playerAni.SetTrigger("startRun");
		curState = PlayerState.run;
		curSpeed = runSpeed;
		playerAudio.setAudio(PlayerState.run);
	}

	void walk()
	{
		lockCamera = false;
		lockMovement = false;
		playerAni.SetTrigger("startWalk");
		curState = PlayerState.walk;
		curSpeed = walkSpeed;
		playerAudio.setAudio(PlayerState.walk);
	}

	void sprint()
	{
		lockCamera = false;
		lockMovement = false;
		playerAni.SetTrigger("startSprint");
		curState = PlayerState.sprint;
		curSpeed = sprintSpeed;
		playerAudio.setAudio(PlayerState.sprint);
	}

	//starts of the state transition
	public void goLunge(Vector3 pos, float range)
	{
		playerTran.position = Vector3.Lerp(playerTran.position, pos, lungeSpeed * Time.deltaTime);
		lungePos = pos;
		meleeRange = range;
		setState(PlayerState.lunge);
	}

	void lunge()
	{
		lockCamera = true;
		lockMovement = true;
		playerAni.SetTrigger("startLunge");
		curState = PlayerState.lunge;
	}

	void falling()
	{
		lockCamera = false;
		playerAni.SetTrigger("startFalling");
		curState = PlayerState.falling;
	}

	void runToRoll()
	{
		playerAni.SetTrigger("startRoll");
		curState = PlayerState.roll;
		lockCamera = true;
		curSlideSpeed = runRollSpeed;
	}

	void sprintToRoll()
	{
		playerAni.SetTrigger("startRoll");
		curState = PlayerState.roll;
		lockCamera = true;
		curSlideSpeed = sprintRollSpeed;
	}

	void runToWall()
	{
		playerAni.SetTrigger("startWallRun");
		curState = PlayerState.wallRun;
		//canJump = false;
		lockCamera = true;
		curWallSpeed = runWallSpeed;
		curWallRunHeight = wallRunHeight;
		curWallRunLength = wallRunLength;
		//Debug.Log(curWallSpeed + " " + curWallRunHeight + " " + curWallRunLength);
	}

	void sprintToWall()
	{
		playerAni.SetTrigger("startWallRun");
		curState = PlayerState.wallRun;
		lockCamera = true;
		curWallSpeed = sprintWallSpeed;
		curWallRunHeight = wallRunHeight;
		curWallRunLength = wallRunLength;
		//Debug.Log(curWallSpeed + " " + curWallRunHeight + " " + curWallRunLength);
	}

	void wallToWall()
	{
		playerAni.SetTrigger("startWallRun");
		curState = PlayerState.wallRun;
		lockCamera = true;
		curWallSpeed = runWallSpeed;
		curWallRunHeight = wallRunHeight;
		curWallRunLength = wallRunLength;
		Debug.Log(curWallSpeed + " " + curWallRunHeight + " " + curWallRunLength);
	}

	void climb()
	{
		curState = PlayerState.climb;
		playerAni.SetTrigger("startClimb");
		lockCamera = true;
		curClimbSpeed = climbSpeed;
	}

	//commands called in the update
	void climbing()
	{
		playerTran.position = Vector3.Lerp(playerTran.position, playerTran.position + transform.up, curClimbSpeed * Time.deltaTime);
		curClimbSpeed -= climbDep;
		if (curClimbSpeed <= 0)
		{
			setState(PlayerState.falling);
			playerAni.SetTrigger("startIdle");
		}
	}

	void Rolling()
	{
		playerTran.position = Vector3.Lerp(playerTran.position, playerTran.position + targetVelocity, curSlideSpeed * Time.deltaTime);
		curSlideSpeed -= slideDep;	//delat time test
		if (curSlideSpeed <= 0)
		{
			setState(PlayerState.run);
			playerAni.SetTrigger("startIdle");
		}
	}

	void lunging()
	{
		playerTran.position = Vector3.Lerp(playerTran.position, lungePos, lungeSpeed * Time.deltaTime);
		if (Vector3.Distance(playerTran.position, lungePos) < meleeRange)
		{
			setState(PlayerState.run);
			playerAni.SetTrigger("startIdle");
			lockCamera = false;
			lockMovement = false;
		}
	}

	void wallRunning()
	{
		Vector3 nextWallSpeed = new Vector3(curWallRunLength, curWallRunHeight, 0).normalized;
		nextWallSpeed = playerTran.TransformDirection(nextWallSpeed);
		playerTran.position = Vector3.Lerp(playerTran.position, playerTran.position + nextWallSpeed, curWallSpeed * Time.deltaTime);
		curWallRunHeight -= wallHeightDep * Time.deltaTime;

		if (!checkWall())
		{
			setState(PlayerState.wallJump);
		}
		else if(checkGround())
		{
			setState(PlayerState.falling);
		}

		//Debug.Log("wall heigher " + curWallRunHeight + nextWallSpeed);
		//curWallRunLength -= wallLengthDep * Time.deltaTime;
	}

	void moving()
	{
		targetVelocity *= curSpeed;
		var v = playerRidg.velocity;
		var velocityChange = (targetVelocity - v);
		velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
		velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
		velocityChange.y = 0;
		//Debug.Log("vel change " + velocityChange);

		//ridgedbody movement
		playerRidg.AddForce(velocityChange, ForceMode.VelocityChange);

		//translation movement
		//just changed this to the proper version of applying time to the movement 

		Debug.Log("moving vel");
		//playerRidg.velocity = targetVelocity.normalized * curSpeed;// * Time.deltaTime;
		//playerTran.position = playerTran.position + targetVelocity * curSpeed * Time.deltaTime;
	}

	/*
	//collisions with wall (will be replaced with raycasts)
	void OnCollisionEnter(Collision col)
	{
		/*
		if (col.gameObject.tag == "Ground" && curState != PlayerState.wallRun)
		{
			Debug.Log("enter ground");
			setState(PlayerState.idle);
			canJump = true;
			canDoubleJump = false;
		}
		else if (col.gameObject.tag == "Ground" && curState == PlayerState.wallRun)
		{
			Debug.Log("exit wall run");
			setState(PlayerState.idle);
			canJump = true;
			canDoubleJump = false;
			lockCamera = false;
		}
		if (col.gameObject.tag == "Wall" && (curState == PlayerState.wallJump || curState == PlayerState.jump || curState == PlayerState.doubleJump))
		{
			Debug.Log("wall to wall start");
			checkWallAngle();
		}
		else if (col.gameObject.tag == "Wall")
		{
			Debug.Log("can jump");
			canClimb = true;
		}
		

		//note: this is for the curved wall running idea by taking the normal of the wall and the plyer will follow that 
		/*Vector3 p = col.contacts[0].point;
		RaycastHit info = new RaycastHit();
		if(Physics.Raycast(transform.position, p - transform.position, out info))
			Debug.DrawLine(info.point, info.point + info.normal, Color.cyan, 10);
	}

	void OnCollisionExit(Collision col)
	{
		/*
		if (col.gameObject.tag == "Ground")
		{
			//Debug.Log("exit ground");
			//setState(PlayerState.falling);
			canJump = false;
			canDoubleJump = true;
		}
		if (col.gameObject.tag == "Wall" && curState == PlayerState.wallRun)
		{
			Debug.Log("falling");
			setState(PlayerState.falling);
			canClimb = false;
		}
		else if (col.gameObject.tag == "Wall")
		{
			Debug.Log("cant jump");
			canClimb = false;
		}
		
	}
	*/
}