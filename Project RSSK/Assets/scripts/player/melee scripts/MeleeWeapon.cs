using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class MeleeWeapon : NetworkBehaviour
{
	public bool primWeap;
	public int damage = 10;
	public float cooldown = 5;
	public float comboCoolDown = 1;
	public float range = 10;
	public float knockback = 5;

	public NetworkAnimator swordAni;   //todo set up a search to find the correct animator
	public MeleeTrigger weaponCol;
	private int meleeHash;

	private bool canAttack = true;
	private float countdownTimer;
	private int comboPos = 0;

	public List<AudioClip> attackAudio;
	public AudioSource audioSource;

	void Start()
	{
		weaponCol.setValues(damage, this.gameObject);
		weaponCol.active(false);
		meleeHash = swordAni.GetLayerIndex("MeleeLayer");
		Debug.Log(swordAni.GetLayerName(meleeHash));
	}

	// Update is called once per frame
	void Update()
	{
        if (!isLocalPlayer)
            return;

		if (canAttack && ((primWeap && Input.GetButtonDown("Fire1")) || (!primWeap && Input.GetButtonDown("Fire2"))) && comboPos < 3)
			attack();
	}

	public void attack()
	{
		comboPos++;

		StopCoroutine(comboWait());

		audioSource.PlayOneShot(attackAudio[Random.Range(0, attackAudio.Count - 1)]);


		if (comboPos == 1)
			swordAni.SetTrigger("attack1");
		else if (comboPos == 2)
			swordAni.SetTrigger("attack2");
		else if (comboPos == 3)
			swordAni.SetTrigger("attack3");

		StartCoroutine(comboWait());
		StartCoroutine(triggerEnable());


	}

	IEnumerator comboWait()
	{
		canAttack = false;
		Debug.Log("cant attack");
		yield return new WaitForSeconds(swordAni.GetCurrentAnimatorStateInfo(meleeHash).length);
		canAttack = true;
		Debug.Log("can attack");
		yield return new WaitForSeconds(5);
		comboPos = 0;
		Debug.Log("reset combo");
	}

	IEnumerator triggerEnable()
	{
		Debug.Log("enter co routing");
		weaponCol.active(true);
		Debug.Log(swordAni.GetCurrentAnimatorStateInfo(meleeHash).length);
		yield return new WaitForSeconds(swordAni.GetCurrentAnimatorStateInfo(meleeHash).length);
		weaponCol.active(false);
	}
}
