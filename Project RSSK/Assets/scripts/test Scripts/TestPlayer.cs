using UnityEngine;
using System.Collections;

public class TestPlayer : MonoBehaviour {

	public Color defaultColour;
	public Color shotColour;
	public Color meleeColour;

	private Color curCol;

	private float time;

	// Use this for initialization
	void Start ()
	{
		this.GetComponent<MeshRenderer>().material.color = defaultColour;
		curCol = defaultColour;
		time = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		time += Time.deltaTime;
		this.GetComponent<MeshRenderer>().material.color = Color.Lerp(curCol, defaultColour, time);
	}

	public void shot()
	{
		this.GetComponent<MeshRenderer>().material.color = shotColour;
		curCol = shotColour;
		time = 0;
	}

	public void hit()
	{
		this.GetComponent<MeshRenderer>().material.color = meleeColour;
		curCol = meleeColour;
		time = 0;
	}
}
