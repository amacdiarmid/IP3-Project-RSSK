using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class testPlayerMovement : MonoBehaviour {

	public List<Vector3> path;
	public float speed = 10;
	private int i = 0;
	private float startTime;
	private float journeyLength;

	// Use this for initialization
	void Start()
	{
		changepos();
		//Debug.Log(path.Count - 1);
	}

	void Update()
	{
		float distCovered = (Time.time - startTime) * speed;
		float fracJourney = distCovered / journeyLength;
		transform.position = Vector3.Lerp(path[i], path[i+1], fracJourney);
		if (Vector3.Distance(transform.position, path[i+1]) < 2)
		{
			if (i == path.Count-2)
			{
				
				i = 0;
				//Debug.Log(i);
			}
			else
			{
				
				i++;
				//Debug.Log(i);
			}
			changepos();
		}
	}

	void changepos()
	{
		startTime = Time.time;
		journeyLength = Vector3.Distance(path[i], path[i + 1]);
	}
}
