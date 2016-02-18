using UnityEngine;
using System.Collections;

public class GunProjectile : MonoBehaviour {

	public int damage = 10;
	public float speed = 10;

	float startTime;

	void Start ()
	{
		startTime = Time.time;
	}
	
	void Update ()
	{
		//giving it a 10 second life
		if (Time.time > startTime + 5)
			Destroy(gameObject);
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player")
		{
			Debug.Log("hit player" + col.gameObject.name);
			col.gameObject.GetComponent<PlayerStats>().damaged(damage);
			Destroy(gameObject);
		}
		//for the test dummies
		if (col.gameObject.tag == "TestPlayer")
		{
			Debug.Log("hit dummie" + col.gameObject.name);
			col.gameObject.GetComponent<TestPlayer>().shot();
			Destroy(gameObject);
		}
		//for the level walls etc
		if (col.gameObject.tag == "Obstacle")
		{
			Debug.Log("hit Obstacle" + col.gameObject.name);
			Destroy(gameObject);
		}
	}
}
