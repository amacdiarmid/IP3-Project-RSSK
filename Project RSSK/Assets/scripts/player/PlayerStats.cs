using UnityEngine;
using UnityEngine.Networking;

public class PlayerStats : NetworkBehaviour {

	private float curHealth;

	public float maxHealth = 100;
	Gun curGun;

	void Start()
	{
		curGun = GetComponent<Gun>();
	}

	// Update is called once per frame
	void Update ()
	{
		if (!isLocalPlayer)
			return;

		if (Input.GetButton("Fire1"))
		{
			Debug.Log("fire 1 down");
			curGun.CmdShoot();
		}

		if (Input.GetButtonUp("Reload"))
		{
			Debug.Log("reload down");
			curGun.reload();
		}
	}

	public void damaged(int dam)
	{
		maxHealth -= dam;
		if (maxHealth <= 0)
		{
			Debug.Log(name + " dead");
		}
	}
}
