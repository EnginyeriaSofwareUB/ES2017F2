using UnityEngine;
public class TileData:MapDrawer.MapCoordinates{
	public TileType type;
    public Vector2 hexPosition;
    //Tile tile;
	public TileData(TileType typeEnum, Vector2 position){
		hexPosition = position;
		type = typeEnum;
	}

	override public string ToString(){
		return ((int)type).ToString()+" ("+hexPosition.x+","+hexPosition.y+")";
	}
	public Vector2 getPosition(){
		return hexPosition;
	}
}
