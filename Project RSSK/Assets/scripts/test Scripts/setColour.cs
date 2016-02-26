using UnityEngine;
using System.Collections;

public class setColour : MonoBehaviour {

	public Color colour;

	// Use this for initialization
	void Start () {

		this.GetComponent<MeshRenderer>().material.color = colour;

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
