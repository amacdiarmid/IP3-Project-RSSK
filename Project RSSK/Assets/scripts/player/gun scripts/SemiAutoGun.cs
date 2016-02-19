using UnityEngine;
using System.Collections;

//fire one shot per click of the mouse
public class SemiAutoGun : Gun
{
	public override void checkInput()
	{
        if (!canFire)
            canFire = (RoFTime += Time.deltaTime) >= rateOfFire;
        else if (Input.GetButtonDown("Fire1"))
            CmdShoot();

        if (Input.GetButtonUp("Reload"))
			reload();

        gunSreadVal -= gunSreadVal - (spreadDep * Time.deltaTime);
    }
}
