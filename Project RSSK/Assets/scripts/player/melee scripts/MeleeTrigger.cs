using UnityEngine;
using System.Collections;

public class MeleeTrigger : MonoBehaviour {

	private int damage;
	private GameObject self;

	// Use this for initialization
	public void setValues(int dam, GameObject sel)
	{
		damage = dam;
		self = sel;
	}

	void OnTriggerEnter(Collider other)
	{

		Debug.Log("trigger hit " + other.gameObject.transform + " " + self.gameObject.transform);
		if (other.gameObject.transform != self.gameObject.transform)
		{
			Debug.Log("trigger destroy " + other.GetInstanceID() + " " + self.GetInstanceID());
			Destroy(other.gameObject);
		}
		
	}
}
