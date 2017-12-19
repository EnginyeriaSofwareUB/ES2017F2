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
		health = GameObject.Find ("Health");
		healthT = GameObject.Find ("HealthT");
		range = GameObject.Find ("Range");
		rangeT = GameObject.Find ("RangeT");
		movement = GameObject.Find ("Movement");
		movementT = GameObject.Find ("MovementT");
		attack = GameObject.Find ("Attack");
		attackT = GameObject.Find ("AttackT");
		defense = GameObject.Find ("Defense");
		defenseT = GameObject.Find ("DefenseT");
		turnPanel = GameObject.Find ("TurnPanel");
		roundPanel = GameObject.Find ("RoundPanel");
		infoPanel = GameObject.Find ("InfoPanel");
		rectTransformT = turnPanel.GetComponent<RectTransform> ();
		rectTransformR = roundPanel.GetComponent<RectTransform> ();
		rectTransformI = infoPanel.GetComponent<RectTransform> ();
		growButton = GameObject.Find ("GrowButton");
		state = 0;
		selected = false;
	}
}