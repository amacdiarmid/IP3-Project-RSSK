using UnityEngine;
using System.Collections;

//fire while the mouse is held down
public class FullAutoGun : Gun {

	public override void checkInput()
	{
		if (!canFire)
		{
			RoFTime += Time.deltaTime;
			if (RoFTime >= rateOfFire)
			{
				canFire = true;
			}
		}

		if (Input.GetButton("Fire1"))
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
