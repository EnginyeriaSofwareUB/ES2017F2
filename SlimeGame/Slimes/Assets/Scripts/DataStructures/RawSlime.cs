public class RawSlime{
	private float MAX_MASS;
	private float MIN_MASS;
	private float mass;
	private StatsContainer element;
	private RawPlayer player;
	private TileData actualTile;

    public RawSlime(float maxmass, float minmas, float mass, StatsContainer stats, TileData tile){
        this.MAX_MASS = maxmass;
        this.MIN_MASS = minmas;
        this.mass = mass;
        this.element = stats;
        this.actualTile = tile;
        actualTile.SetSlimeOnTop(this);
    }

    public void SetPlayer(RawPlayer player){
        this.player = player;
    }

    public void SetTile(TileData tile){
        this.actualTile = tile;
        tile.SetSlimeOnTop(this);
    }

    public TileData GetTileData(){
        return actualTile;
    }

    public override string ToString(){
        return "RAWSLIME - Mass: " + mass + ", Tile: " + actualTile.getPosition().x + ":" + actualTile.getPosition().y;
    }
}