using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour {
	public TileData actualTile;
	public Dictionary<TileData,List<TileData>> possibleMovements;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    //TODO modify when we have more attributes
    public override string ToString()
    {
        return "Insert some text here to describe the slime";
    }
}
