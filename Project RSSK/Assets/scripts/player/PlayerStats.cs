using UnityEngine;
using UnityEngine.Networking;

public class PlayerStats : NetworkBehaviour
{
    [SyncVar]
	private float curHealth;

	public float maxHealth = 100;
	Gun curGun;
	MeleeWeapon curMeleeWep;

	void Start()
	{
		curGun = GetComponent<Gun>();
		curMeleeWep = GetComponent<MeleeWeapon>();
	}

	// Update is called once per frame
	void Update ()
	{
		if (!isLocalPlayer)
			return;

		if (Input.GetButton("Fire2"))
		{
			Debug.Log("fire 2 down");
			curMeleeWep.attack();
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
