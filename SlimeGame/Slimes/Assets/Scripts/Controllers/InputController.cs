﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{

    GameController gameController;
    UIController uiController;
    public int xLimit;
    public int yLimit;
    public int minZoom;
    public int maxZoom;
    public int speed;

	List<Tile> attackTiles;
	List<Tile> moveTiles;
	List<Tile> splitTiles;

    void Start()
    {
        speed = 25;
        
        xLimit = (int) MapDrawer.MapSize().x;
        yLimit = (int) MapDrawer.MapSize().y;
        minZoom = 3;
        maxZoom = 13;
		gameController = Camera.main.GetComponent<GameController>();
        uiController = Camera.main.GetComponent<UIController>();
		moveTiles = new List<Tile> ();
		attackTiles = new List<Tile> ();
		splitTiles = new List<Tile> ();
    }

    void Update()
	{
		//Boto esquerra del mouse
		if (gameController.getStatus () == GameControllerStatus.WAITINGFORACTION) {
			if (Input.GetMouseButtonDown (0)) {
				//Obtinc els colliders que hi ha a la posicio del mouse
				Collider2D[] colliders = Physics2D.OverlapPointAll (Camera.main.ScreenToWorldPoint (Input.mousePosition));

				// Priority to action colliders
				string s = " ";
				foreach (Collider2D col in colliders) {
					if (col.gameObject.tag == "Slime" && col.gameObject.GetComponent<Slime> ().GetPlayer () == gameController.GetCurrentPlayer () && 
						gameController.GetSelectedSlime() != col.gameObject.GetComponent<Slime>()) {
						uiController.hideCurrentUITiles ();
						splitTiles = uiController.showSplitRange (col.gameObject.GetComponent<Slime> ());
						gameController.SetSelectedSlime (col.gameObject.GetComponent<Slime> ());
						s = col.gameObject.GetComponent<Slime>().ToString();
						break;
					/*}else if(col.gameObject.tag == "Slime") {
						//s = col.gameObject.GetComponent<Slime>().ToString();
*/ 
					} else if (col.gameObject.tag == "Slime" && col.gameObject.GetComponent<Slime> () == gameController.GetSelectedSlime ()) {
						gameController.ConquerTile (col.gameObject.GetComponent<Slime> ().GetActualTile());
						uiController.DisableCanvas ();
						uiController.hideCurrentUITiles ();
						gameController.SetSelectedSlime (null);
					} else if(col.gameObject.tag == "Tile") {
						Tile target = col.gameObject.GetComponent<Tile> ();
						bool isMoveTile = moveTiles.Contains (target);
						bool isAttackTile = attackTiles.Contains (target);
						if ((isMoveTile || isAttackTile) && gameController.GetSelectedSlime () != null) {
							if (isMoveTile) {
								Debug.Log (Time.time + "Move");
								gameController.MoveSlime (target);
								uiController.DisableCanvas ();
								uiController.hideCurrentUITiles ();
								gameController.SetSelectedSlime (null);
							} else if (isAttackTile) {
								Debug.Log (Time.time + "Attack");
								gameController.AttackSlime (target.GetSlimeOnTop ());
								uiController.DisableCanvas ();
								uiController.hideCurrentUITiles ();
								gameController.SetSelectedSlime (null);
							}
						} else {
							s = target.ToString();
						}
					}
				}
				if (!s.Equals (" ")) {
					uiController.ShowCanvasInfo (s);
				}
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
					if (s != null && s!=gameController.GetSelectedSlime()) {
						Debug.Log (Time.time+"Join");
						gameController.FusionSlime (s);
						uiController.DisableCanvas ();
						uiController.hideCurrentUITiles ();
						gameController.SetSelectedSlime (null);
					}else if(t!=null && splitTiles.Contains(t)){
						Debug.Log (Time.time+"Split");
						gameController.SlplitSlime (t);
						uiController.DisableCanvas ();
						uiController.hideCurrentUITiles ();
						gameController.SetSelectedSlime (null);
					}else{
						uiController.hideCurrentUITiles ();
						moveTiles = uiController.showMoveRange (gameController.GetSelectedSlime());
						attackTiles = uiController.showAttackRange (gameController.GetSelectedSlime());
					}
				}
			} else if (Input.GetMouseButtonDown (1)) {
				gameController.SetSelectedSlime (null);
				uiController.DisableCanvas ();
				uiController.hideCurrentUITiles ();
			} else if (Input.GetAxis ("Mouse ScrollWheel") > 0 && this.GetComponent<Camera> ().orthographicSize > minZoom) {
				this.GetComponent<Camera> ().orthographicSize--;
			} else if (Input.GetAxis ("Mouse ScrollWheel") < 0 && this.GetComponent<Camera> ().orthographicSize < maxZoom) {
				this.GetComponent<Camera> ().orthographicSize++;
			} else if (Input.GetKey (KeyCode.UpArrow)) {
				if (this.transform.position.y < yLimit) {
					this.transform.position += (new Vector3 (0, speed, 0) * Time.deltaTime);
				}
			} else if (Input.GetKey (KeyCode.DownArrow)) {
				if (this.transform.position.y > -yLimit) {
					this.transform.position -= (new Vector3 (0, speed, 0) * Time.deltaTime);
				}
			} else if (Input.GetKey (KeyCode.LeftArrow)) {
				if (this.transform.position.x > -xLimit) {
					this.transform.position -= (new Vector3 (speed, 0, 0) * Time.deltaTime);
				}
			} else if (Input.GetKey (KeyCode.RightArrow)) {
				if (this.transform.position.x < xLimit) {
					this.transform.position += (new Vector3 (speed, 0, 0) * Time.deltaTime);
				}
			}
		}
	}
		
}
