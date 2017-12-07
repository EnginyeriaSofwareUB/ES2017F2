using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour {
    private static int ID = 0;
	private int id;
	private Player player;
	public Tile actualTile;
	private float mass;
	private SpriteAnimation animation;
	private float maxMass = 300f;
	private float minMass = 20f;
	private float maxScale = 0.6f;
	private float minScale = 0.2f;
	private StatsContainer element;
	// Use this for initialization
	void Start () {
		id = ID;
		ID++;
		element = StatsFactory.GetStat (ElementType.NONE);
	}
	
	// Update is called once per frame
	void Update () {
		if(animation != null) animation.update ();
	}

	public void initSpriteAnimation(){
		animation = new SpriteAnimation (gameObject.GetComponent<SpriteRenderer>());
		animation.LoadSprites (player.statsCoreInfo.picDirection,5);
		animation.playAnimation ();

	}

	public void ManhattanDistance(Vector2 pos1, Vector2 pos2){

	}

    public override string ToString()
    {
        StatsContainer core = player.statsCoreInfo;
        string s = "";
        s += "Slime de "+ player.GetName() + "\n";
        s += "Vida/Masa: " + mass.ToString() + "\n";
        s += "Rango de ataque: " + core.range + "\n";
        s += "Rango de movimiento: " + core.move + "\n";
        s += "Fuerza de ataque: " + core.attack + "\n";
        s += "Coste de atacar: " + core.attackCost + "\n";
        if(element!= StatsFactory.GetStat(ElementType.NONE))
        {
            s += "Recubrimiento de  \n";
        }
        else
        {
            s += "Sin recubrimiento\n";
        }

        return s;
    }

	public int GetId(){
		return this.id;
	}
	public void SetActualTile(Tile newTile){
		if(actualTile!=null)actualTile.SetSlimeOnTop(null);
		actualTile=newTile;
		actualTile.SetSlimeOnTop(this);
	}
	public Tile GetActualTile(){
		return actualTile;
	}

	public TileData GetTileData(){
		return actualTile.GetTileData ();
	}

	public void setPlayer(Player player){
		this.player = player;
		SetMass(player.statsCoreInfo.startingHP);
		initSpriteAnimation ();
		gameObject.GetComponent<SpriteRenderer> ().color = player.GetColor ();
	}

	public Player GetPlayer(){
		return player;
	}

	public int GetMovementRange(){
		return player.statsCoreInfo.move + element.move;
	}

	public int GetAttackRange(){
		return player.statsCoreInfo.range + element.range;
	}

	public void changeMass(float q){
		SetMass (mass + q);
	}

    public bool CheckId(int id){
        return this.id == id;
    }

	public float getDamage(){
		return player.statsCoreInfo.attack + element.attack;
	}

	public bool isAlive(){
		return mass > 0.0f ? true : false;
	}

	public void SetMass(float mass){
		this.mass = mass;
		changeScaleSlime ();

	}

	public float GetMass(){
		return mass;
	}

	public void changeScaleSlime(){
		float scale;
		if (mass >= maxMass)
			scale = maxScale;
		else if (mass <= minMass)
			scale = minScale;
		else {
			scale = (maxScale-minScale)/(maxMass-minMass)*mass+minScale-(minMass*(maxScale-minScale))/(maxMass-minMass);
		}
		this.gameObject.transform.localScale = new Vector3(scale, scale, 0.5f);
	}

	public RawSlime GetRawCopy(){
		return new RawSlime(id, maxMass, minMass, mass, element, actualTile.GetTileData().GetRawCopy());
	}

}
