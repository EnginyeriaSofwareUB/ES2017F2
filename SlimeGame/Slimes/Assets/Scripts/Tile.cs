﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	public ElementType elementType;
	private TileData data;
	private SpriteAnimation backAnimation;
	private SpriteAnimation frontAnimation;
	public SpriteRenderer tileConquerLayer;
	public SpriteRenderer tileUILayer;
	public SpriteRenderer tileElementLayerBack;
	public SpriteRenderer tileElementLayerFront;

	private Player player;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (backAnimation != null) {
			backAnimation.update ();
		}
		if (frontAnimation != null) {
			frontAnimation.update ();
		}
	}

	public Vector2 getPosition(){
		return data.getPosition();
	}

    public override string ToString()
    {
        if (elementType.Equals(ElementType.NONE))
        {
            return "Casilla normal";
        } else if (elementType.Equals(ElementType.EARTH))
        {
            return "Casilla de tierra";
        }
        else if (elementType.Equals(ElementType.WATER))
        {
            return "Casilla de agua";
        }
        else if (elementType.Equals(ElementType.FIRE))
        {
            return "Casilla de fuego";
        }
        return "Casilla de elemento desconocido";
    }

	public TileData GetTileData(){
		return data;
	}
	public void SetTileData(TileData data){
		this.data=data;
	}

	public void startUILayer(Vector3 pos, Vector3 size){
		GameObject gotileUILayer = new GameObject ("TileUILayer");
		gotileUILayer.GetComponent<Transform> ().SetParent (this.transform);
		tileUILayer = gotileUILayer.AddComponent<SpriteRenderer> ();
		tileUILayer.gameObject.transform.position = pos;
		tileUILayer.gameObject.transform.localScale = new Vector2(1f,1f);;
		tileUILayer.sortingLayerName = "TileUI";
		tileUILayer.color = new Color (1f, 1f, 1f, 0.5f);
	}

	public void startConquerLayer(Vector3 pos, Vector3 size){
		GameObject gotileUILayer = new GameObject ("TileConquerLayer");
		gotileUILayer.GetComponent<Transform> ().SetParent (this.transform);
		tileConquerLayer = gotileUILayer.AddComponent<SpriteRenderer> ();
		tileConquerLayer.gameObject.transform.position = pos;
		tileConquerLayer.gameObject.transform.localScale = new Vector2(1f,1f);
		tileConquerLayer.sortingLayerName = "TileElement";
		tileConquerLayer.sortingOrder = (int) (1000-data.GetRealWorldPosition().y*5);
		tileConquerLayer.color = new Color (1f, 1f, 1f, 1f);
	}

	public void startElementLayer(Vector3 pos, Vector3 size){
		
		GameObject gotileElementLayer = new GameObject ("TileElementLayer");
		gotileElementLayer.GetComponent<Transform> ().SetParent (this.transform);
		tileElementLayerBack = gotileElementLayer.AddComponent<SpriteRenderer> ();
		tileElementLayerBack.gameObject.transform.position = pos;
		tileElementLayerBack.gameObject.transform.localScale = new Vector2(1f,1f);
		tileElementLayerBack.sortingLayerName = "TileElement";
		tileElementLayerBack.sortingOrder = (int) (1000-data.GetRealWorldPosition().y*5);
		tileElementLayerBack.color = new Color (1f, 1f, 1f, 1f);

		GameObject gotileElementLayer2 = new GameObject ("TileElementLayer2");
		gotileElementLayer2.GetComponent<Transform> ().SetParent (this.transform);
		tileElementLayerFront = gotileElementLayer2.AddComponent<SpriteRenderer> ();
		tileElementLayerFront.gameObject.transform.position = pos;
		tileElementLayerFront.gameObject.transform.localScale = new Vector2(1f,1f);
		tileElementLayerFront.sortingLayerName = "TileElement";
		tileElementLayerFront.color = new Color (1f, 1f, 1f, 1f);
		tileElementLayerFront.sortingOrder = (int) (1000-data.GetRealWorldPosition().y*5+4);
		switch(Random.Range(1,5)){
			//Fire case
			case 1:
				elementType = ElementType.FIRE;
				tileElementLayerBack.gameObject.transform.position = pos + new Vector3 (0.0f, +0.5f);
				tileElementLayerFront.gameObject.transform.position = pos + new Vector3 (0.0f, -0.25f);
				//Animations
				frontAnimation = new SpriteAnimation (tileElementLayerFront);
				frontAnimation.LoadSprites ("Tiles/Fire/front", 6);
				frontAnimation.RandomStart ();
				frontAnimation.playAnimation ();
				backAnimation = new SpriteAnimation (tileElementLayerBack);
				backAnimation.LoadSprites ("Tiles/Fire/back",6);
				backAnimation.RandomStart ();
				backAnimation.playAnimation ();
				//Shader
				int baseOffset = Random.Range(0,20);
				if (GameObject.Find ("Main Camera").GetComponent<GameController> () != null) {
					Material mat = GameObject.Find ("Main Camera").GetComponent<GameController> ().fire;
					//tileElementLayerFront.material = mat;
					//tileElementLayerFront.material.SetFloat ("_RandomStart",Mathf.PI+baseOffset);
				}

				if (GameObject.Find ("Main Camera").GetComponent<GameController> () != null) {
					Material mat = GameObject.Find ("Main Camera").GetComponent<GameController> ().fire;
					//tileElementLayerBack.material = mat;
					//tileElementLayerBack.material.SetFloat ("_RandomStart",2*Mathf.PI+baseOffset);
				}
				break;
				//Water case
			case 2:
				elementType = ElementType.WATER;
				tileElementLayerFront.gameObject.transform.position = pos + new Vector3 (0.0f, 0.0f);
					//Animations
				frontAnimation = new SpriteAnimation (tileElementLayerFront);
				frontAnimation.LoadSprites ("Tiles/Water/tile_water_", 10, 26);
				frontAnimation.RandomStart ();
				frontAnimation.mode = SpriteAnimationMode.BOUNCE;
				frontAnimation.playAnimation ();

				tileElementLayerBack.gameObject.transform.position = pos + new Vector3 (1.2f, 0.7f);
				backAnimation = new SpriteAnimation (tileElementLayerBack);
				backAnimation.LoadSprites ("Tiles/Water/tile_water_", 10, 26);
				backAnimation.RandomStart ();
				backAnimation.mode = SpriteAnimationMode.BOUNCE;
				backAnimation.playAnimation ();
				break;
				//Earth case
			case 3:
				elementType = ElementType.EARTH;
				tileElementLayerFront.gameObject.transform.position = pos+new Vector3(0.75f,-0.25f);
				tileElementLayerBack.gameObject.transform.position = pos+new Vector3(-0.5f,+0.55f);
				tileElementLayerFront.gameObject.transform.localScale = new Vector2(0.6f,1f);
				tileElementLayerBack.gameObject.transform.localScale = new Vector2(0.85f,1.2f);

				//Animations
				frontAnimation = new SpriteAnimation (tileElementLayerFront);
				frontAnimation.LoadSprites("Tiles/Earth/tile_earth_", 16);
				frontAnimation.RandomStart ();
				frontAnimation.mode = SpriteAnimationMode.LOOP;
				frontAnimation.playAnimation ();

				backAnimation = new SpriteAnimation (tileElementLayerBack);
				backAnimation.LoadSprites("Tiles/Earth/tile_earth_", 16);
				backAnimation.RandomStart ();
				backAnimation.mode = SpriteAnimationMode.LOOP;
				backAnimation.playAnimation ();
				break;

			default:
				elementType = ElementType.NONE;
				break;
		}

	}

	private void StopAnimations(){
		if (backAnimation != null) {
			backAnimation.StopAnimation ();
			backAnimation = null;
		}
		if (frontAnimation != null) {
			frontAnimation.StopAnimation ();
			frontAnimation = null;
		}
	}

	public void RemoveElement(){
		StopAnimations ();
		tileElementLayerBack.sprite = null;
		tileElementLayerFront.sprite = null;
		elementType = ElementType.NONE;
	}

	public void SetSlimeOnTop(Slime obj){
		data.SetSlimeOnTop (obj);
	}

	public Slime GetSlimeOnTop(){
		return data.GetSlimeOnTop ();
	}

	public Player GetOwner(){
		return player;
	}

	public void SetOwner(Player p){
		player = p;
	}

	public string GetMass(){
		return "";
	}

	public int GetAttackRange(){
		if (elementType != null) {
			return StatsFactory.GetStat (elementType).range;
		} else {
			return 0;
		}

	}

	public int GetMovementRange(){
		if (elementType != null) {
			return StatsFactory.GetStat (elementType).movement;
		} else {
			return 0;
		}
	}

	public int GetDamage(){
		if (elementType != null) {
			return StatsFactory.GetStat (elementType).maxBaseAttack;
		} else {
			return 0;
		}
	}

	public int GetDamageReduction(){
		if (elementType != null) {
			return (int)(StatsFactory.GetStat (elementType).maxDamageReduction*100f);
		} else {
			return 0;
		}
	}

}
