using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour {
	private int id;
	private Player player;
	public Tile actualTile;
	private int mass;
	private SpriteAnimation canimation;
	private float maxScale = 0.6f;
	private float minScale = 0.25f;
	private ElementType elementType;
	private StatsContainer element;
	public GameObject face;
	private GameObjectAnimationController controller;
	// Use this for initialization
	void Start () {
		ChangeElement(elementType);
	}

	public void InitElementTypeNone(){
		elementType = ElementType.NONE;
		element = StatsFactory.GetStat (elementType);
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
        s += "Vida/Masa: " + mass + "\n";
        s += "Rango de ataque: " + range + "\n";
        s += "Rango de movimiento: " + movement + "\n";
		s += "Fuerza de ataque: " + getDamage + "\n";
		s += "Coste de atacar: " + (int)(mass * attackDrain) + "\n";
        s += GetElement() + "\n";
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
		SetMass(player.statsCoreInfo.baseMass,false);
		initSpriteAnimation ();
		gameObject.GetComponent<SpriteRenderer> ().color = player.GetColor ();
	}

	public Player GetPlayer(){
		return player;
	}

	public int GetMovementRange(){
		return movement;
	}

	public int GetAttackRange(){
		return range;
	}

	public void ChangeMass(float q){
		SetMass ((int)(mass + q),true);
	}

	public void ChangeMass(int q){
		SetMass ((int)(mass + q),true);
	}

    public bool CheckId(int id){
        return this.id == id;
    }

	public float GetDamageReduction(){
		StatsContainer core = player.statsCoreInfo;
		float currentRatio = massRatio;
		float damageReduction = (currentRatio * (maxDamageReduction - minDamageReduction) + minDamageReduction);
		return (1-damageReduction);
	}

	public void GrowSlime(){
		StatsContainer core = player.statsCoreInfo;
		int lastMass = mass;
		mass += (int)(mass * (scalingGrowth) + plainGrowth);
		if (mass > maxMass) {
			mass = maxMass;
		}
		FloatingTextController.CreateFloatingText (printInteger((int)(this.mass-lastMass)),this.transform);
		changeScaleSlime ();
	}

	public bool isAlive(){
		return mass > 0.0f ? true : false;
	}

	public void SetMass(int mass,bool popup){
		if(popup) FloatingTextController.CreateFloatingText (printInteger((int)(mass-this.mass)),this.transform);
		this.mass = mass;
		changeScaleSlime ();
	}

	private string printInteger(int number){
		if (number>=0) return "+"+number.ToString();
		else return number.ToString();
	}

	public float GetMass(){
		return mass;
	}

	//Funcio per determinar la funcio dels nous slimes splitejats, i despres s'actualitza a gamecontroller
	public void InitMass(){
		mass = 0;
	}

	public void changeScaleSlime(){
		float scale;
		StatsContainer core = player.statsCoreInfo;
		scale = massRatio*(maxScale-minScale)+minScale;
		this.gameObject.transform.localScale = new Vector3(scale, scale, 0.5f);
	}

	public ElementType GetElementType(){
		return elementType;
	}

	public void ChangeElement(ElementType newElement){
		if (elementType == ElementType.NONE) {
			elementType = newElement;
			element = StatsFactory.GetStat (elementType);
			canimation = new SpriteAnimation (gameObject.GetComponent<SpriteRenderer> ());
			canimation.LoadSprites (element.picDirection, element.picCount);
			canimation.SetMode (SpriteAnimationMode.BOUNCE);
			canimation.playAnimation ();
			changeScaleSlime ();
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
			canimation = new SpriteAnimation (gameObject.GetComponent<SpriteRenderer> ());
			canimation.LoadSprites (element.picDirection, element.picCount);
			canimation.playAnimation ();
			canimation.SetMode (SpriteAnimationMode.LOOP);
			changeScaleSlime ();
		}

		CenterFace ();
	
	}

	private void CenterFace(){
		if (controller != null) {
			Destroy (controller);
		}
		//controller = face.AddComponent<GameObjectAnimationController> ();
		//controller.initLists ();
		switch (elementType) {
		case ElementType.EARTH:
			face.transform.localPosition = new Vector3 (0.85f,-0.2f,0f);
			face.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
			/*
			controller.AddTransformTransition (new Vector3(face.transform.localPosition.x, 0f, 0f), 0f, 0f);
			controller.AddTransformTransition (new Vector3(face.transform.localPosition.x, 0f, 0f), 0.5f, 0f);
			controller.AddTransformTransition (face.transform.localPosition, 0.0f, 0.0f);
			*/
			break;
		case ElementType.FIRE:
			face.transform.localPosition = new Vector3 (0.05f,-0.5f,0f);
			face.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
			/*
			controller.AddTransformTransition (new Vector3(face.transform.localPosition.x, 0f, 0f), 0f, 0f);
			controller.AddTransformTransition (new Vector3(face.transform.localPosition.x, 0f, 0f), 0.5f, 0f);
			controller.AddTransformTransition (face.transform.localPosition, 0.0f, 0.0f);
			*/
			break;
		case ElementType.LAVA:
			face.transform.localPosition = new Vector3 (0.5f, 0.45f, 0f);
			face.transform.localScale = new Vector3 (1f, 1f, 1f);
			/*
			controller.AddTransformTransition (new Vector3(face.transform.localPosition.x, 0f, 0f), 0f, 0f);
			controller.AddTransformTransition (new Vector3(face.transform.localPosition.x, 0f, 0f), 0.5f, 0f);
			controller.AddTransformTransition (face.transform.localPosition, 0.0f, 0.0f);
			*/
			break;
		case ElementType.MUD:
			face.transform.localPosition = new Vector3 (0.15f,0.5f,0f);
			face.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
			/*
			controller.AddTransformTransition (new Vector3(face.transform.localPosition.x, 0f, 0f), 0f, 0f);
			controller.AddTransformTransition (new Vector3(face.transform.localPosition.x, 0f, 0f), 0.5f, 0f);
			controller.AddTransformTransition (face.transform.localPosition, 0.0f, 0.0f);
			*/
			break;
		case ElementType.NONE:
			face.transform.localPosition = new Vector3 (0.2f,-0.15f,0f);
			face.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
			/*
			controller.AddTransformTransition (new Vector3(face.transform.localPosition.x, 0f, 0f), 0f, 0f);
			controller.AddTransformTransition (new Vector3(face.transform.localPosition.x, 0f, 0f), 0.38f, 0f);
			controller.AddTransformTransition (face.transform.localPosition, 0.0f, 0.0f);
			*/
			break;
		case ElementType.STEAM:
			face.transform.localPosition = new Vector3 (0.05f,0.3f,0f);
			face.transform.localScale = new Vector3(0.65f, 0.65f, 0.65f);
			/*
			controller.AddTransformTransition (new Vector3(face.transform.localPosition.x, 0f, 0f), 0f, 0f);
			controller.AddTransformTransition (new Vector3(face.transform.localPosition.x, 0f, 0f), 0.5f, 0f);
			controller.AddTransformTransition (face.transform.localPosition, 0.0f, 0.0f);
			*/
			break;
		case ElementType.WATER:
			face.transform.localPosition = new Vector3 (0.25f,0f,0f);
			face.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
			/*
			controller.AddTransformTransition (new Vector3(face.transform.localPosition.x, 0f, 0f), 0f, 0f);
			controller.AddTransformTransition (new Vector3(face.transform.localPosition.x, 0f, 0f), 0.5f, 0f);
			controller.AddTransformTransition (face.transform.localPosition, 0.0f, 0.0f);
			*/
			break;
		}
		//controller.StartAnimation ();
	}

	public RawSlime GetRawCopy(){
		return new RawSlime(id, mass, elementType, element, actualTile.GetTileData().GetRawCopy());
	}

    public string GetElement()
    {
        if (element == StatsFactory.GetStat(ElementType.NONE))
        {
            return "Sin recubrimiento";
        }
        else if(element == StatsFactory.GetStat(ElementType.EARTH))
        {
           return "Recubrimiento de tierra";
        }
        else if (element == StatsFactory.GetStat(ElementType.FIRE))
        {
            return "Recubrimiento de fuego";
        }
        else if (element == StatsFactory.GetStat(ElementType.WATER))
        {
            return "Recubrimiento de agua";
        }
        return "Recubrimiento desconocido";

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

	public float damageReduction{
		get{ 
			return (massRatio*(maxDamageReduction-minDamageReduction)+minDamageReduction);
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

	public int selfDamage{
		get{ 
			return (int)(Mathf.Min (-1f, -mass* attackDrain));
		}
	}

}
