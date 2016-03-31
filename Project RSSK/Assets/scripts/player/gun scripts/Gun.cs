using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

//dont use this for that characters use one of the derived classes
public class Gun : NetworkBehaviour
{

	protected int curAmmo;
	protected float RoFTime = 0;
	protected bool canFire = true;
	protected float gunSreadVal = 0;
	public Transform barrel;
	public GameObject bulletTrail;

	public bool primWeap;
	public float range = 100;
	public int maxAmmo = 30;
	public int spareAmmo = 90;
	public float rateOfFire = 1;
	public int damage = 50;

	public float maxSpread = 3;
	public float spreadAdv = 0.2f;
	public float spreadDep = 0.1f;

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
		//barrel = transform.FindChild("camera/barrel point");
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
		if (curAmmo >= 0)
		{
			RoFTime = 0;
			canFire = false;

			--curAmmo;

			float targetX = Screen.width / 2 + Random.Range(-gunSreadVal, gunSreadVal);
			float targetY = Screen.height / 2 + Random.Range(-gunSreadVal, gunSreadVal);
			Debug.Log(targetX +" " + targetY);

			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(new Vector2(targetX, targetY));
			if (Physics.Raycast(ray, out hit))
			{
				//gun to target ray
				Debug.DrawLine(barrel.transform.position, hit.point, Color.blue, 10);
				GameObject trail = Instantiate(bulletTrail);
				trail.GetComponent<GunProjectile>().setUpLine(barrel.transform.position, hit.point);
				//screen to target ray
				Debug.DrawLine(ray.origin, hit.point, Color.red, 10);
				if (hit.collider.tag == "Player")
					CmdHit (hit.transform.gameObject, damage);
			}
			else
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
			gunSreadVal = Mathf.Clamp(gunSreadVal + spreadAdv, 0, maxSpread);
			Debug.Log(gunSreadVal);
		}
		else
		{
			Debug.Log("reload");
			audioSource.PlayOneShot(outOfAmmoAudio);
			//reload();
		}


		//dan version
		//Vector3 pos = barrel.position;
		//Vector3 velocity = barrel.right + Random.insideUnitSphere * gunSreadVal;
		//velocity = velocity.normalized * projectile.GetComponent<GunProjectile>().speed;
		//CmdSpawnBullet(pos, velocity);

		//audio/ani call


	}

	public void reload()
	{
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
