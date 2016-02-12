using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public enum PlayerState
{
	//white
	idle, 
	//green
	walk,
	//yellow
	run, 
	//red
	sprint,
	//cyan 
	jump, 
	//blue
	doubleJump,
	//madgenta
	falling, 
	//black
	slide, 
	//gray
	lunge,
	climb,
	backEject,
	//will add others when needed
}

public class PlayerController : NetworkBehaviour {

	public PlayerState curState;

	private Transform playerTran;
	private Rigidbody playerRidg;
	private bool canJump;
	private bool canDoubleJump;
	private float curSpeed;
	private Vector3 targetVelocity;
	private float curSlideSpeed;
	private bool lockCamera;
	private bool lockMovement;
	private Vector3 lungePos;
	private float meleeRange;
	private bool canClimb;
	private float curClimbSpeed;

	public float walkSpeed = 5;
	public float runSpeed = 10;
	public float sprintSpeed = 25;
	public float fallingSpeed = 0;
	public float lungeSpeed = 25;
	public float maxVelocityChange = 10;
	public float jumpHeight = 500;
	public float runSlideSpeed = 30;
	public float sprintSlideSpeed = 50;
	public float slideDep = 1;
	public float angleToWallRun = 10;
	public float climbSpeed = 20;
	public float climbDep = 1;
	public float backEjectHeight = 300;

	// Use this for initialization
	void Start ()
	{
		curState = PlayerState.run;
		this.GetComponent<MeshRenderer>().material.color = Color.yellow;
		playerTran = this.transform;
		playerRidg = this.GetComponent<Rigidbody>();

		this.transform.FindChild("camera").gameObject.GetComponent<Camera>().enabled = isLocalPlayer;
		this.transform.FindChild("camera").gameObject.GetComponent<AudioListener>().enabled = isLocalPlayer;
		curSpeed = runSpeed;
		canJump = true;
		canDoubleJump = false;
		lockCamera = false;
		lockMovement = false;

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

		if (!lockMovement)
		{
			checkInput();
			if (curState == PlayerState.slide)
			{
				sliding();
			}
			else if (curState == PlayerState.climb)
			{
				climbing();
			}
			else
			{
				moving();
			}
		}
		else
		{
			lunging();
		}
	}

	void checkInput()
	{
		if (Input.GetButtonUp("Jump"))
		{
			if (canClimb)
			{
				if (!canJump)
				{
					Debug.Log("set backEject");
					setState(PlayerState.backEject);
				}
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
				if (Physics.Raycast(ray, out hit))
				{
					float angleToWall = Mathf.Rad2Deg * Mathf.Acos(Vector2.Dot(new Vector2(ray.direction.x, ray.direction.z), new Vector2(hit.normal.x, hit.normal.z)));
					Debug.DrawLine(ray.origin, hit.point, Color.cyan, 10);
					Debug.Log("hit angle " + angleToWall);
					if (angleToWall > 180 - angleToWallRun)
					{
						Debug.Log("climb");
						setState(PlayerState.climb);
					}
				}
			}
			else if (canJump)
			{
				setState(PlayerState.jump);
			}
			else if (canDoubleJump)
			{
				setState(PlayerState.doubleJump);
			}
		}
		else if (Input.GetButtonUp("Walk"))
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
		else if (Input.GetButtonUp("Sprint"))
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
				setState(PlayerState.slide);
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
			case PlayerState.doubleJump:
				doubleJump();
				break;
			case PlayerState.falling:
				break;
			case PlayerState.slide:
				if (curState == PlayerState.run)
				{
					runToSlide();
				}
				else if(curState == PlayerState.sprint)
				{
					sprintToSlide();
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
			default:
				break;
		}
	}

	void idle()
	{
		lockCamera = false;
		this.GetComponent<MeshRenderer>().material.color = Color.white;
		curState = PlayerState.idle;
	}

	void jump()
	{
		lockCamera = false;
		this.GetComponent<MeshRenderer>().material.color = Color.cyan;
		curState = PlayerState.jump;
		playerRidg.AddForce(transform.up * jumpHeight);
		canJump = false;
		//curSpeed = fallingSpeed;
	}

	void doubleJump()
	{
		lockCamera = false;
		this.GetComponent<MeshRenderer>().material.color = Color.blue;
		playerRidg.AddForce(transform.up * jumpHeight);
		canDoubleJump = false;
		//curSpeed = fallingSpeed;
	}

