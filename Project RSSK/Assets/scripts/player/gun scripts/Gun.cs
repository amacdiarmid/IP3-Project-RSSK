using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

//dont use this for that characters use one of the derived classes
public class Gun : NetworkBehaviour {

	protected int curAmmo;
	protected float RoFTime = 0;
	protected bool canFire = true;
	protected float gunSreadVal = 0;
	protected Transform barrel;

	public bool primWeap;
	public float range = 100;
	public int maxAmmo = 30;
	public int spareAmmo = 90;
	public float rateOfFire = 1;

	public float maxSpread = 3;
	public float spreadAdv = 0.2f;
	public float spreadDep = 0.1f;
	
	public GameObject projectile;

	public Animator gunAni;   //todo set up a search to find the correct animator

	public AudioClip fireAudio;
	public AudioClip outOfAmmoAudio;
	public AudioClip reloadAudio;
	public AudioSource audioSource;

	void Start ()
	{
		curAmmo = maxAmmo;
		barrel = transform.FindChild("camera/barrel point");
	}
	
	void Update ()
	{
		checkInput();
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

            //dan version
            Vector3 pos = barrel.position;
            Vector3 velocity = barrel.right + Random.insideUnitSphere * gunSreadVal;
            velocity = velocity.normalized * projectile.GetComponent<GunProjectile>().speed;
            CmdSpawnBullet(pos, velocity);

            //audio/ani call
            audioSource.PlayOneShot(fireAudio);
			gunAni.SetTrigger("fire");
					
			//add spread to the next shot 
			gunSreadVal = Mathf.Clamp(gunSreadVal + spreadAdv, 0, maxSpread);
		}
		else
		{
			Debug.Log("reload");
			audioSource.PlayOneShot(outOfAmmoAudio);
		}
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

    [Command]
    void CmdSpawnBullet(Vector3 pos, Vector3 velocity)
    {
        GameObject curBull = Instantiate(projectile, pos, Quaternion.identity) as GameObject;
        curBull.GetComponent<Rigidbody>().velocity = velocity;
        NetworkServer.Spawn(curBull);
    }
}
