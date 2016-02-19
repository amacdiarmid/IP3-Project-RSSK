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

    public float range = 100;
	public int maxAmmo = 30;
	public int spareAmmo = 90;
	public float rateOfFire = 1;

	public float maxSpread = 3;
	public float spreadAdv = 0.2f;
	public float spreadDep = 0.1f;
	
	public GameObject projectile;

	void Start ()
	{
		curAmmo = maxAmmo;
		barrel = transform.FindChild("camera/teat gun/barrel point");
	}
	
	void Update ()
	{
		checkInput();
	}

	public virtual void checkInput()
	{
        Debug.LogError("Don't use the base class!", gameObject);
	}

	[Command] public virtual void CmdShoot()
	{
		if (curAmmo >= 0)
		{
			RoFTime = 0;
			canFire = false;

			//Debug.Log("fire current ammo " + curAmmo);
			--curAmmo;

			//creating the bullet locally
			GameObject curBull = Instantiate(projectile, barrel.position, Quaternion.identity) as GameObject;

			//alex test version
			//curBull.GetComponent<Rigidbody>().velocity = (hit.point - barrel.transform.position).normalized * curBull.GetComponent<GunProjectile>().speed;

            //dan version
            Vector3 velocity = barrel.right + Random.insideUnitSphere * gunSreadVal;
            curBull.GetComponent<Rigidbody>().velocity = velocity.normalized * curBull.GetComponent<GunProjectile>().speed;

            //telling the server to spawn this bullet for everyone
            NetworkServer.Spawn(curBull);
					
			//add spread to the next shot 
			gunSreadVal = Mathf.Clamp(gunSreadVal + spreadAdv, 0, maxSpread);
		}
		else
		{
			Debug.Log("reload");
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
