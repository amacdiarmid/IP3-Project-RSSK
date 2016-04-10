using UnityEngine;
using System.Collections;

public class GunProjectile : MonoBehaviour
{
	public float lifeSpan = 5;

	void Start ()
	{
		Destroy(gameObject, lifeSpan);
	}

	public void setUpLine(Vector3 start, Vector3 finish)
	{
		LineRenderer line = this.GetComponent<LineRenderer>();
		line.SetPosition(0, start);
		line.SetPosition(1, finish);
	}
}
