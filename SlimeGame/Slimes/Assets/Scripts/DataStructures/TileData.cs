using UnityEngine;
public class TileData {
	public TileType type;
    public Vector2 hexPosition;
    //Tile tile;
	public TileData(TileType typeEnum, Vector2 position){
		hexPosition = position;
		type = typeEnum;
	}

	public string ToString(){
		return ((int)type).ToString()+" ("+hexPosition.x+","+hexPosition.y+")";
	}
}
