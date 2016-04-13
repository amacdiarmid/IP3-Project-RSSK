using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerStats : NetworkBehaviour
{
	PlayerAudioController playerAudio;
	Text text;
	PlayerCamera playerCam;
	PlayerHUD HUD;
	bool ragdollTest = false;
	public GameObject Mesh;
	public GameObject deathExplosion;

	[SyncVar(hook = "HealthChanged")]
	public int maxHealth = 100;

	void Start()
	{
		playerAudio = GetComponent<PlayerAudioController>();
		if (isLocalPlayer)
		{
			playerCam = this.GetComponent<PlayerCamera>();
			HUD = GameObject.Find("HUD man").GetComponent<PlayerHUD>();
			HUD.Spawn(gameObject);
		}
	}

	void Update()
	{
		if (isLocalPlayer)
			if (Input.GetKeyDown(KeyCode.K))
				Damage(1000, this.gameObject);
	}

	//SERVER-SIDE ONLY STUFF
	public void Damage(int dmg, GameObject personWhoHit)
	{
		if (maxHealth <= 0) //just a safety check
			return;

		maxHealth -= dmg;

		if (maxHealth <= 0)
			StartCoroutine(RespawnTimer());
	}

	IEnumerator RespawnTimer()
	{
		yield return new WaitForSeconds(5); // respawn timer
		((GameManager)NetworkManager.singleton).OnPlayerDied(gameObject);
	}

	//CLIENT-SIDE ONLY
	void HealthChanged(int newHealth)
	{
		int dmg = maxHealth - newHealth;
		maxHealth = newHealth;
		if(isLocalPlayer)
			HUD.Damaged(dmg);

		if (maxHealth <= 0) 
		{
			//playerCam.Dead (personWhoHit);
			playerAudio.dead ();

			if (isLocalPlayer) 
			{
				if (ragdollTest)
					GetComponent<NetworkAnimator> ().animator.enabled = false;
				else 
					CmdSpawnExplosion();

				GetComponent<CharacterController> ().enabled = false;
				Gun g = GetComponent<Gun> ();
				if (g)
					g.enabled = false;
				MeleeWeapon m = GetComponent<MeleeWeapon> ();
				if (m)
					m.enabled = false;
			}
		} 
		else 
		{
			playerAudio.damaged ();
			if(isLocalPlayer)
				playerCam.damShake();
		}
	}

	[Command]
	void CmdSpawnExplosion()
	{
		RpcTurnOffObj();
		GameObject tempExplosion = Instantiate(deathExplosion, this.transform.position, Quaternion.identity) as GameObject;
		NetworkServer.Spawn(tempExplosion);
		Destroy(tempExplosion, 5);
	}

	[ClientRpc]
	void RpcTurnOffObj()
	{
		Mesh.SetActive(false);
	}
}
