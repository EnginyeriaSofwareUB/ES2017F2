﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	private GameObject selectedItem;
	private Matrix matrix;
	// Use this for initialization
	void Start () {
		//MapDrawer.InitTest ();
		matrix = new Matrix(MapParser.ReadMap(MapTypes.Small));
		MapDrawer.instantiateMap(matrix.getIterable());
		instantiateSlime ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void instantiateSlime(){
		GameObject slime = new GameObject ("Slime");
		slime.AddComponent<SpriteRenderer> ();
		slime.tag = "Slime";
		slime.AddComponent<Slime> ();
		slime.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Test/slime");
		slime.GetComponent<SpriteRenderer> ().sortingOrder = 1;
		slime.AddComponent<BoxCollider2D> ();
	}
}
