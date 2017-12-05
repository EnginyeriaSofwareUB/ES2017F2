using UnityEngine;
public class TileData:MapDrawer.MapCoordinates{
	private TileType type;
    private Vector2 hexPosition;
	public Slime slimeOnTop;
	public RawSlime rawSlimeOnTop;
	private Tile tile;
	private RawPlayer conquerer;

    //Tile tile;
	public TileData(TileType typeEnum, Vector2 position){
		hexPosition = position;
		type = typeEnum;
		slimeOnTop=null;
	}

    //Se tiene que modificar si se a�aden tipos de tiles
	override public string ToString(){
        if (type.ToString().Equals(TileType.Sand.ToString()))
            return "Casilla de tierra";
        else if (type.ToString().Equals(TileType.Water.ToString()))
            return "Casilla de agua";
        else if (type.ToString().Equals(TileType.Null.ToString()))
            return "Casilla de normal";
        return "Hay un nuevo tipo de casilla, modifica TileData";
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

	public void Conquer(RawPlayer pl){
		this.conquerer = pl;
	}

	public bool IsConquered(){
		return this.conquerer  != null;
	}

	public TileData GetRawCopy(){
		TileData copy = new TileData(type, new Vector2(hexPosition.x, hexPosition.y));
		return copy;
	}
}
