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

	NetworkAnimator swordAni;   //todo set up a search to find the correct animator
	public MeleeTrigger weaponCol;
	private int meleeHash;

	private bool canAttack = true;
	private float countdownTimer;
	private int comboPos = 0;

	public List<AudioClip> attackAudio;
	public AudioSource audioSource;

	void Start()
	{
		weaponCol.setValues(gameObject, this);
		weaponCol.active(false);
		swordAni = GetComponent<NetworkAnimator>();
		if (swordAni == null)
			Debug.LogError ("Setup: Failed to find NetworkAnimator");
		meleeHash = swordAni.animator.GetLayerIndex("MeleeLayer");
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
		yield return new WaitForSeconds(swordAni.animator.GetCurrentAnimatorStateInfo(meleeHash).length);
		canAttack = true;
		yield return new WaitForSeconds(5);
		comboPos = 0;
	}

	IEnumerator triggerEnable()
	{
		weaponCol.active(true);
		yield return new WaitForSeconds(swordAni.animator.GetCurrentAnimatorStateInfo(meleeHash).length);
		weaponCol.active(false);
	}

	[Command]
	public void CmdHit(GameObject obj)
	{
		obj.GetComponent<PlayerStats>().Damage(damage);
	}
}
