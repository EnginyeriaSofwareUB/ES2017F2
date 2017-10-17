using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour {
	public TileData actualTile;
	public Dictionary<TileData,List<TileData>> possibleMovements;
	public bool rangeUpdated;
	// Use this for initialization
	void Start () {
		rangeUpdated = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
