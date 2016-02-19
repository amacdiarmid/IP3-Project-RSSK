using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

//fire several projectiles in one press of the mouse
public class BurstFireGun : Gun
{
	public int burstCount = 3;
	public float burstTime = 0.1f;

	public override void checkInput()
	{
		if (!canFire)
            canFire = (RoFTime += Time.deltaTime) >= rateOfFire;
		else if (Input.GetButtonDown("Fire1"))
			StartCoroutine(burstFire());

        if (Input.GetButtonUp("Reload"))
			reload();
		
	    gunSreadVal -= gunSreadVal - (spreadDep * Time.deltaTime);  //reduce the gun spread
	}

	IEnumerator burstFire()
	{
		for (int i = 0; i < burstCount; i++)
		{
			CmdShoot();
			yield return new WaitForSeconds(burstTime);
		}
	}
}
