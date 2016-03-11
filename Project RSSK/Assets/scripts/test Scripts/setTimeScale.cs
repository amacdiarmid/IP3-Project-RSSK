using UnityEngine;
using System.Collections;

public class setTimeScale : MonoBehaviour {

	public float setTime;

	// Use this for initialization
	void Start ()
	{
		Time.timeScale = setTime;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
