using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class MeleeWeapon : NetworkBehaviour
{
	public float lungeRange = 5;
	public int damage = 10;
	public float cooldown = 5;
	public float range = 10;
	public float knockback = 5;

	private bool canAttack;
	private float countdownTimer;
	private PlayerController playerCon;
	public Animator swordAni;   //todo set up a search to find the correct animator

	public List<AudioClip> attackAudio;
	public AudioSource audioSource;

	void Start()
	{
		canAttack = true;
		playerCon = GetComponent<PlayerController>();
	}

	// Update is called once per frame
	void Update()
	{
		if (!canAttack)
		{
			countdownTimer += Time.deltaTime;
			if (countdownTimer >= cooldown)
				canAttack = true;
		}

		if (Input.GetButton("Fire2"))
			attack();
	}

	public void attack()
	{
		if (canAttack)
		{
			canAttack = false;
			countdownTimer = 0;

			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
			if (Physics.Raycast(ray, out hit))
			{
				Debug.Log("hit melee wep " + hit.collider.gameObject.name);
				if (hit.collider.gameObject.tag == "TestPlayer")
				{
					if (Vector3.Distance(transform.position, hit.point) <= range)
					{
						Debug.DrawLine(ray.origin, hit.point, Color.yellow, 10);
						hit.collider.gameObject.GetComponent<TestPlayer>().hit();
					}
					else if (Vector3.Distance(transform.position, hit.point) <= lungeRange)
					{
						Debug.DrawLine(ray.origin, hit.point, Color.black, 10);
						playerCon.goLunge(hit.point, range);
						hit.collider.gameObject.GetComponent<TestPlayer>().hit();
					}
					else
					{
						Debug.DrawLine(ray.origin, ray.GetPoint(lungeRange), Color.magenta, 10);
					}
				}
				//need to test with other people 
				else if (hit.collider.gameObject.tag == "Player")
				{
					if (Vector3.Distance(transform.position, hit.point) <= range)
					{
						Debug.DrawLine(ray.origin, hit.point, Color.yellow, 10);
						hit.collider.gameObject.GetComponent<PlayerStats>().CmdDamage(damage);
					}
					else if (Vector3.Distance(transform.position, hit.point) <= lungeRange)
					{
						Debug.DrawLine(ray.origin, hit.point, Color.black, 10);
						playerCon.goLunge(hit.point, range);
						hit.collider.gameObject.GetComponent<PlayerStats>().CmdDamage(damage);
					}
					else
					{
						Debug.DrawLine(ray.origin, ray.GetPoint(lungeRange), Color.magenta, 10);
					}
				}
			}
			else
			{
				Debug.DrawLine(ray.origin, ray.GetPoint(lungeRange), Color.magenta, 10);
			}
			audioSource.PlayOneShot(attackAudio[Random.Range(0, attackAudio.Count - 1)]);
			swordAni.SetTrigger("attack");
		}
	}

	public void attackAni()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
		if (Physics.Raycast(ray, out hit, 10))
		{
			Debug.Log("hit melee wep " + hit.collider.gameObject.name);
			if (hit.collider.gameObject.tag == "TestPlayer")
			{
				hit.collider.GetComponent<Rigidbody>().AddForce(this.transform.right * knockback, ForceMode.Impulse);
				this.GetComponent<Rigidbody>().AddForce(-this.transform.right * (knockback * 2), ForceMode.Impulse);
			}
			//need to test with other people 
			else if (hit.collider.gameObject.tag == "Player")
			{
				hit.collider.GetComponent<Rigidbody>().AddForce(this.transform.right * knockback, ForceMode.Impulse);
				this.GetComponent<Rigidbody>().AddForce(-this.transform.right * (knockback * 2), ForceMode.Impulse);
			}
		}
	}
}
