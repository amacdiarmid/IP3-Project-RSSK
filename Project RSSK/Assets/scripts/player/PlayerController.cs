using UnityEngine;
using System.Collections;

public enum PlayerState
{
	walk,
	run, 
	sprint, 
	jump, 
	doubleJump,
	falling, 
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
			//else if (canDoubleJump)
			//{
			//	switchState(PlayerState.jump);
			//}
		}
	}

	void setState(PlayerState tempState)
	{
		switch (tempState)
		{
			case PlayerState.walk:
				break;
			case PlayerState.run:
				run();
				break;
			case PlayerState.sprint:
				break;
			case PlayerState.jump:
				jump();
				break;
			case PlayerState.doubleJump:
				jump();
				break;
			case PlayerState.falling:
				break;
			case PlayerState.slide:
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

	void run()
	{
		curState = PlayerState.run;
		curSpeed = runSpeed;
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
