using UnityEngine;
using System.Collections;

public class MeleeTrigger : MonoBehaviour {

	private int damage;
	private GameObject self;
	private BoxCollider wepCol;

	// Use this for initialization
	public void setValues(int dam, GameObject sel)
	{
		damage = dam;
		self = sel;
		wepCol = this.GetComponent<BoxCollider>();
	}

	public void active(bool active)
	{
		Debug.Log("change trigger " + active);
		wepCol.enabled = active;
	}

	void OnTriggerEnter(Collider other)
	{

		if (other.gameObject.transform != self.gameObject.transform)
		{
			if (other.gameObject.tag == "TestPlayer")
				other.gameObject.GetComponent<TestPlayer>().hit();
			else if (other.gameObject.tag == "Player") //need to test with other people 
				other.gameObject.GetComponent<PlayerStats>().Damage(damage);

			//Destroy(other.gameObject);
		}
		
	}

}
