using UnityEngine;
using System.Collections;

public class MeleeTrigger : MonoBehaviour {

	private GameObject self;
	private BoxCollider wepCol;
	private MeleeWeapon ownerWeap;

	// Use this for initialization
	public void setValues(GameObject sel, MeleeWeapon ownerWeap)
	{
		self = sel;
		wepCol = GetComponent<BoxCollider>();
		this.ownerWeap = ownerWeap;
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
			else if (other.gameObject.tag == "Player") //need to test with other people 
				ownerWeap.CmdHit(other.gameObject);
		}
	}
}
