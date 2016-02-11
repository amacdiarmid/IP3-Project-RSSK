using UnityEngine;
using System.Collections;

public class MeleeWeapon : MonoBehaviour {

	public float lungeRange = 5;
	public int damage = 10;
	public float cooldown = 5;
	public float range = 10;

	private bool canAttack;
	private float countdownTimer;
	private PlayerController playerCon;
	public Animator swordAni;   //todo set up a search to find the correct animator
	// Use this for initialization
	void Start ()
	{
		canAttack = true;
		playerCon = GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!canAttack)
		{
			countdownTimer += Time.deltaTime;
			if (countdownTimer >= cooldown)
			{
				canAttack = true;
			}
		}
	}

	public void attack()
	{
		if (canAttack)
		{
			canAttack = false;
			countdownTimer = 0;

			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width/ 2, Screen.height/ 2));
			if (Physics.Raycast(ray, out hit))
			{
				Debug.Log("hit melee wep " + hit.collider.gameObject.name);
				if (hit.collider.gameObject.tag == "TestPlayer")
				{
					if (Vector3.Distance(transform.position, hit.point) <= range)
					{
						Debug.DrawLine(ray.origin, hit.point, Color.yellow, 10);
						swordAni.SetTrigger("attack");
						hit.collider.gameObject.GetComponent<TestPlayer>().hit();
					}
					else if (Vector3.Distance(transform.position, hit.point) <= lungeRange)
					{
						Debug.DrawLine(ray.origin, hit.point, Color.black, 10);
						playerCon.goLunge(hit.point, range);
						swordAni.SetTrigger("attack");
						hit.collider.gameObject.GetComponent<TestPlayer>().hit();
					}
					else
					{
						Debug.DrawLine(ray.origin, ray.GetPoint(lungeRange), Color.magenta, 10);
						swordAni.SetTrigger("attack");
					}
				}
			}
			else
			{
				Debug.DrawLine(ray.origin, ray.GetPoint(lungeRange), Color.magenta, 10);
				swordAni.SetTrigger("attack");
			}
		}
	}

	public void attackAni()
	{

	}
}
