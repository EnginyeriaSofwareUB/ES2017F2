﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMovement : MonoBehaviour {

	private Vector2 startPos, endPos;
	private List<TileData> bufferPositions = null;
	private int indexBuffer;
	public bool moving;

	public float secondsXmovement = 0.4f; //1 segon, pero es pot determinar des de unity
	private float startTime;

	private GameController gameController;
	public GameObject parent;

	void Start(){
		/*Exemple de recorregut:
		List<Vector2> list = new List<Vector2> ();
		list.Add(new Vector2(4,4));
		list.Add(new Vector2(4,-4));
		list.Add(new Vector2(-4,-4));
		list.Add(new Vector2(-4,4));

		SetBufferAndPlay (list);*/

		moving = false;
		gameController = GameObject.Find ("Main Camera").GetComponent<GameController> ();
	}
	
	// Update is called once per frame

	void Update(){
		//si tenim recorregut a fer:
		gameObject.GetComponent<SpriteRenderer>().sortingOrder = (int) (1000-parent.transform.position.y*5+2);
		gameObject.GetComponent<Slime>().face.GetComponent<SpriteRenderer>().sortingOrder = (int) (1000-parent.transform.position.y*5+2)+1;

		if (bufferPositions != null && bufferPositions.Count>0) {
			float i = (Time.time - startTime) / secondsXmovement;
			parent.transform.position = Vector2.Lerp (startPos, endPos, i);

			if (i >= 1) {
				startTime = Time.time;
				indexBuffer++;
				//gameObject.GetComponent<Slime>().SetActualTile(bufferPositions [indexBuffer]);
				if (indexBuffer >= bufferPositions.Count) {
					bufferPositions = null;
					moving = false; //acaba el moviment
					gameController.updateStatus(GameControllerStatus.CHECKINGLOGIC);
				} else {
					startPos = endPos;
					endPos = bufferPositions [indexBuffer].GetRealWorldPosition();
					
				}
				
			}
		}
	}

	//funcio per determinar el recorregut que ha de ser Slime i iniciar recorregut
	public void SetBufferAndPlay(List<TileData> buffer){
		if (buffer != null && buffer.Count > 0) {
			bufferPositions = buffer;
			startPos = parent.transform.position;
			startTime = Time.time;
			indexBuffer = 0;
			endPos = bufferPositions [indexBuffer].GetRealWorldPosition();
			moving = true; //inici del moviment
			//gameObject.GetComponent<Slime>().SetActualTile(buffer [buffer.Count-1]);
		}
	}
}
