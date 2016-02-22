using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

//fire multiple projectiles with one press
public class ScattershotGun : Gun
{
	public int peletCount = 5;

	public override void checkInput()
	{
		if (!canFire)
			canFire = (RoFTime += Time.deltaTime) >= rateOfFire;
		else if (Input.GetButtonDown("Fire1"))
			for (int i = 0; i < peletCount; i++)
				CmdShoot();

		if (Input.GetButtonUp("Reload"))
			reload();

		gunSreadVal -= spreadDep * Time.deltaTime;  //reduce the gun spread
	}
}
