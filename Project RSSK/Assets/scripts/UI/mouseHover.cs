using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;
using System;

public class mouseHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

	public Image charImg;
	public Sprite showSprite;

	public void OnPointerEnter(PointerEventData eventData)
	{
		Debug.Log("ender");
		charImg.sprite = showSprite;
		charImg.enabled = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		//charImg.enabled = false;
	}
}
