using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerStats : NetworkBehaviour
{
	PlayerAudioController playerAudio;
    Text text;

    [SyncVar]
    public float maxHealth = 100;

	void Start()
	{
		playerAudio = GetComponent<PlayerAudioController>();
        text = GetComponentInChildren<Text>();
	}

    [Command]
	public void CmdDamage(int dmg)
	{
        if (!isServer)
            return;

		maxHealth -= dmg;
		if (maxHealth <= 0)
		{
			playerAudio.dead();
            Destroy(gameObject, playerAudio.deathAudio[0].length);
		}
		else
			playerAudio.damaged();

        text.text = "Health: " + maxHealth;
	}
}
