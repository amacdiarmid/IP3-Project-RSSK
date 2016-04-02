﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Collections;

//dont use this for that characters use one of the derived classes
public class Gun : NetworkBehaviour
{

	protected int curAmmo;
	protected float RoFTime = 0;
	protected bool canFire = true;
	protected bool reloading = false;
	protected float gunSreadVal = 0;
	public Transform barrel;
	public GameObject bulletTrail;

	public bool primWeap;
	public float range = 100;
	public int maxAmmo = 30;
	public int spareAmmo = 90;
	public float rateOfFire = 1;
	public int damage = 50;
	public float reloadSpeed = 5;

	public float maxSpread = 3;
	public float spreadAdv = 0.2f;
	public float spreadDep = 0.1f;
	public float aimMuli = 0.5f;
	public float sprintMuli = 2;


	//public GameObject projectile;

	NetworkAnimator gunAni;   //todo set up a search to find the correct animator

	public AudioClip fireAudio;
	public AudioClip outOfAmmoAudio;
	public AudioClip reloadAudio;
	public AudioSource audioSource;

	void Start()
	{
		curAmmo = maxAmmo;
		gunAni = GetComponent<NetworkAnimator>();
		if (gunAni == null)
			Debug.LogError ("Setup: Failed to find NetworkAnimator");
	}

	void Update()
	{
		checkInput();
		checkAim();
	}

	public virtual void checkInput()
	{
		Debug.LogError("Don't use the base class!", gameObject);
	}

	public virtual void Shoot()
	{
		if (curAmmo >= 0 && !reloading)
		{
			RoFTime = 0;
			canFire = false;

			--curAmmo;

			float targetX = Screen.width / 2 + Random.Range(-gunSreadVal, gunSreadVal);
			float targetY = Screen.height / 2 + Random.Range(-gunSreadVal, gunSreadVal);

			RaycastHit[] hits;
			Ray ray = Camera.main.ScreenPointToRay(new Vector2(targetX, targetY));
			hits = Physics.RaycastAll(ray);
			foreach (var hit in hits)
			{
				if (hit.transform != this.transform)
				{
					//gun to target ray
					Debug.DrawLine(barrel.transform.position, hit.point, Color.blue, 10);
					GameObject trail = Instantiate(bulletTrail);
					trail.GetComponent<GunProjectile>().setUpLine(barrel.transform.position, hit.point);
					//screen to target ray
					Debug.DrawLine(ray.origin, hit.point, Color.red, 10);
					if (hit.collider.tag == "Player")
						CmdHit(hit.transform.gameObject, damage);
					break;
				}
			}
			if (hits.Length == 0)
			{
				Debug.Log("no hit");
				//gun to target ray
				Debug.DrawLine(barrel.transform.position, ray.GetPoint(range), Color.blue, 10);
				GameObject trail = Instantiate(bulletTrail);
				trail.GetComponent<GunProjectile>().setUpLine(barrel.transform.position, ray.GetPoint(range));
				//screen to target ray
				Debug.DrawLine(ray.origin, ray.GetPoint(range), Color.red, 10);
			}

			audioSource.PlayOneShot(fireAudio);
			gunAni.SetTrigger("fire");

			//add spread to the next shot 

			float curMaxSpread = maxSpread;
			if (Input.GetButton("Aim"))
				curMaxSpread = curMaxSpread * aimMuli;
			if (Input.GetButton("Sprint"))
				curMaxSpread = curMaxSpread * sprintMuli;


			gunSreadVal = Mathf.Clamp(gunSreadVal + spreadAdv, 0, curMaxSpread);
		}
		else
		{
			if(!reloading)
			audioSource.PlayOneShot(outOfAmmoAudio);
		}
	}

	public void reload()
	{
		StartCoroutine(reloadWait());
		//this could be alot better
		if (curAmmo != maxAmmo && spareAmmo > 0)
		{
			gunAni.SetTrigger("reload");
		}
		while (curAmmo != maxAmmo && spareAmmo > 0)
		{
			++curAmmo;
			--spareAmmo;
			audioSource.PlayOneShot(reloadAudio);
		}
		gunSreadVal = 0;
	}

	IEnumerator reloadWait()
	{
		reloading = true;
		yield return new WaitForSeconds(reloadSpeed);
		reloading = false;
	}

	void checkAim()
	{
		if (Input.GetButtonDown("Aim"))
		{
			gunAni.animator.SetFloat("aim", 1);
		}
		else if (Input.GetButtonUp("Aim"))
		{
			gunAni.animator.SetFloat("aim", 0);
		}
	}

	[Command]
	void CmdHit(GameObject obj, int damage)
	{
		obj.GetComponent<PlayerStats> ().Damage (damage);
	}
}
