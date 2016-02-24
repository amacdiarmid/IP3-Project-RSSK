using UnityEngine;
using System.Collections;

//fire while the mouse is held down
public class FullAutoGun : Gun
{
	public override void checkInput()
	{
		if (!canFire)
            canFire = (RoFTime += Time.deltaTime) >= rateOfFire;
        else if (Input.GetButton("Fire1"))
			CmdShoot();

        if (Input.GetButtonUp("Reload"))
			reload();
			
		gunSreadVal -= gunSreadVal - (spreadDep * Time.deltaTime);   //reduce the gun spread
	}
}
