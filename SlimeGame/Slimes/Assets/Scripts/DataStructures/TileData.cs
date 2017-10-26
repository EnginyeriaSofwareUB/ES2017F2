using UnityEngine;
public class TileData:MapDrawer.MapCoordinates{
	private TileType type;
    private Vector2 hexPosition;
	private Vector2 realWorldPosition;
	public GameObject slimeOnTop;
    //Tile tile;
	public TileData(TileType typeEnum, Vector2 position){
		hexPosition = position;
		type = typeEnum;
		slimeOnTop=null;
	}

	override public string ToString(){
		return ((int)type).ToString()+" ("+hexPosition.x+","+hexPosition.y+")";
	}
	public Vector2 getPosition(){
		return hexPosition;
	}
	public bool isBlocking(){
		return type==TileType.Block||slimeOnTop!=null; //or someone on it
	}

	public TileType getTileType(){
		return type;
	}
	public void SetSlimeOnTop(GameObject slimeTop){
		this.slimeOnTop=slimeTop;
	}
	public Slime GetSlimeOnTop(){
		if(slimeOnTop != null) return slimeOnTop.GetComponent<Slime>();
		return null;
	}
	public void SetRealWorldPosition(Vector2 vec){
		realWorldPosition=vec;
	}
	public Vector2 GetRealWorldPosition(){
		return realWorldPosition;
	}
}
