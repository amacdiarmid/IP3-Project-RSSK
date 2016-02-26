using UnityEngine;
using UnityEngine.Networking;

public class PlayerStats : NetworkBehaviour
{
	[SyncVar]
	private float curHealth;

	private PlayerAudioController playerAudio;

	public float maxHealth = 100;
	Gun curGun;
	MeleeWeapon curMeleeWep;

	void Start()
	{
		curGun = GetComponent<Gun>();
		curMeleeWep = GetComponent<MeleeWeapon>();
		playerAudio = GetComponent<PlayerAudioController>();

		playerAudio.spawn();
	}

	// Update is called once per frame
	void Update ()
	{
		if (!isLocalPlayer)
			return;
	}

	public void damaged(int dam)
	{
		maxHealth -= dam;
		if (maxHealth <= 0)
		{
			Debug.Log(name + " dead");
			playerAudio.dead();
		}
		else
			playerAudio.damaged();
	}
}
