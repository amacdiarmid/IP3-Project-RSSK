using UnityEngine;
using System.Collections;

public class MeleeTrigger : MonoBehaviour {

	private GameObject self;
	private BoxCollider wepCol;
	private MeleeWeapon ownerWeap;
	private PlayerTeam curTeam;

	// Use this for initialization
	public void setValues(GameObject sel, MeleeWeapon ownerWeap)
	{
		self = sel;
		wepCol = GetComponent<BoxCollider>();
		this.ownerWeap = ownerWeap;
		curTeam = sel.GetComponent<PlayerController>().team;
	}

	public void active(bool active)
	{
		wepCol.enabled = active;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.transform != self.gameObject.transform)
		{
			if (other.gameObject.tag == "TestPlayer")
				other.gameObject.GetComponent<TestPlayer>().hit();
			else if (other.gameObject.tag == "Player")
				if (other.GetComponent<PlayerController>().team != curTeam) //should work need to test with others. 
					ownerWeap.CmdHit(other.gameObject);
		}
	}
}
