using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour {
	private int id;
	private Player player;
	public Tile actualTile;
	private float mass;
	private SpriteAnimation canimation;
	private float maxMass = 300f;
	private float minMass = 20f;
	private float maxScale = 0.6f;
	private float minScale = 0.2f;
	private ElementType elementType;
	private StatsContainer element;
	public GameObject face;
	// Use this for initialization
	void Start () {
		elementType = ElementType.NONE;
		ChangeElement(elementType);
	}
	
	// Update is called once per frame
	void Update () {
		if(canimation != null) canimation.update ();
	}

	public void initSpriteAnimation(){
		//canimation = new SpriteAnimation (gameObject.GetComponent<SpriteRenderer>());
		//canimation.LoadSprites (player.statsCoreInfo.picDirection,5);
		//canimation.playAnimation ();

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

	public void SetId(int id){
		this.id = id;
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
		if(element == null) Debug.Log("ELEMENT NULL");
		return player.statsCoreInfo.move + element.move;
	}

	public int GetAttackRange(){
		if(element == null) Debug.Log("ELEMENT NULL");
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

	public void ChangeElement(ElementType newElement){
		if (elementType == ElementType.NONE) {
			elementType = newElement;
			element = StatsFactory.GetStat (elementType);
			canimation = new SpriteAnimation (gameObject.GetComponent<SpriteRenderer> ());
			canimation.LoadSprites (element.picDirection, element.picCount);
			canimation.playAnimation ();
		}
	}

	public RawSlime GetRawCopy(){
		return new RawSlime(id, maxMass, minMass, mass, element, actualTile.GetTileData().GetRawCopy());
	}

}
