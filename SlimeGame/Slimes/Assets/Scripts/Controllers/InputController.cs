using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{

    protected GameController gameController;
	protected UIController uiController;
	protected CameraController cameraController;

	protected List<Tile> attackTiles;
	protected List<Tile> moveTiles;
	protected List<Tile> splitTiles;
	protected List<Tile> joinTiles;

	protected bool ConquerEnabled;
	protected bool MoveEnabled;
	protected bool EatEnabled;
	protected bool AttackEnabled;
	protected bool SplitEnabled;
	protected bool JoinEnabled;
	protected bool InputEnabled;
	
    void Start()
    {
		ConquerEnabled = true;
		MoveEnabled = true;
		EatEnabled = true;
		AttackEnabled = true;
		SplitEnabled = true;
		JoinEnabled = true;
		InputEnabled = true;

        gameController = Camera.main.GetComponent<GameController>();
        uiController = Camera.main.GetComponent<UIController>();
		cameraController = Camera.main.GetComponent<CameraController>();
		moveTiles = new List<Tile> ();
		attackTiles = new List<Tile> ();
		splitTiles = new List<Tile> ();
		joinTiles = new List<Tile> ();
    }

    void Update()
	{
		if (Application.isMobilePlatform) {
			ManageInput (
				Input.GetTouch(0).phase==TouchPhase.Began,
				Input.GetTouch(0).phase==TouchPhase.Stationary||Input.GetTouch(0).phase==TouchPhase.Moved,
				Input.GetTouch(0).phase==TouchPhase.Ended,
				Camera.main.ScreenToWorldPoint (Input.GetTouch(0).position)
			);
		} else {
			ManageInput (
				Input.GetMouseButtonDown(0),
				Input.GetMouseButton (0),
				Input.GetMouseButtonUp(0),
				Camera.main.ScreenToWorldPoint (Input.mousePosition)
			);
			CheckKeyBoardInputMovement ();
			CheckScrollInputMovement();
		}


	}

	private void ManageInput(bool inputStarted, bool inputMaintained, bool inputEnded,Vector3 position){
		bool selectedSlime = gameController.GetSelectedSlime()!=null;

		if (InputEnabled && gameController.getStatus () == GameControllerStatus.WAITINGFORACTION &&
			(inputStarted || inputMaintained || inputEnded)) {
			Collider2D[] colliders = Physics2D.OverlapPointAll (position);
			Slime s=null;
			Tile t=null;
			foreach (Collider2D col in colliders) {
				if (col.gameObject.tag == "Slime") {
					s = col.gameObject.GetComponent<Slime> ();
				} else if (col.gameObject.tag == "Tile") {
					t = col.gameObject.GetComponent<Tile> ();
				}
			}
			if (selectedSlime) {
				if (inputStarted) {
					//Selecciono al slime si es del player actual, sino, solo muestro la información
					if (s != null && s.GetPlayer () == gameController.GetCurrentPlayer () && s != gameController.GetSelectedSlime ()) {
						gameController.SetSelectedSlime (s);
						ClearMarkedTiles ();
						uiController.HideAndShowInfoPanel (s,t);
					} else if (s != null && s != gameController.GetSelectedSlime () && !attackTiles.Contains (t)) {
						uiController.HideAndShowInfoPanel (s, t);
						uiController.DisableGrowButton ();
						gameController.SetSelectedSlime (null);
						ClearMarkedTiles ();
					} else if (t != null) {
						if (moveTiles.Contains (t)) {
							gameController.DoAction (new SlimeAction (ActionType.MOVE, t));
							OnMove ();
							uiController.HideInfoPanel ();
						} else if (attackTiles.Contains (t)) {
							gameController.DoAction (new SlimeAction (ActionType.ATTACK, t.GetSlimeOnTop ()));
							uiController.HideInfoPanel ();
						} else if (gameController.GetSelectedSlime ().actualTile == t) {
							if (ConquerEnabled) {
								gameController.DoAction (new SlimeAction (ActionType.CONQUER, gameController.GetSelectedSlime ().actualTile));
								OnConquer ();
							}
							uiController.HideInfoPanel ();
						} else {
							uiController.HideAndShowInfoPanel (s, t);
							uiController.DisableGrowButton ();
						}
						gameController.SetSelectedSlime (null);
						ClearMarkedTiles ();
					} else {
						uiController.HideInfoPanel ();
						ClearMarkedTiles ();
						gameController.SetSelectedSlime (null);
					}
					//Acciones de slime manteniendo pulsado
				} else if (inputMaintained) {
					if (SplitEnabled && gameController.GetSelectedSlime ().canSplit) {
						splitTiles = gameController.GetSplitRangeTiles (gameController.GetSelectedSlime());
						uiController.markTiles (splitTiles, ActionType.SPLIT);
					}
					if (JoinEnabled) {
						joinTiles = gameController.GetJoinTile (gameController.GetSelectedSlime());
						uiController.markTiles (joinTiles, ActionType.FUSION);
					}
					//Acciones de slime al soltar el ratón
				} else if (inputEnded) {
					if (splitTiles.Contains (t)) {
						gameController.DoAction (new SlimeAction (ActionType.SPLIT, t));
						ClearMarkedTiles ();
						OnSplit ();
					} else if (joinTiles.Contains (t)) {
						gameController.DoAction (new SlimeAction (ActionType.FUSION, t.GetSlimeOnTop()));
						ClearMarkedTiles ();
						OnJoin ();
					} else {
						ClearMarkedTiles ();
						BeforeShowMove ();
						if (MoveEnabled) {
							moveTiles = gameController.GetPossibleMovements (gameController.GetSelectedSlime ());
							uiController.markTiles (moveTiles, ActionType.MOVE);
						}
						AfterShowMove ();
						BeforeShowAttack ();
						if (AttackEnabled && gameController.GetSelectedSlime ().canAttack) {
							attackTiles = gameController.GetSlimesInAttackRange (gameController.GetSelectedSlime ());
							uiController.markTiles (attackTiles, ActionType.ATTACK);
						}
						AfterShowAttack ();
						List<Tile> tiles = new List<Tile> ();
						//uiController.showSelectedSlime (gameController.GetSelectedSlime ());
						//uiController.UpdateInfo(gameController.GetSelectedSlime(),null);
						//uiController.HideAndShowInfoPanel (s,t);
					}
				}
			} else {
				if (inputStarted) {
					//Selecciono al slime si es del player actual, sino, solo muestro la información
					if (s != null && s.GetPlayer () == gameController.GetCurrentPlayer ()) {
						gameController.SetSelectedSlime (s);
						if (uiController.selected) {
							uiController.HideAndShowInfoPanel (s, t);
						} else {
							uiController.ShowInfoPanel (s,t);
						}
						uiController.EnableGrowButton ();
					} else if (s != null || t != null){
						if (uiController.selected) {
							uiController.HideAndShowInfoPanel (s, t);
						} else {
							uiController.ShowInfoPanel (s,t);
						}
						uiController.DisableGrowButton ();
					}
				} else if (inputMaintained) {
					CheckInputMovement ();
				} else if (inputEnded) {
					
				}
			}
		}
	}

	public void CheckInputMovement(){
		if (Application.isMobilePlatform) {
			if(Input.touchCount == 0 && gameController.GetSelectedSlime()){
				Vector2 touchDelta = Input.GetTouch(0).deltaPosition*Time.deltaTime;
				Camera.main.transform.Translate(-touchDelta.x, -touchDelta.y, 0f);
			} else if (Input.touchCount == 2) {
				// Store both touches.
				Touch touchZero = Input.GetTouch(0);
				Touch touchOne = Input.GetTouch(1);

				// Find the position in the previous frame of each touch.
				Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
				Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

				// Find the magnitude of the vector (the distance) between the touches in each frame.
				float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
				float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

				// Find the difference in the distances between each frame.
				float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

				if(deltaMagnitudeDiff>0.01f){
					cameraController.ZoomOut();
				}else if(deltaMagnitudeDiff<-0.01f){
					cameraController.ZoomIn();
				}
			}
		}else{
			
		}
	}

	public void CheckScrollInputMovement(){
		if (Input.GetAxis ("Mouse ScrollWheel") > 0) {
			cameraController.ZoomIn();
		} else if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
			cameraController.ZoomOut();
		}
	}


	public void CheckKeyBoardInputMovement(){
		if (Input.GetKey (KeyCode.UpArrow)) {
			cameraController.MoveUp();				
		} else if (Input.GetKey (KeyCode.DownArrow)) {
			cameraController.MoveDown();				
		} else if (Input.GetKey (KeyCode.LeftArrow)) {
			cameraController.MoveLeft();				
		} else if (Input.GetKey (KeyCode.RightArrow)) {
			cameraController.MoveRight();
		}
	}

	public void SetActiveMove(bool active){
		MoveEnabled = active;
	}

	public void SetActiveEat(bool active){
		EatEnabled = active;
	}

	public void SetActiveAttack(bool active){
		AttackEnabled = active;
	}

	public void SetActiveSplit(bool active){
		SplitEnabled = active;
	}

	public void SetActiveJoin(bool active){
		JoinEnabled = active;
	}

	public void SetActiveConquer(bool active){
		ConquerEnabled = active;
	}

	public void SetActiveInput(bool active){
		InputEnabled = active;
	}

	protected virtual void AfterSelect(){
	
	}

	protected virtual void AfterShowMove(){

	}

	protected virtual void AfterShowAttack(){

	}

	protected virtual void BeforeShowMove(){

	}

	protected virtual void BeforeShowAttack(){

	}

	protected virtual void OnMove(){
		
	}

	protected virtual void OnSplit(){
	
	}

	protected virtual void OnJoin(){
	
	}

	protected virtual void OnConquer(){
	
	}

	protected virtual void OnAttack(){
	
	}

	public void GrowSlime(){
		if (gameController.GetSelectedSlime () != null) {
			gameController.DoAction (new SlimeAction(ActionType.EAT,gameController.GetSelectedSlime()));
			gameController.SetSelectedSlime (null);
			uiController.hideCurrentUITiles();
			//uiController.DisableCanvas ();
			uiController.HideInfoPanel();
		}
	}

	public void ClearMarkedTiles(){
		uiController.hideCurrentUITiles ();
		moveTiles = new List<Tile> ();
		attackTiles = new List<Tile> ();
		splitTiles = new List<Tile> ();
		joinTiles = new List<Tile> ();
	}

}
