using UnityEngine;
using System.Collections;

public class GunProjectile : MonoBehaviour
{
	public int damage = 10;
	public float speed = 10;

	void Start ()
	{
		Destroy(gameObject, Time.time + 5);
	}
	
	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player")
			col.gameObject.GetComponent<PlayerStats>().CmdDamage(damage);
		
		if (col.gameObject.tag == "TestPlayer")
			col.gameObject.GetComponent<TestPlayer>().shot();

		if(col.gameObject.tag != "Bullet" || col.gameObject.GetComponent<BoxCollider>().isTrigger != true)
			Destroy(gameObject);
	}
}
