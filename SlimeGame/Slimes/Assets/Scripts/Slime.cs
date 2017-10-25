using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour {
	private SlimeCore core;
	private Player player;
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

	public void ShowRanges(List<Player> players){
		MapDrawer.ShowMovementRange(possibleMovements);

		/*Sprite attackFilter = Resources.Load<Sprite>("Test/attackRangeFilter");
		Vector2 myPos = GetActualTile().getPosition();
		int attackRange = core.GetAttackRange();
		foreach(Player pl in players){
			if(pl != player){
				foreach(GameObject slGO in pl.GetSlimes()){
					Slime slime = slGO.GetComponent<Slime>();
					Vector2 slPos = slime.GetActualTile().getPosition();
					if(matrix.method <= attackRange){
						MapDrawer.MarkRanged(slime.GetActualTile(), attackFilter); // MANHATTAN
					}
				}
			}
		}*/
	}

	public void ManhattanDistance(Vector2 pos1, Vector2 pos2){

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

	public void setPlayer(Player player){
		this.player = player;
	}

	public int GetMovementRange(){
		if(core != null){
			return core.GetMovementRange();
		}
		return 0;
	}
}
