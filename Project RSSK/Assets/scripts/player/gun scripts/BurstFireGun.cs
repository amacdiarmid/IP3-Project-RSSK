using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

//fire several projectiles in one press of the mouse
public class BurstFireGun : Gun {

	public int burstCount = 3;
	public float burstTime = 0.1f;

	public override void checkInput()
	{
		//Debug.Log("over written input");
		if (!canFire)
		{
			RoFTime += Time.deltaTime;
			if (RoFTime >= rateOfFire)
			{
				canFire = true;
			}
		}

		if (Input.GetButtonDown("Fire1"))
		{
			Debug.Log("fire 1 down");
			StartCoroutine(burstFire());
		}
		else if (Input.GetButtonUp("Reload"))
		{
			Debug.Log("reload down");
			reload();
		}
		else if (!Input.GetButton("Fire1"))
		{
			//reduce the gun spread
			gunSreadVal -= gunSreadVal - (spreadDep * Time.deltaTime);
		}
	}

	IEnumerator burstFire()
	{
		if (canFire)
		{
			for (int i = 0; i < burstCount; i++)
			{
				Debug.Log("fire");
				CmdShoot();
				yield return new WaitForSeconds(burstTime);
			}
		}
	}

	[Command] public override void CmdShoot()
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
