using System;
using UnityEngine;
using System.Collections.Generic;

public class TutorialInputController: InputController
{
	TutorialGameController tgc;

	void Start(){
		
		gameController = Camera.main.GetComponent<TutorialGameController>();
		tgc = (TutorialGameController)gameController;
		uiController = Camera.main.GetComponent<TutorialUIController>();
		cameraController = Camera.main.GetComponent<CameraController>();

		moveTiles = new List<Tile> ();
		attackTiles = new List<Tile> ();
		splitTiles = new List<Tile> ();
		joinTiles = new List<Tile> ();

	}

	protected override void AfterSelect ()
	{
		
		if (tgc.GetTutorialStatus () == TutorialGameController.TutorialFSMStatus.SELECTSLIME) {
			tgc.marker.SetActive (false);
		}

	}

	protected override void BeforeShowMove(){
		if (tgc.GetTutorialStatus () == TutorialGameController.TutorialFSMStatus.SELECTSLIME) {
			tgc.tutorialFSMCheck ();
		}
	}
}
