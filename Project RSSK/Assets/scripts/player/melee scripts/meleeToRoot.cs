using UnityEngine;
using System.Collections;

public class meleeToRoot : MonoBehaviour {

	public void meleeHitPoint()
	{
		this.gameObject.GetComponentInParent<MeleeWeapon>().attackAni();
	}

	public void attackNum(int i)
	{
		if (i == 1)
		{
			Debug.Log("main attack");
		}
		else if (i == 2)
		{
			Debug.Log("2nd attack");
		}
		else if (i == 3)
		{
			Debug.Log("3rd attack");
		}
	}

	public void endCombo()
	{
		this.gameObject.GetComponentInParent<MeleeWeapon>().endCombo();
	}
}
