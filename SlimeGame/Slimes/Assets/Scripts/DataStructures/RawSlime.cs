using UnityEngine;

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

        if (stats == null) {
			element = StatsFactory.GetStat (ElementType.NONE);
        }
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

        if (stats == null) {
			element = StatsFactory.GetStat (ElementType.NONE);
        }
    }

	public int GetMovementRange(){
		return player.statsCoreInfo.movement + element.movement;
	}

    public int GetAttackRange(){
        if(player == null) Debug.Log("PLAYER NULL");
        else if(player.statsCoreInfo == null) Debug.Log("PLAYER NULL");
        else if(element == null) Debug.Log("ELEMENT NULL");
		return player.statsCoreInfo.range + element.range;
	}

    public TileData GetActualTile(){
		return actualTile;
	}

    public int getDamage(){
		StatsContainer core = player.statsCoreInfo;
		float currentRatio = (((float)(mass - core.minCalcMass)) / (core.maxCalcMass - core.minCalcMass));
		int baseDamage = (int) (currentRatio * (core.maxBaseAttack - core.minBaseAttack) + core.minBaseAttack);
		float scalingDamage = currentRatio * (core.maxAttackDrain - core.minAttackDrain) + core.minAttackDrain;
		float scalingRatio = currentRatio * (core.maxAttackMultiplier - core.minAttackMultiplier) + core.minAttackMultiplier;
		int finalDamage = (int) (scalingRatio * (baseDamage + scalingDamage * mass));
		return 10;
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
        mass = newmass;
    }

    public int GetId(){
        return this.id;
    }

    public bool CheckId(int id){
        return this.id == id;
    }

    public RawSlime Split(TileData toSplit){
        float newmass = (mass/2.0f);
        this.mass = newmass;
        RawSlime newSlime =  new RawSlime(MAX_MASS, MIN_MASS, newmass, element, toSplit);
        newSlime.SetPlayer(player);
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