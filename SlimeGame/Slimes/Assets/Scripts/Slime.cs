using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour {
	private SlimeCore core;
	private Player player;
	public  TileData actualTile;
	public Dictionary<TileData,List<TileData>> possibleMovements;
	public bool rangeUpdated;
	private float massa;
	private GameObject gameObjectController;
	// Use this for initialization
	void Start () {
		rangeUpdated = false;
	}
	
	// Update is called once per frame
	void Update () {
		
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

	public Player GetPlayer(){
		return player;
	}

	public int GetMovementRange(){
		if(core != null){
			return core.GetMovementRange();
		}
		return 0;
	}

	public int GetAttackRange(){
		if(core != null){
			return core.GetAttackRange();
		}
		return 0;
	}

	//no posar aqui que actualitzi les barres de vida del joc, perque aqui es posa la massa per primer cop
	public void SetMassa(float m){
		massa = m;
	}

	public float GetMassa(){
		return massa;
	}

	//metode el qual ens resten vida, s'ha de tractar que passa quan s'acaba la vida
	//de moment esta posat aixi per tal de que si hi ha un canvi en la massa aquest repercuteixi a les barres de vida del joc
	/*public void Attack(float damage){
		massa = massa - damage;
		gameObjectController.GetComponent<GameController>().PrintHealthBars ();
	}*/

	public void SetGameObjectController(GameObject go){
		gameObjectController = go;
	}
}
