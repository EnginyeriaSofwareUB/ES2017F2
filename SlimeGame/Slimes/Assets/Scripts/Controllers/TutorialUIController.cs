using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUIController: UIController
{
	void Start () {
		gameController = Camera.main.GetComponent<TutorialGameController>();
		//canvasInfo = GameObject.Find("Dialog");
		//DisableCanvas();
		/*
        RectTransform rt = canvasInfo.GetComponent(typeof(RectTransform)) as RectTransform;
        rt.sizeDelta =  new Vector2(200, 150); ;

        RectTransform rt2 = canvasInfo.GetComponentInChildren<Text>().GetComponent(typeof(RectTransform)) as RectTransform;
        rt2.sizeDelta = new Vector2(200, 150);*/
		//Si clica OK desactiva el canvas
		if (canvasInfo != null) {
			canvasInfo.GetComponentInChildren<Button> ().onClick.AddListener (DisableCanvas);
		}
		TileSprite = SpritesLoader.GetInstance().GetResource("Tiles/new_border");
		currentUIRenderer = new List<SpriteRenderer> ();
		round = GameObject.Find ("RoundNum");
		playerColor = GameObject.Find ("PlayerColor");
		actionsLeft = GameObject.Find ("ActionsNum");
		turnPanel = GameObject.Find ("TurnPanel");
		roundPanel = GameObject.Find ("RoundPanel");
		rectTransformT = turnPanel.GetComponent<RectTransform> ();
		rectTransformR = roundPanel.GetComponent<RectTransform> ();
		state = 0;
	}
}