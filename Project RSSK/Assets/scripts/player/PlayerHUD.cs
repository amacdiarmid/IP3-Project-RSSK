using UnityEngine;
using System.Collections;

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
	
	public void Spawn(GameObject player)
	{
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

	}

	// Update is called once per frame
	void Update ()
	{
		HUDComps.HPText.text = playerStats.maxHealth + "/" + maxHP;
		HUDComps.HPSlider.value = ((float)playerStats.maxHealth / maxHP) * 100;

		if (hasGun)
		{
			HUDComps.AmmoText.text = playerGun.getCurAmmo() + "/" + playerGun.spareAmmo;
			HUDComps.CrosshairImg.rectTransform.sizeDelta = new Vector2(startCrosshairSize.x + playerGun.getCurSpread(), startCrosshairSize.y + playerGun.getCurSpread());
		}
	}
}
