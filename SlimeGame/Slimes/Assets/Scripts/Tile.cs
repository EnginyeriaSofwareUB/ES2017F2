using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
	private TileData data;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public Vector2 getPosition(){
		return data.getPosition();
	}

    //TODO modify when we have more attributes
    public override string ToString()
    {
        return "Here will be info about tile when we got this";
        //TODO Em peta perque data me diu que es NULL
        //return data.ToString();
    }
	public TileData GetTileData(){
		return data;
	}
	public void SetTileData(TileData data){
		this.data=data;
	}
	
}
