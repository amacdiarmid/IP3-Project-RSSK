using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

//dont use this for that characters use one of the derived classes
public class Gun : NetworkBehaviour {

	protected int curAmmo;
	protected float RoFTime;
	protected bool canFire;

	public float range = 100;
	public int maxAmmo = 30;
	public int spareAmmo = 90;
	public float rateOfFire = 1;

	public float maxSpread = 3;
	public float spreadAdv = 0.2f;
	public float spreadDep = 0.1f;
	protected float gunSreadVal;

	public GameObject projectile;

	protected GameObject barrel;

	// Use this for initialization
	void Start ()
	{
		curAmmo = maxAmmo;
		gunSreadVal = 0;
		Debug.Log("curammo = " + curAmmo);
		barrel = transform.FindChild("camera/teat gun/barrel point").gameObject;
		canFire = true;
		RoFTime = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		checkInput();
	}

	public virtual void checkInput()
	{
		Debug.Log("dont use this gun class");
	}

	[Command] public virtual void CmdShoot()
	{
		if (canFire)
		{
			if (curAmmo >= 0)
			{
				RoFTime = 0;
				canFire = false;

				//Debug.Log("fire current ammo " + curAmmo);
				--curAmmo;

				//spread to the current shot
				float targetX = Screen.width / 2 + Random.Range(-gunSreadVal, gunSreadVal);
				float targetY = Screen.height / 2 + Random.Range(-gunSreadVal, gunSreadVal);

				//Vector3 bulletSpawn = new Vector3(barrel.transform.position.x + gunSpread[gunSpreadI].x, barrel.transform.position.y + gunSpread[gunSpreadI].y, barrel.transform.position.z);
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

						//alex test version
						curBull.GetComponent<Rigidbody>().velocity = (hit.point - barrel.transform.position).normalized * curBull.GetComponent<GunProjectile>().speed;

						//dan version
						//curBull.GetComponent<Rigidbody>().velocity = transform.right * curBull.GetComponent<GunProjectile>().speed;
						
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

					//alex version
					curBull.GetComponent<Rigidbody>().velocity = (ray.GetPoint(range) - barrel.transform.position).normalized * curBull.GetComponent<GunProjectile>().speed;

					//dan version
					//curBull.GetComponent<Rigidbody>().velocity = transform.right * curBull.GetComponent<GunProjectile>().speed;
					
					//telling the server to create it for everyone
					NetworkServer.Spawn(curBull);
				}

				//add spread to the next shot 
				gunSreadVal = Mathf.Clamp(gunSreadVal + spreadAdv, 0, maxSpread);
				Debug.Log(gunSreadVal);
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
		gunSreadVal = 0;
	}
}
