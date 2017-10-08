﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {
	public TileTypeEnum type;

	public Tile(TileTypeEnum typeEnum){
		
		type = typeEnum;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public string ToString(){
		return ((int)type).ToString();
	}
}
