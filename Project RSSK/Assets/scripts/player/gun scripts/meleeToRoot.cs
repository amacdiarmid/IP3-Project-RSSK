using UnityEngine;
using System.Collections;

public class meleeToRoot : MonoBehaviour {

	public void meleeHitPoint()
	{
		this.gameObject.GetComponentInParent<MeleeWeapon>().attackAni();
	}
}
