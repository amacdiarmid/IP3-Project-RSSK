using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class MeleeWeapon : NetworkBehaviour
{
	public int damage = 10;
	public float cooldown = 5;
	public float range = 10;
	public float knockback = 5;
    public Animator swordAni;   //todo set up a search to find the correct animator

    private bool canAttack = true;
	private float countdownTimer;

	public List<AudioClip> attackAudio;
	public AudioSource audioSource;

	// Update is called once per frame
	void Update()
	{
		if (!canAttack)
		{
			countdownTimer += Time.deltaTime;
			if (countdownTimer >= cooldown)
				canAttack = true;
		}

		if (canAttack && Input.GetButtonDown("Fire2"))
			attack();
	}

	public void attack()
	{
		canAttack = false;
        countdownTimer = 0;

		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
		if (Physics.Raycast(ray, out hit, range))
		{
			Debug.Log("hit melee wep " + hit.collider.gameObject.name);
            Debug.DrawLine(ray.origin, hit.point, Color.yellow, 10);
            if (hit.collider.gameObject.tag == "TestPlayer")
				hit.collider.gameObject.GetComponent<TestPlayer>().hit();
			else if (hit.collider.gameObject.tag == "Player") //need to test with other people 
                hit.collider.gameObject.GetComponent<PlayerStats>().CmdDamage(damage);
		}
		else
            Debug.DrawLine(ray.origin, ray.GetPoint(range), Color.magenta, 10);

        audioSource.PlayOneShot(attackAudio[Random.Range(0, attackAudio.Count - 1)]);
		swordAni.SetTrigger("attack");
	}

	public void attackAni()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));

		if (Physics.Raycast(ray, out hit, range))
		{
			Debug.Log("hit melee wep " + hit.collider.gameObject.name);
            if (hit.collider.gameObject.tag == "TestPlayer" || hit.collider.gameObject.tag == "Player")
            {
                hit.collider.GetComponent<Rigidbody>().AddForce(transform.forward * (knockback * 2), ForceMode.Impulse);
                this.GetComponent<Rigidbody>().AddForce(-transform.forward * knockback, ForceMode.Impulse);
            }
		}
	}

	public void endCombo()
	{
		canAttack = false;
	}
}
