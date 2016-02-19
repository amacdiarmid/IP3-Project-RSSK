using UnityEngine;
using System.Collections;

//fire one shot per click of the mouse
public class SemiAutoGun : Gun {

	public override void checkInput()
	{
		Debug.Log("over written input");
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
			CmdShoot();
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
}
