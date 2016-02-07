using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour {

	private float curHealth;

	public float maxHealth = 100;
	public Gun curGun;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetButton("Fire1"))
		{
			Debug.Log("fire 1 down");
			curGun.shoot();
		}

		if (Input.GetButtonUp("Reload"))
		{
			Debug.Log("reload down");
			curGun.reload();
		}
	}
}
