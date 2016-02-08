using UnityEngine;
using System.Collections;

public class GunProjectile : MonoBehaviour {

	public int damage = 10;
	public float speed = 10;

	private Vector3 startPoint;
	private Vector3 endPoint;
	private float startTime;
	private float journyLength;
	private bool start;


	// Use this for initialization
	void Start ()
	{
		startPoint = this.transform.position;
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (start)
		{
			lerpProjectile();
		}
	}

	public void setEndPoint(Vector3 end)
	{
		endPoint = end;
		journyLength = Vector3.Distance(startPoint, endPoint);
		start = true;
	}

	void lerpProjectile()
	{
		float distCovered = (Time.time - startTime) * speed;
		float fractJourny = distCovered / journyLength;

		this.transform.position = Vector3.Lerp(startPoint, endPoint, fractJourny);

		if (distCovered >= 100)
		{
			Destroy(this.gameObject);
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player")
		{
			Debug.Log("hit player" + col.gameObject.name);
			col.gameObject.GetComponent<PlayerStats>().damaged(damage);
			Destroy(this.gameObject);
		}
	}
}
