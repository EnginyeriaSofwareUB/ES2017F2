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
		//Boto esquerra del mouse
		if (InputEnabled && gameController.getStatus () == GameControllerStatus.WAITINGFORACTION) {
			if (Input.GetMouseButtonDown (0)) {
				//Obtinc els colliders que hi ha a la posicio del mouse
				Collider2D[] colliders = Physics2D.OverlapPointAll (Camera.main.ScreenToWorldPoint (Input.mousePosition));

				// Priority to action colliders
				string s = " ";
				foreach (Collider2D col in colliders) {
					if (col.gameObject.tag == "Slime" && col.gameObject.GetComponent<Slime> ().GetPlayer () == gameController.GetCurrentPlayer () && 
						gameController.GetSelectedSlime() != col.gameObject.GetComponent<Slime>()) {
						uiController.hideCurrentUITiles ();
						if (SplitEnabled) {
							splitTiles = gameController.GetSplitRangeTiles (col.gameObject.GetComponent<Slime> ());
							uiController.markTiles (splitTiles,ActionType.SPLIT);
						}
						if (JoinEnabled) {
							joinTiles = gameController.GetJoinTile (col.gameObject.GetComponent<Slime> ());
							uiController.markTiles (joinTiles,ActionType.FUSION);
						}
						gameController.SetSelectedSlime (col.gameObject.GetComponent<Slime> ());
						s = col.gameObject.GetComponent<Slime>().ToString();
						AfterSelect();
						break;
					/*}else if(col.gameObject.tag == "Slime") {
						//s = col.gameObject.GetComponent<Slime>().ToString();
*/ 
					} else if (col.gameObject.tag == "Slime" && col.gameObject.GetComponent<Slime> () == gameController.GetSelectedSlime ()) {
						if(ConquerEnabled){
							gameController.DoAction(new SlimeAction(ActionType.CONQUER,col.gameObject.GetComponent<Slime> ().GetActualTile()));
						}//uiController.DisableCanvas ();
						uiController.hideCurrentUITiles ();
						gameController.SetSelectedSlime (null);
						break;
					} else if (col.gameObject.tag == "Slime" && col.gameObject.GetComponent<Slime>().GetPlayer()!=gameController.GetCurrentPlayer()
						&& gameController.GetSelectedSlime()==null){
						s = col.gameObject.GetComponent<Slime>().ToString();
						break;
					} else if(col.gameObject.tag == "Tile") {
						Tile target = col.gameObject.GetComponent<Tile> ();
						bool isMoveTile = moveTiles.Contains (target);
						bool isAttackTile = attackTiles.Contains (target);
						if ((isMoveTile || isAttackTile) && gameController.GetSelectedSlime () != null) {
							if (isMoveTile) {
								//Debug.Log (Time.time + "Move");
								gameController.DoAction(new SlimeAction(ActionType.MOVE,target));
								//uiController.DisableCanvas ();
								uiController.hideCurrentUITiles ();
								gameController.SetSelectedSlime (null);
							} else if (isAttackTile) {
								//Debug.Log (Time.time + "Attack");
								gameController.DoAction(new SlimeAction(ActionType.ATTACK,target.GetSlimeOnTop ()));
								//uiController.DisableCanvas ();
								uiController.hideCurrentUITiles ();
								gameController.SetSelectedSlime (null);
							}
						} else {
                            //Quan selecciona una casella, sense seleccionar cap slime abans
							s = target.ToString();
						}
					}
				}
				/*if (!s.Equals (" ")) {
					uiController.ShowCanvasInfo (s);
				}*/
			} else if (Input.GetMouseButtonUp (0) && gameController.GetSelectedSlime()!=null) {
				if (gameController.GetSelectedSlime () != null) {
					Collider2D[] colliders = Physics2D.OverlapPointAll (Camera.main.ScreenToWorldPoint (Input.mousePosition));
					Tile t = null;
					Slime s = null;
					foreach (Collider2D c in colliders) {
						if (c.gameObject.tag == "Tile") {
							t = c.gameObject.GetComponent<Tile> ();
						} else if (c.gameObject.tag == "Slime" && c.gameObject.GetComponent<Slime> ().GetPlayer () == gameController.GetCurrentPlayer ()) {
							s = c.gameObject.GetComponent<Slime> ();
						}
					}
					if (s != null && s!=gameController.GetSelectedSlime() && joinTiles.Contains(s.actualTile)) {
						//Debug.Log (Time.time+"Join");
						gameController.DoAction(new SlimeAction(ActionType.FUSION,s));
						//uiController.DisableCanvas ();
						uiController.hideCurrentUITiles ();
						gameController.SetSelectedSlime (null);
					}else if(t!=null && splitTiles.Contains(t)){
						//Debug.Log (Time.time+"Split");
						gameController.DoAction(new SlimeAction(ActionType.SPLIT,t));
						//uiController.DisableCanvas ();
						uiController.hideCurrentUITiles ();
						gameController.SetSelectedSlime (null);
					}else{
						uiController.hideCurrentUITiles ();
						BeforeShowMove ();
						if (MoveEnabled) {
							moveTiles = gameController.GetPossibleMovements (gameController.GetSelectedSlime ());
							uiController.markTiles (moveTiles, ActionType.MOVE);
						}
						AfterShowMove ();
						BeforeShowAttack ();
						if (AttackEnabled) {
							attackTiles = gameController.GetSlimesInAttackRange (gameController.GetSelectedSlime());
							uiController.markTiles (attackTiles,ActionType.ATTACK);
						}
						AfterShowAttack ();
						List<Tile> tiles = new List<Tile>();
						uiController.showSelectedSlime (gameController.GetSelectedSlime ());
						tiles.AddRange(moveTiles);
						tiles.AddRange(attackTiles);
						cameraController.AllTilesInCamera(gameController.GetSelectedSlime().actualTile,tiles);
					}
				}
			} else if (Input.GetMouseButtonDown (1)) {
				gameController.SetSelectedSlime (null);
//				uiController.DisableCanvas ();
				uiController.hideCurrentUITiles ();
				cameraController.GlobalCamera();
			} else if (Input.GetAxis ("Mouse ScrollWheel") > 0) {
				cameraController.ZoomIn();
			} else if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
				cameraController.ZoomOut();
			} else if (Input.GetKey (KeyCode.UpArrow)) {
				cameraController.MoveUp();				
			} else if (Input.GetKey (KeyCode.DownArrow)) {
				cameraController.MoveDown();				
			} else if (Input.GetKey (KeyCode.LeftArrow)) {
				cameraController.MoveLeft();				
			} else if (Input.GetKey (KeyCode.RightArrow)) {
				cameraController.MoveRight();
			}
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
}
