using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerStats : NetworkBehaviour
{
	PlayerAudioController playerAudio;
	Text text;

	[SyncVar(hook = "HealthChanged")]
	public int maxHealth = 100;

	void Start()
	{
		playerAudio = GetComponent<PlayerAudioController>();
		text = ((GameManager)NetworkManager.singleton).canvas.transform.Find("Health").GetComponent<Text>();
	}

	void Update()
	{
		if (isLocalPlayer)
			if (Input.GetKeyDown(KeyCode.K))
				Damage(1000);
	}

	public void Damage(int dmg)
	{
		if (maxHealth <= 0) //just a safety check
			return;

		maxHealth -= dmg;
		if (maxHealth <= 0)
			((GameManager)NetworkManager.singleton).OnPlayerDied(gameObject);		
	}

	void HealthChanged(int newHealth)
	{
		maxHealth = newHealth;
		text.text = "Health: " + maxHealth;
		if (maxHealth <= 0)
			playerAudio.dead();
		else
			playerAudio.damaged();
	}
}
