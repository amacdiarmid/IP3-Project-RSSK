using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAudioController : MonoBehaviour {

	public List<AudioClip> jumpAudio;
	public List<AudioClip> movingAudio;
	public float walkSpeed;
	public float runSpeed;
	public float SpringSpeed;
	public AudioSource feetAudio;

	public List<AudioClip> damageAudio;
	public List<AudioClip> deathAudio;
	public AudioClip spawnAudio;
	public AudioSource mouthAudio;

	private bool moving = false;
	private bool nextStep = true;
	private float footSpeed = 0;
	private float footSpeedTime = 0;

	// Update is called once per frame
	void Update ()
	{
		if (moving && nextStep)
		{
			feetAudio.PlayOneShot(movingAudio[Random.Range(0, movingAudio.Count - 1)]);
			footSpeedTime = 0;
			nextStep = false;
		}
		else if (!nextStep)
		{
			nextStep = (footSpeedTime += Time.deltaTime) >= footSpeed;
		}
	}	

	public void setAudio(PlayerState state)
	{
		switch (state)
		{
			case PlayerState.idle:
				moving = false;
				break;
			case PlayerState.walk:
				moving = true;
				footSpeed = walkSpeed;
				break;
			case PlayerState.run:
				moving = true;
				footSpeed = runSpeed;
				break;
			case PlayerState.sprint:
				moving = true;
				footSpeed = SpringSpeed;
				break;
			case PlayerState.jump:
				moving = false;
				feetAudio.PlayOneShot(jumpAudio[Random.Range(0, jumpAudio.Count - 1)]);
				break;
			default:
				Debug.Log("wrong state switch in audio");
				break;
		}
	}

	public void spawn()
	{
		mouthAudio.PlayOneShot(spawnAudio, 1);
		Debug.Log("spawn audio");
	}

	public void damaged()
	{
		feetAudio.PlayOneShot(damageAudio[Random.Range(0, damageAudio.Count - 1)]);
	}

	public void dead()
	{
		feetAudio.PlayOneShot(deathAudio[Random.Range(0, deathAudio.Count - 1)]);
	}
}
