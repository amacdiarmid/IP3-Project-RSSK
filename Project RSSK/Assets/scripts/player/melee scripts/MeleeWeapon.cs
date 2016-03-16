using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class MeleeWeapon : NetworkBehaviour
{
	public bool primWeap;
	public int damage = 10;
	public float cooldown = 5;
	public float comboCoolDown = 1;
	public float range = 10;
	public float knockback = 5;
	public Animator swordAni;   //todo set up a search to find the correct animator

	private bool canAttack = true;
	private float countdownTimer;
	private int comboPos = 0;

	public List<AudioClip> attackAudio;
	public AudioSource audioSource;

	// Update is called once per frame
	void Update()
	{
		/*
		if (!canAttack)
		{
			countdownTimer += Time.deltaTime;
			if (countdownTimer >= cooldown)
			{
				Debug.Log("combo reset");
				canAttack = true;
				curComboPos = 0;
			}
		}
		*/

		if (canAttack && ((primWeap && Input.GetButtonDown("Fire1")) || (!primWeap && Input.GetButtonDown("Fire2"))) && comboPos < 3)
			attack();
		/*else
			Debug.Log("can attack " + canAttack + " combo pos " + comboPos);*/
	}

	public void attack()
	{
		//Debug.Log("attack called");
		//canAttack = false;
		StopCoroutine(comboWait());
		//countdownTimer = 0;
		comboPos++;

		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));

		audioSource.PlayOneShot(attackAudio[Random.Range(0, attackAudio.Count - 1)]);


		if (comboPos == 1)
			swordAni.SetTrigger("attack1");
		else if (comboPos == 2)
			swordAni.SetTrigger("attack2");
		else if (comboPos == 3)
			swordAni.SetTrigger("attack3");
		StartCoroutine(comboWait());

		if (Physics.Raycast(ray, out hit, range))
		{
			Debug.Log("hit melee wep " + hit.collider.gameObject.name);
			Debug.DrawLine(ray.origin, hit.point, Color.yellow, 10);
			if (hit.collider.gameObject.tag == "TestPlayer")
				hit.collider.gameObject.GetComponent<TestPlayer>().hit();
			else if (hit.collider.gameObject.tag == "Player") //need to test with other people 
				hit.collider.gameObject.GetComponent<PlayerStats>().Damage(damage);
		}
		else
		{
			Debug.DrawLine(ray.origin, ray.GetPoint(range), Color.magenta, 10);
			//canAttack = false;
		}
	}

	IEnumerator comboWait()
	{
		//Debug.Log("swing time " + Time.time);
		yield return new WaitForSeconds(comboCoolDown);
		//Debug.Log("combocool down time " + Time.time);
		comboPos = 0;
		canAttack = false;
		yield return new WaitForSeconds(cooldown);
		//Debug.Log("attackcool down time " + Time.time);
		canAttack = true;
	}

	public void attackAni()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));

		if (Physics.Raycast(ray, out hit, range))
		{
			Debug.Log("hit melee wep " + hit.collider.gameObject.name);
			if (hit.collider.gameObject.tag == "TestPlayer" || hit.collider.gameObject.tag == "Player")
			{
				//hit.collider.GetComponent<Rigidbody>().AddForce(transform.forward * (knockback * 2), ForceMode.Impulse);
				//this.GetComponent<Rigidbody>().AddForce(-transform.forward * knockback, ForceMode.Impulse);
			}
		}
	}

	public void endCombo()
	{
		canAttack = false;
	}
}
