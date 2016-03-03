using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAudioController : MonoBehaviour {

	public List<AudioClip> jumpAudio;
	public List<AudioClip> movingAudio;
	public List<AudioClip> damageAudio;
	public List<AudioClip> deathAudio;
	public AudioClip spawnAudio;
    
    AudioSource source;
    PlayerState state;

    void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.PlayOneShot(spawnAudio);
    }

    void Update()
    {
        switch (state)
        {
            case PlayerState.run:
                if (!source.isPlaying)
                    source.PlayOneShot(movingAudio[Random.Range(0, movingAudio.Count - 1)]);
                break;
            default:
                break;
        }
    }

	public void setAudio(PlayerState state)
	{
        this.state = state;
        if(state == PlayerState.jump)
            source.PlayOneShot(jumpAudio[Random.Range(0, jumpAudio.Count - 1)]);
	}

	public void damaged()
	{
        source.PlayOneShot(damageAudio[Random.Range(0, damageAudio.Count - 1)]);
	}

	public void dead()
	{
        source.PlayOneShot(deathAudio[Random.Range(0, deathAudio.Count - 1)]);
	}
}
