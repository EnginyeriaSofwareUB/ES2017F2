using UnityEngine;
public class TileData:MapDrawer.MapCoordinates{
	private TileType type;
    private Vector2 hexPosition;
	public Slime slimeOnTop;
	public RawSlime rawSlimeOnTop;
	private Tile tile;
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
		return slimeOnTop!=null; //or someone on it
	}

	public TileType getTileType(){
		return type;
	}
	public void SetTileType(TileType type){
		this.type= type;
	}
	public void SetSlimeOnTop(Slime slimeTop){
		this.slimeOnTop=slimeTop;
	}
	public void SetSlimeOnTop(RawSlime slimeTop){
		this.rawSlimeOnTop=slimeTop;
	}
	public Slime GetSlimeOnTop(){
		return slimeOnTop;
	}
	public RawSlime GetRawSlimeOnTop(){
		if(rawSlimeOnTop == null && slimeOnTop == null) return null;
		else if(rawSlimeOnTop == null) return slimeOnTop.GetRawCopy();
		return rawSlimeOnTop;
	}
	public Vector2 GetRealWorldPosition(){
		return tile.gameObject.transform.position;
	}

	public void SetTile(Tile t){
		tile = t;
	}

	public Tile getTile(){
		return tile;
	}

	public TileData GetRawCopy(){
		TileData copy = new TileData(type, new Vector2(hexPosition.x, hexPosition.y));
		return copy;
	}
}
