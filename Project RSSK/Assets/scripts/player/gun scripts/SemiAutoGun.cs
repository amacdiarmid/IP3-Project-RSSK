using UnityEngine;
using System.Collections;

//fire one shot per click of the mouse
public class SemiAutoGun : Gun
{
	public override void checkInput()
	{
		if (!canFire)
			canFire = (RoFTime += Time.deltaTime) >= rateOfFire;
		else if ((primWeap && Input.GetButtonDown("Fire1")) || (!primWeap && Input.GetButtonDown("Fire2")))
			Shoot();

		if (Input.GetButtonUp("Reload"))
			reload();

		gunSreadVal -= gunSreadVal - (spreadDep * Time.deltaTime);
	}
}
