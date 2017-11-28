﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
	
	private TileData data;
	private SpriteAnimation backAnimation;
	private SpriteAnimation frontAnimation;
	public SpriteRenderer tileConquerLayer;
	public SpriteRenderer tileUILayer;
	public SpriteRenderer tileElementLayerBack;
	public SpriteRenderer tileElementLayerFront;

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

    //TODO modify when we have more attributes
    public override string ToString()
    {
		return data.ToString();
        //TODO Em peta perque data me diu que es NULL
        //return data.ToString();
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
		tileConquerLayer.gameObject.transform.localScale = new Vector2(1f,1f);;
		tileConquerLayer.sortingLayerName = "TileConquest";
		tileConquerLayer.color = new Color (1f, 1f, 1f, 1f);
	}

	public void startElementLayer(Vector3 pos, Vector3 size){
		GameObject gotileElementLayer = new GameObject ("TileElementLayer");
		gotileElementLayer.GetComponent<Transform> ().SetParent (this.transform);
		tileElementLayerBack = gotileElementLayer.AddComponent<SpriteRenderer> ();
		tileElementLayerBack.gameObject.transform.position = pos;
		tileElementLayerBack.gameObject.transform.localScale = new Vector2(1f,1f);
		tileElementLayerBack.sortingLayerName = "TileElement";
		tileElementLayerBack.sortingOrder = (int) (1000-data.GetRealWorldPosition().y*4);
		tileElementLayerBack.color = new Color (1f, 1f, 1f, 1f);

		GameObject gotileElementLayer2 = new GameObject ("TileElementLayer2");
		gotileElementLayer2.GetComponent<Transform> ().SetParent (this.transform);
		tileElementLayerFront = gotileElementLayer2.AddComponent<SpriteRenderer> ();
		tileElementLayerFront.gameObject.transform.position = pos;
		tileElementLayerFront.gameObject.transform.localScale = new Vector2(1f,1f);
		tileElementLayerFront.sortingLayerName = "TileElement";
		tileElementLayerFront.color = new Color (1f, 1f, 1f, 1f);
		tileElementLayerFront.sortingOrder = (int) (1000-data.GetRealWorldPosition().y*4+3);


		switch(Random.Range(1,5)){
			//Fire case
			case 1:
			tileElementLayerBack.gameObject.transform.position = pos+new Vector3(0.0f,+0.5f);
			tileElementLayerFront.gameObject.transform.position = pos+new Vector3(0.0f,-0.25f);
			//Animations
			frontAnimation = new SpriteAnimation (tileElementLayerFront);
			frontAnimation.LoadSprites ("Tiles/Fire/front",6);
			frontAnimation.playAnimation ();
			backAnimation = new SpriteAnimation (tileElementLayerBack);
			backAnimation.LoadSprites ("Tiles/Fire/back",6);
			backAnimation.playAnimation ();
			//Shader
			if (GameObject.Find ("Main Camera").GetComponent<GameController> () != null) {
				Material mat = GameObject.Find ("Main Camera").GetComponent<GameController> ().fire;
				tileElementLayerFront.material = mat;
			}

			if (GameObject.Find ("Main Camera").GetComponent<GameController> () != null) {
				Material mat = GameObject.Find ("Main Camera").GetComponent<GameController> ().fire;
				tileElementLayerBack.material = mat;
			}
			break;
			//Water case
			case 2:
			tileElementLayerFront.gameObject.transform.position = pos+new Vector3(0.0f,+0.2f);
			//Animations
			frontAnimation = new SpriteAnimation (tileElementLayerFront);
			frontAnimation.LoadSprites("Tiles/Water/tile_water_", 36);
			frontAnimation.mode = SpriteAnimationMode.SUBBOUNCE;
			frontAnimation.playAnimation ();
			break;
			//Earth case
			case 3:
			tileElementLayerFront.gameObject.transform.position = pos+new Vector3(0.75f,-0.25f);
			tileElementLayerBack.gameObject.transform.position = pos+new Vector3(-0.5f,+0.55f);
			tileElementLayerFront.gameObject.transform.localScale = new Vector2(0.6f,0.6f);
			tileElementLayerBack.gameObject.transform.localScale = new Vector2(0.85f,0.85f);

			//Animations
			frontAnimation = new SpriteAnimation (tileElementLayerFront);
			frontAnimation.LoadSprites("Tiles/Earth/tile_earth_", 16);
			frontAnimation.mode = SpriteAnimationMode.LOOP;
			frontAnimation.playAnimation ();

			backAnimation = new SpriteAnimation (tileElementLayerBack);
			backAnimation.LoadSprites("Tiles/Earth/tile_earth_", 16);
			backAnimation.mode = SpriteAnimationMode.LOOP;
			backAnimation.playAnimation ();
			break;
		default:
			
			break;
		}

	}

	public void SetSlimeOnTop(GameObject obj){
		data.SetSlimeOnTop (obj);
	}

	public Slime GetSlimeOnTop(){
		return data.GetSlimeOnTop ();
	}

}
