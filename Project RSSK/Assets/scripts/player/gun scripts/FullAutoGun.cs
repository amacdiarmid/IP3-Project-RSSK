using UnityEngine;
using System.Collections;

//fire while the mouse is held down
public class FullAutoGun : Gun
{
	public override void checkInput()
	{
		if (!canFire)
			canFire = (RoFTime += Time.deltaTime) >= rateOfFire;
		else if ((primWeap && Input.GetButton("Fire1")) || (!primWeap && Input.GetButton("Fire2")))
			Shoot();
		else
			gunSreadVal -= gunSreadVal - (spreadDep * Time.deltaTime);   //reduce the gun spread

		if (Input.GetButtonUp("Reload"))
			reload();
			
	}
}
