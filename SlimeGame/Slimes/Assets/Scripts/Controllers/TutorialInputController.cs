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

	protected override void OnMove(){
		if (tgc.GetTutorialStatus () == TutorialGameController.TutorialFSMStatus.MOVEFIRSTSLIME) {
			tgc.tutorialFSMCheck ();
		}else if (tgc.GetTutorialStatus () == TutorialGameController.TutorialFSMStatus.RETURNSLIME) {
			tgc.tutorialFSMCheck ();
		}else if (tgc.GetTutorialStatus () == TutorialGameController.TutorialFSMStatus.MOVESECONDARYSLIME) {
			tgc.tutorialFSMCheck ();
		}else if (tgc.GetTutorialStatus () == TutorialGameController.TutorialFSMStatus.MOVETOCONQUER) {
			tgc.tutorialFSMCheck ();
		}
	}

	protected override void OnSplit(){
		if (tgc.GetTutorialStatus () == TutorialGameController.TutorialFSMStatus.SPLITSLIME) {
			tgc.tutorialFSMCheck ();
		}
	}

	protected override void OnJoin(){
		if (tgc.GetTutorialStatus () == TutorialGameController.TutorialFSMStatus.JOINSLIME) {
			tgc.tutorialFSMCheck ();
		}
	}

	protected override void OnConquer ()
	{
		if (tgc.GetTutorialStatus () == TutorialGameController.TutorialFSMStatus.CONQUERTILE) {
			tgc.tutorialFSMCheck ();
		}
	}

	protected override void OnAttack ()
	{
		if (tgc.GetTutorialStatus () == TutorialGameController.TutorialFSMStatus.ATTACKSLIME) {
			tgc.tutorialFSMCheck ();
		} else if (tgc.GetTutorialStatus () == TutorialGameController.TutorialFSMStatus.ATTACKWITHFIRE) {
			tgc.tutorialFSMCheck ();
		}
	}
}
