using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class HUDcomps : MonoBehaviour
{
	public Slider HPSlider;
	public Text HPText;

	public Text AmmoText;
	public Image AmmoImage;

	public Image CrosshairImg;

	public Image DamageImg;

	public Text YellowScore, BlueScore, TimeLim;

	public List<Image> yellowTeam, blueTeam;
}
