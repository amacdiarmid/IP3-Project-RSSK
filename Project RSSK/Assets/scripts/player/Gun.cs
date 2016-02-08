﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Gun : NetworkBehaviour {

	private int curAmmo;
	private float RoFTime;
	private bool canFire;

	public float range = 100;
	public int maxAmmo = 30;
	public int spareAmmo = 90;
	public float rateOfFire = 1;

	public List<Vector2> gunSpread = new List<Vector2>();
	private int gunSpreadI;

	public GameObject projectile;

	private GameObject barrel;

	// Use this for initialization
	void Start ()
	{
		curAmmo = maxAmmo;
		gunSpreadI = 0;
		Debug.Log("curammo = " + curAmmo);
		barrel = transform.FindChild("teat gun/barrel point").gameObject;
		canFire = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!canFire)
		{
			if ((Time.time - RoFTime) >= rateOfFire)
			{
				canFire = true;
			}
		}
	}

	[Command] public void CmdShoot()
	{
		if (canFire)
		{
			if (curAmmo >= 0)
			{
				RoFTime = Time.time;
				canFire = false;

				//Debug.Log("fire current ammo " + curAmmo);
				--curAmmo;

				float targetX = Screen.width / 2 + gunSpread[gunSpreadI].x;
				float targetY = Screen.height / 2 + gunSpread[gunSpreadI].y;
				++gunSpreadI;
				if (gunSpreadI == gunSpread.Count - 1)
				{
					gunSpreadI = 0;
				}

				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay(new Vector2(targetX, targetY));
				if (Physics.Raycast(ray, out hit))
				{
					if (hit.collider != null)
					{
						//Debug.Log("hit");
						//gun to target ray
						Debug.DrawLine(barrel.transform.position, hit.point, Color.blue, 10);
						//screen to target ray
						Debug.DrawLine(ray.origin, hit.point, Color.red, 10);

						//creating the bullet locally
						GameObject curBull = Instantiate(projectile, barrel.transform.position, Quaternion.identity) as GameObject;
						curBull.GetComponent<Rigidbody>().velocity = transform.right * curBull.GetComponent<GunProjectile>().speed;
						//telling the server to spawn this bullet for everyone
						NetworkServer.Spawn(curBull);
					}
				}
				else
				{
					//Debug.Log("no hit");
					//gun to target ray
					Debug.DrawLine(barrel.transform.position, ray.GetPoint(range), Color.blue, 10);
					//screen to target ray
					Debug.DrawLine(ray.origin, ray.GetPoint(range), Color.red, 10);

					//creating the bullet locally
					GameObject curBull = Instantiate(projectile, barrel.transform.position, Quaternion.identity) as GameObject;
					curBull.GetComponent<Rigidbody>().velocity = transform.right * curBull.GetComponent<GunProjectile>().speed;
					//telling the server to create it for everyone
					NetworkServer.Spawn(curBull);
				}
			}
			else
			{
				Debug.Log("reload");
			}
		}
	}

	public void reload()
	{
		//this could be alot better
		while (curAmmo != maxAmmo && spareAmmo > 0)
		{
			++curAmmo;
			--spareAmmo;
		}
	}
}
