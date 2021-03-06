﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingTextController : MonoBehaviour {

	private static FloatingText popupText;
	private static GameObject canvas;

	public static void Initialize(){
		canvas = GameObject.Find ("Canvas");
		if (!popupText)
			popupText = Resources.Load<FloatingText> ("Prefabs/PopupTextParent");
	}

	public static void CreateFloatingText(string text, Transform location,Color color){

		FloatingText instance = Instantiate (popupText);
		if (instance != null){
			instance.SetPosition(location.position);
			Vector2 screenPosition = Camera.main.WorldToScreenPoint (location.position);
			instance.transform.SetParent (canvas.transform,false);
			instance.transform.position = screenPosition;
			instance.SetText (text);
			instance.gameObject.transform.GetChild (0).gameObject.GetComponent<Text> ().color = color;
		}
	}
}
