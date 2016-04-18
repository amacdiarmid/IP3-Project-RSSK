using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum retHightlight
{
	friendly,
	foe,
	no,
}


public class PlayerHUD : MonoBehaviour {

	public GameObject yellowHud;
	public GameObject blueHud;

	private PlayerStats playerStats;
	private PlayerController playerCont;
	private Gun playerGun;
	private bool hasGun;

	private HUDcomps HUDComps;

	private Vector2 startCrosshairSize;

	private int maxHP;

	private Color startDamageColour;
	private Color damageColour;
	private float damageTime;

	private Color StartCrosshailColour;

	public GameManager gameMan;
	public Transform objective;
	public Canvas objectCanvas;
	public float ObjectMarkerScaleValue = 0.01f;
	public List<Sprite> characters;

	byte[] playerTeams, playerChars;

	public void Spawn(GameObject player)
	{
		if (HUDComps)
			Destroy(HUDComps.gameObject);

		playerStats = player.GetComponent<PlayerStats>();
		playerCont = player.GetComponent<PlayerController>();

		if (playerCont.team == PlayerTeam.TeamYellow)
			HUDComps = Instantiate(yellowHud).GetComponent<HUDcomps>();
		else
			HUDComps = Instantiate(blueHud).GetComponent<HUDcomps>();

		maxHP = playerStats.maxHealth;

		if (player.GetComponent<Gun>())
		{
			hasGun = true;
			playerGun = player.GetComponent<Gun>();
		}
		else
		{
			hasGun = false;
			HUDComps.AmmoImage.enabled = false;
			HUDComps.AmmoText.enabled = false;
		}

		startCrosshairSize = HUDComps.CrosshairImg.rectTransform.sizeDelta;

		startDamageColour = HUDComps.DamageImg.color;
		damageColour = startDamageColour;
		damageTime = 0;

		StartCrosshailColour = HUDComps.CrosshairImg.color;
	}

	// Update is called once per frame
	void Update ()
	{
		if (!playerStats)
			return;

		HUDComps.HPText.text = playerStats.maxHealth + "/" + maxHP;
		HUDComps.HPSlider.value = ((float)playerStats.maxHealth / maxHP) * 100;

		if (hasGun)
		{
			HUDComps.AmmoText.text = playerGun.getCurAmmo() + "/" + playerGun.maxAmmo;
			HUDComps.CrosshairImg.rectTransform.sizeDelta = new Vector2(startCrosshairSize.x + playerGun.getCurSpread(), startCrosshairSize.y + playerGun.getCurSpread());
			if (playerGun.playerInRange() == retHightlight.foe)
				HUDComps.CrosshairImg.color = Color.red;
			else if (playerGun.playerInRange() == retHightlight.friendly)
				HUDComps.CrosshairImg.color = Color.green;
			else
				HUDComps.CrosshairImg.color = StartCrosshailColour;
		}

		damageTime += Time.deltaTime;
		HUDComps.DamageImg.color = Color.Lerp(damageColour, startDamageColour, damageTime);
		//Debug.Log(HUDComps.DamageImg.color);

		//might be the wrong way round
		HUDComps.YellowScore.text = gameMan.getScore(0).ToString();
		HUDComps.BlueScore.text = gameMan.getScore(1).ToString();
		HUDComps.TimeLim.text = gameMan.getCurTime().ToString();

		//setting game state
		if (playerTeams != null) 
		{
			int blueI = 0, yellowI = 0;
			Image curImg;
			for (int i = 0; i < playerTeams.Length; i++) 
			{
				byte team = playerTeams [i];
				byte character = playerChars [i];
				if ((PlayerTeam)team == PlayerTeam.TeamYellow) 
				{
					curImg = HUDComps.yellowTeam [yellowI];
					curImg.sprite = characters [character];
					yellowI++;
				} 
				else if ((PlayerTeam)team == PlayerTeam.TeamBlue) 
				{
					curImg = HUDComps.blueTeam [blueI];
					curImg.sprite = characters [character];
					blueI++;
				} else
					Debug.Log ("no team");
			}
		}

		if (objectCanvas) 
		{
			objectCanvas.transform.LookAt (playerStats.transform);

			objectCanvas.transform.localScale = Vector3.one * (Vector3.Distance (objectCanvas.transform.position, playerStats.transform.position) * ObjectMarkerScaleValue);
		}
	}

	public void Damaged(float damAmount)
	{
		float alpha = damAmount / maxHP;
		//Debug.Log(alpha);
		damageColour = startDamageColour;
		damageColour.a = damageColour.a + (alpha * 2);
		Debug.Log("damage colour " + damageColour);
		damageTime = 0;
	}

	public void setTeam(byte[] teams, byte[] chars)
	{
		playerTeams = teams;
		playerChars = chars;
	}
}
