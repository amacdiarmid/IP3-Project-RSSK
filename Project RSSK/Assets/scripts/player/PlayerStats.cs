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
		text = GetComponentInChildren<Text>();
	}

    void Update()
    {
        if (isLocalPlayer)
            if (Input.GetKeyDown(KeyCode.K))
                Damage(1000);
    }

    public void Damage(int dmg)
    {
        if(isServer)
        {
            if (maxHealth <= 0)
                return;

            maxHealth -= dmg;
            if (maxHealth <= 0)
                ((GameManager)NetworkManager.singleton).OnPlayerDied(gameObject);
        }

        if (isLocalPlayer)
            playerAudio.damaged();
    }

    void HealthChanged(int newHealth)
    {
        maxHealth = newHealth;
        text.text = "Health: " + maxHealth;
        if (maxHealth <= 0)
            playerAudio.dead();
    }
}
