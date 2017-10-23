using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour {
	private SlimeCore core;
	public  TileData actualTile;
	public Dictionary<TileData,List<TileData>> possibleMovements;
	public bool rangeUpdated;
	// Use this for initialization
	void Start () {
		rangeUpdated = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    //TODO modify when we have more attributes
    public override string ToString()
    {
        return "Insert some text here to describe the slime";
    }
	public void SetActualTile(TileData newTile){
		if(actualTile!=null)actualTile.SetSlimeOnTop(null);
		actualTile=newTile;
		actualTile.SetSlimeOnTop(gameObject);
	}
	public TileData GetActualTile(){
		return actualTile;
	}

	public void SetCore(SlimeCore core){
		this.core = core;
	}

	public int GetMovementRange(){
		if(core != null){
			return core.GetMovementRange();
		}
		return 0;
	}
}
