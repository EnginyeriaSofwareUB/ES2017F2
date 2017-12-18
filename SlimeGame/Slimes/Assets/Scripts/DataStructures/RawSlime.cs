using UnityEngine;

public class RawSlime{
    private static int ID = 0;
    private int id;
	private float mass;
	private ElementType elementType;
	private StatsContainer element;
	private RawPlayer player;
	private TileData actualTile;

    public RawSlime(int id, float mass, ElementType elementType, StatsContainer stats, TileData tile){
        this.mass = mass;
        this.element = stats;
        this.elementType = elementType;
        this.actualTile = tile;
        actualTile.SetSlimeOnTop(this);
        this.id = id;
        if(id > ID) ID = id+1; // ID sempre es el maxim de tots els id de RawSlime

        if (stats == null) {
			element = StatsFactory.GetStat (ElementType.NONE);
            elementType = ElementType.NONE;
        }
    }

    public RawSlime(float mass, ElementType elementType, StatsContainer stats, TileData tile){
        this.mass = mass;
        this.element = stats;
        this.elementType = elementType;
        this.actualTile = tile;
        actualTile.SetSlimeOnTop(this);
        this.id = ID;
        ID++;

        if (stats == null) {
			element = StatsFactory.GetStat (ElementType.NONE);
            elementType = ElementType.NONE;
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

    /*public int getDamage(){
		StatsContainer core = player.statsCoreInfo;
		float currentRatio = (((float)(mass - core.minCalcMass)) / (core.maxCalcMass - core.minCalcMass));
		int baseDamage = (int) (currentRatio * (core.maxBaseAttack - core.minBaseAttack) + core.minBaseAttack);
		float scalingDamage = currentRatio * (core.maxAttackDrain - core.minAttackDrain) + core.minAttackDrain;
		float scalingRatio = currentRatio * (core.maxAttackMultiplier - core.minAttackMultiplier) + core.minAttackMultiplier;
		int finalDamage = (int) (scalingRatio * (baseDamage + scalingDamage * mass));
		return 10;
	}*/

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
        RawSlime newSlime =  new RawSlime(newmass, elementType, element, toSplit);
        newSlime.SetPlayer(player);
        player.AddSlime(newSlime);
        return newSlime;
    }

    public float GetDamageReduction(){
		StatsContainer core = player.statsCoreInfo;
		float currentRatio = massRatio;
		float damageReduction = (currentRatio * (maxDamageReduction - minDamageReduction) + minDamageReduction);
		return (1-damageReduction);
	}

	public void GrowSlime(){
		StatsContainer core = player.statsCoreInfo;
		mass += (int)(mass * (scalingGrowth) + plainGrowth);
		if (mass > maxMass) {
			mass = maxMass;
		}
	}

    	public void ChangeElement(ElementType newElement){
		if (elementType == ElementType.NONE) {
			elementType = newElement;
			element = StatsFactory.GetStat (elementType);
		}
		if (elementType == ElementType.FIRE ||
		   elementType == ElementType.WATER ||
		   elementType == ElementType.EARTH) {
			switch (newElement) {
			case ElementType.EARTH:
				if (elementType == ElementType.FIRE) {
					elementType = ElementType.LAVA;
				} else if (elementType == ElementType.WATER) {
					elementType = ElementType.MUD;
				}
				break;
			case ElementType.FIRE:
				if (elementType == ElementType.EARTH) {
					elementType = ElementType.LAVA;
				} else if (elementType == ElementType.WATER) {
					elementType = ElementType.STEAM;
				}
				break;
			case ElementType.WATER:
				if (elementType == ElementType.FIRE) {
					elementType = ElementType.STEAM;
				} else if (elementType == ElementType.EARTH) {
					elementType = ElementType.MUD;
				}
				break;
			default:
				break;
			}
			element = StatsFactory.GetStat (elementType);
		}
	
	}

    public override string ToString(){
        return "RAWSLIME - Mass: " + mass + ", Tile: " + actualTile.getPosition().x + ":" + actualTile.getPosition().y;
    }

    public RawSlime GetCopy(){
        return new RawSlime(id, mass, elementType, element, actualTile);
    }



    //Damage calculation methods
	public float massRatio{
		get{
			float ratio = ((float)(mass - (minCalcMass)) / ((maxCalcMass) - (minCalcMass)));
			if (ratio > 1f) {
				ratio = 1f;
			}else if (ratio < 0f) {
				ratio = 0f;
			}
			return ratio;
		}
	}

	public float scalingGrowth{
		get{ 
			return player.statsCoreInfo.scalingMassGain + element.scalingMassGain;
		}
	}

	public float plainGrowth{
		get{ 
			return player.statsCoreInfo.plainMassGain + element.plainMassGain;
		}
	}

	public int maxMass{
		get{ 
			return player.statsCoreInfo.maxMass + element.maxMass;
		}
	}

	private float minCalcMass{
		get{ 
			return player.statsCoreInfo.minCalcMass + element.minCalcMass;
		}
	}

	private float maxCalcMass{
		get{ 
			return player.statsCoreInfo.maxCalcMass + element.maxCalcMass;
		}
	}

	private float minDamageReduction{
		get{ 
			return player.statsCoreInfo.minDamageReduction + element.minDamageReduction;
		}
	}

	private float maxDamageReduction{
		get{ 
			return player.statsCoreInfo.maxDamageReduction + element.maxDamageReduction;
		}
	}

	private float maxBaseAttack{
		get{ 
			return player.statsCoreInfo.maxBaseAttack + element.maxBaseAttack;
		}	
	}

	private float minBaseAttack{
		get{ 
			return player.statsCoreInfo.minBaseAttack + element.minBaseAttack;
		}
	}

	private float maxAttackDrain{
		get{ 
			return player.statsCoreInfo.maxAttackDrain + element.maxAttackDrain;
		}
	}

	private float minAttackDrain{
		get{ 
			return player.statsCoreInfo.minAttackDrain + element.minAttackDrain;
		}
	}

	private int movement{
		get{ 
			if(player.statsCoreInfo.movement + element.movement > 0){
				return player.statsCoreInfo.movement + element.movement;
			}else{
				return 1;
			}
		}
	}

	private int range{
		get{
			if(player.statsCoreInfo.range + element.range > 0){
				return player.statsCoreInfo.range + element.range;
			}else{
				return 1;
			}
		}
	}

	private float maxAttackMultiplier{
		get{
			return player.statsCoreInfo.maxAttackMultiplier + element.maxAttackMultiplier;
		}
	}

	private float minAttackMultiplier{
		get{
			return player.statsCoreInfo.minAttackMultiplier + element.minAttackMultiplier;
		}
	}

	public int getDamage {
		get {
			float currentRatio = massRatio;
			int baseDamage = (int)(currentRatio * (maxBaseAttack - minBaseAttack) + minBaseAttack);
			float scalingDamageRatio = currentRatio * (maxAttackDrain - minAttackDrain) + minAttackDrain;
			float scalingRatio = currentRatio * (maxAttackMultiplier - minAttackMultiplier) + minAttackMultiplier;
			int finalDamage = (int)(scalingRatio * (baseDamage + scalingDamageRatio * mass));
			//Debug.Log ("Mass: " + mass + "\nBase damage: " + baseDamage + "\nScaling damage: " + scalingDamageRatio + "\nScaling ratio: " + scalingRatio + "\nFinal damage: " + finalDamage);
			return finalDamage;
		}
	}

	public float attackDrain{
		get {
			return massRatio*(maxAttackDrain - minAttackDrain) + minAttackDrain;
		}
	}

	public bool canSplit{
		get{ 
			return mass > 30;
		}
	}

	public bool canAttack{
		get{ 
			return mass > 15;
		}
	}

	public bool canGrow{
		get{ 
			return mass < maxMass;
		}
	}
}