	void backEject()
	{
		Debug.Log("back eject");
		lockCamera = false;
		canDoubleJump = true;
		this.GetComponent<MeshRenderer>().material.color = Color.blue;
		playerRidg.AddForce(-transform.right * backEjectHeight);
		canDoubleJump = true;
	}

	void run()
	{
		lockCamera = false;
		this.GetComponent<MeshRenderer>().material.color = Color.yellow;
		curState = PlayerState.run;
		curSpeed = runSpeed;
	}

	void walk()
	{
		lockCamera = false;
		this.GetComponent<MeshRenderer>().material.color = Color.green;
		curState = PlayerState.walk;
		curSpeed = walkSpeed;
	}

	void sprint()
	{
		lockCamera = false;
		this.GetComponent<MeshRenderer>().material.color = Color.red;
		curState = PlayerState.sprint;
		curSpeed = sprintSpeed;
	}

	void lunge()
	{
		lockCamera = true;
		lockMovement = true;
		this.GetComponent<MeshRenderer>().material.color = Color.gray;
		curState = PlayerState.lunge;
	}

	void falling()
	{
		lockCamera = false;
		this.GetComponent<MeshRenderer>().material.color = Color.magenta;
		curState = PlayerState.falling;
	}

	void runToSlide()
	{
		this.GetComponent<MeshRenderer>().material.color = Color.black;
		curState = PlayerState.slide;
		lockCamera = true;
		curSlideSpeed = runSlideSpeed;
	}

	void sprintToSlide()
	{
		this.GetComponent<MeshRenderer>().material.color = Color.black;
		curState = PlayerState.slide;
		lockCamera = true;
		curSlideSpeed = sprintSlideSpeed;
	}

	void climb()
	{
		curState = PlayerState.climb;
		lockCamera = true;
		curClimbSpeed = climbSpeed;
	}

	void climbing()
	{
		playerTran.position = Vector3.Lerp(playerTran.position, playerTran.position + transform.up, curClimbSpeed * Time.deltaTime);
		curClimbSpeed -= climbDep;
		if (curClimbSpeed <= 0)
		{
			setState(PlayerState.falling);
		}
	}

	void sliding()
	{
		playerTran.position = Vector3.Lerp(playerTran.position, playerTran.position + targetVelocity, curSlideSpeed * Time.deltaTime);
		curSlideSpeed -= slideDep;
		if (curSlideSpeed <= 0)
		{
			setState(PlayerState.run);
		}
	}

	void lunging()
	{
		playerTran.position = Vector3.Lerp(playerTran.position, lungePos, lungeSpeed * Time.deltaTime);
		if (Vector3.Distance(playerTran.position, lungePos) < meleeRange)
		{
			setState(PlayerState.run);
			lockCamera = false;
			lockMovement = false;
		}
	}

	void moving()
	{	
		//uncomment for ridgid body movement
		//targetVelocity *= curSpeed;
		//var v = playerRidg.velocity;
		//var velocityChange = (targetVelocity - v);
		//velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
		//velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
		//velocityChange.y = 0;

		//ridgedbody movement
		//playerRidg.AddForce(velocityChange, ForceMode.VelocityChange);

		//translation movement
		playerTran.position = Vector3.Lerp(playerTran.position, playerTran.position + targetVelocity, curSpeed*Time.deltaTime);
	}

	void OnTriggerEnter(Collider col)
	{
		//Debug.Log("enter col");
		if (col.gameObject.tag == "Ground")
		{
			//Debug.Log("enter ground");
			setState(PlayerState.run);
			canJump = true;
			canDoubleJump = false;
		}
		if(col.gameObject.tag == "Wall")
		{
			Debug.Log("can jump");
			canClimb = true;
		}
	}

	void OnTriggerExit(Collider col)
	{
		//Debug.Log("exit col");
		if (col.gameObject.tag == "Ground")
		{
			//Debug.Log("exit ground");
			setState(PlayerState.falling);
			canJump = false;
			canDoubleJump = true;
		}
		if (col.gameObject.tag == "Wall")
		{
			Debug.Log("cant jump");
			canClimb = false;
		}
	}

	public void goLunge(Vector3 pos, float range)
	{
		playerTran.position = Vector3.Lerp(playerTran.position, pos, lungeSpeed * Time.deltaTime);
		lungePos = pos;
		meleeRange = range;
		setState(PlayerState.lunge);
	}
}
