using System.Collections;
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

    void Start()
    {
        speed = 1;
        
        xLimit = (int) MapDrawer.MapSize().x;
        yLimit = (int) MapDrawer.MapSize().y;
        minZoom = 3;
        maxZoom = 13;
		gameController = Camera.main.GetComponent<GameController>();
        uiController = Camera.main.GetComponent<UIController>();
    }

    void Update()
	{
		//Boto esquerra del mouse
		if (gameController.getStatus () == GameControllerStatus.WAITINGFORACTION) {
			if (Input.GetMouseButtonDown (0)) {
				//Obtinc els colliders que hi ha a la posicio del mouse
				Collider2D[] colliders = Physics2D.OverlapPointAll (Camera.main.ScreenToWorldPoint (Input.mousePosition));

				// Priority to action colliders
				foreach (Collider2D col in colliders) {
					if (col.gameObject.tag == "Slime" && col.gameObject.GetComponent<Slime> ().GetPlayer() == gameController.GetCurrentPlayer()) {
						uiController.hideCurrentUITiles ();
						uiController.showSplitRange (col.gameObject.GetComponent<Slime> ());
						gameController.SetSelectedSlime (col.gameObject.GetComponent<Slime> ());
					}
				}
			} else if (Input.GetMouseButtonUp (0)) {
				if (gameController.GetSelectedSlime () != null) {
					uiController.hideCurrentUITiles ();
					uiController.showMoveRange (gameController.GetSelectedSlime());
					uiController.showAttackRange (gameController.GetSelectedSlime());
				}
			} else if (Input.GetMouseButtonDown (1)) {
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
