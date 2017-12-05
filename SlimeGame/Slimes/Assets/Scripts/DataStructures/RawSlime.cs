public class RawSlime{
    private static int ID = 0;
    private int id;
	private float MAX_MASS;
	private float MIN_MASS;
	private float mass;
	private StatsContainer element;
	private RawPlayer player;
	private TileData actualTile;

    public RawSlime(int id, float maxmass, float minmas, float mass, StatsContainer stats, TileData tile){
        this.MAX_MASS = maxmass;
        this.MIN_MASS = minmas;
        this.mass = mass;
        this.element = stats;
        this.actualTile = tile;
        actualTile.SetSlimeOnTop(this);
        this.id = id;
        if(id > ID) ID = id+1; // ID sempre es el maxim de tots els id de RawSlime
    }

    public RawSlime(float maxmass, float minmas, float mass, StatsContainer stats, TileData tile){
        this.MAX_MASS = maxmass;
        this.MIN_MASS = minmas;
        this.mass = mass;
        this.element = stats;
        this.actualTile = tile;
        actualTile.SetSlimeOnTop(this);
        this.id = ID;
        ID++;
    }

    public float getDamage(){
		return player.statsCoreInfo.attack + element.attack;
	}

    public bool isAlive(){
		return mass > 0.0f;
	}

    public float changeMass(float q){
		mass += q;
        return mass;
	}

    public void SetPlayer(RawPlayer player){
        this.player = player;
    }

    public RawPlayer GetPlayer(){
		return player;
	}

    public void SetTile(TileData tile){
        this.actualTile = tile;
        tile.SetSlimeOnTop(this);
    }

    public TileData GetTileData(){
        return actualTile;
    }
    
    public float GetMass(){
        return mass;
    }

    public void SetMass(float newmass){
        mass += newmass;
    }

    public int GetId(){
        return this.id;
    }

    public bool CheckId(int id){
        return this.id == id;
    }

    public RawSlime Split(TileData toSplit){
        mass = (mass/2.0f);
        RawSlime newSlime =  new RawSlime(MAX_MASS, MIN_MASS, mass, element, toSplit);
        player.AddSlime(newSlime);
        return newSlime;
    }

    public override string ToString(){
        return "RAWSLIME - Mass: " + mass + ", Tile: " + actualTile.getPosition().x + ":" + actualTile.getPosition().y;
    }

    public RawSlime GetCopy(){
        return new RawSlime(id, MAX_MASS, MIN_MASS, mass, element, actualTile);
    }
}