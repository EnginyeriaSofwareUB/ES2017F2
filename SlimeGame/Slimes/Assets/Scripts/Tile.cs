using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour,MapDrawer.MapCoordinates {
	public TileData data;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public Vector2 getPosition(){
		return data.hexPosition;
	}

    //TODO modify when we have more attributes
    public override string ToString()
    {
        return "Here will be info about tile when we got this";
        //TODO Em peta perque data me diu que es NULL
        //return data.ToString();
    }
}
