using UnityEngine;
using System.Collections;

public enum PlayerState
{
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
	//will add others when needed
}

public class PlayerController : MonoBehaviour {

	public PlayerState curState;

	private Transform playerTran;
	private Rigidbody playerRidg;
	private Camera playerCam;
	private bool canJump;
	private bool canDoubleJump;
	private float curSpeed;

	public float walkSpeed;
	public float runSpeed;
	public float sprintSpeed;
	public float fallingSpeed;
	public float maxVelocityChange;
	public float jumpHeight;

	// Use this for initialization
	void Start ()
	{
		curState = PlayerState.run;
		playerTran = this.transform;
		playerRidg = this.GetComponent<Rigidbody>();
		playerCam = this.transform.FindChild("camera").gameObject.GetComponent<Camera>();
		curSpeed = runSpeed;
		canJump = true;
		canDoubleJump = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		checkInput();
		move();
		
	}

	void checkInput()
	{
		if (Input.GetButtonUp("Jump"))
		{
			if (canJump)
			{
				setState(PlayerState.jump);
			}
			else if (canDoubleJump)
			{
				setState(PlayerState.doubleJump);
			}
		}
		if (Input.GetButtonUp("Walk"))
		{
			if (curState == PlayerState.run || curState == PlayerState.sprint)
			{
				setState(PlayerState.walk);
			}
		}
		if (Input.GetButtonUp("Sprint"))
		{
			if (curState == PlayerState.walk || curState == PlayerState.run)
			{
				setState(PlayerState.sprint);
			}
		}
	}

	void setState(PlayerState tempState)
	{
		switch (tempState)
		{
			case PlayerState.walk:
				walk();
				this.GetComponent<MeshRenderer>().material.color = Color.green;
				break;
			case PlayerState.run:
				run();
				this.GetComponent<MeshRenderer>().material.color = Color.yellow;
				break;
			case PlayerState.sprint:
				sprint();
				this.GetComponent<MeshRenderer>().material.color = Color.red;
				break;
			case PlayerState.jump:
				jump();
				this.GetComponent<MeshRenderer>().material.color = Color.cyan;
				break;
			case PlayerState.doubleJump:
				doubleJump();
				this.GetComponent<MeshRenderer>().material.color = Color.blue;
				break;
			case PlayerState.falling:
				this.GetComponent<MeshRenderer>().material.color = Color.magenta;
				break;
			case PlayerState.slide:
				this.GetComponent<MeshRenderer>().material.color = Color.black;
				break;
			default:
				break;
		}
	}

	void jump()
	{
		curState = PlayerState.jump;
		playerRidg.AddForce(transform.up * jumpHeight);
		canJump = false;
		//curSpeed = fallingSpeed;
	}

	void doubleJump()
	{
		playerRidg.AddForce(transform.up * jumpHeight);
		canDoubleJump = false;
		//curSpeed = fallingSpeed;
	}

	void run()
	{
		curState = PlayerState.run;
		curSpeed = runSpeed;
	}

	void walk()
	{
		curState = PlayerState.walk;
		curSpeed = walkSpeed;
	}

	void sprint()
	{
		curState = PlayerState.sprint;
		curSpeed = sprintSpeed;
	}

	void falling()
	{
		curState = PlayerState.falling;
	}

	void move()
	{
		//ridgedbody movement
		Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		targetVelocity = playerCam.transform.TransformDirection(targetVelocity);
		targetVelocity *= curSpeed;
		var v = playerRidg.velocity;
		var velocityChange = (targetVelocity - v);
		velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
		velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
		velocityChange.y = 0;

		playerRidg.AddForce(velocityChange, ForceMode.VelocityChange);
	}

	void OnTriggerEnter(Collider col)
	{
		Debug.Log("enter col");
		if (col.gameObject.tag == "Ground")
		{
			Debug.Log("enter ground");
			setState(PlayerState.run);
			canJump = true;
			canDoubleJump = false;
		}
	}

	void OnTriggerExit(Collider col)
	{
		Debug.Log("exit col");
		if (col.gameObject.tag == "Ground")
		{
			Debug.Log("exit ground");
			setState(PlayerState.falling);
			canJump = false;
			canDoubleJump = true;
		}
	}
}
