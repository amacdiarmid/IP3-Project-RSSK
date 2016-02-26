using UnityEngine;
using System.Collections;

public class GunProjectile : MonoBehaviour {

	public int damage = 10;
	public float speed = 10;

	public int canHitTargets = 1;
	[Range(0, 100)] public float damageReduction = 100;

	float startTime;

	void Start ()
	{
		Destroy(gameObject, Time.time + 5);
	}
	
	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player")
		{
			col.gameObject.GetComponent<PlayerStats>().damaged(damage);
			canHitTargets--;
			damage = (int)(damage * damageReduction);
			if (canHitTargets == 0)
			{
				Destroy(gameObject);
			}
		}
		else if (col.gameObject.tag == "TestPlayer")
		{
			col.gameObject.GetComponent<TestPlayer>().shot();
			canHitTargets--;
			damage = (int)(damage * damageReduction);
			if (canHitTargets == 0)
			{
				Destroy(gameObject);
			}
		}
		else if (col.gameObject.tag != "Bullet")
		{
			Destroy(gameObject);
		}
	}
}
