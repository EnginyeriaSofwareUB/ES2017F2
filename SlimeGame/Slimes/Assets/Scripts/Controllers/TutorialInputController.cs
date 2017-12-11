using System;
using UnityEngine;
using System.Collections.Generic;

public class TutorialInputController: InputController
{
	
	void Start(){

		MoveEnabled = true;

		gameController = Camera.main.GetComponent<TutorialGameController>();
		uiController = Camera.main.GetComponent<TutorialUIController>();
		cameraController = Camera.main.GetComponent<CameraController>();

		moveTiles = new List<Tile> ();
		attackTiles = new List<Tile> ();
		splitTiles = new List<Tile> ();
		joinTiles = new List<Tile> ();

	}
}
