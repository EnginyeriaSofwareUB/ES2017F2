﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour {
	private Player player;
	public Tile actualTile;
	public Dictionary<TileData,List<TileData>> possibleMovements;
	public bool rangeUpdated;
	private float mass;
	private SpriteAnimation animation;
	// Use this for initialization
	void Start () {
		rangeUpdated = false;
	}
	
	// Update is called once per frame
	void Update () {
		animation.update ();
	}

	public void initSpriteAnimation(){
		animation = new SpriteAnimation (gameObject.GetComponent<SpriteRenderer>());
		animation.LoadSprites (player.slimeCoreData.picDirection,5);
		animation.playAnimation ();

	}

	public void ManhattanDistance(Vector2 pos1, Vector2 pos2){

	}

    //TODO modify when we have more attributes
    public override string ToString()
    {
		return mass.ToString();
    }
	public void SetActualTile(Tile newTile){
		if(actualTile!=null)actualTile.SetSlimeOnTop(null);
		actualTile=newTile;
		actualTile.SetSlimeOnTop(gameObject);
	}
	public Tile GetActualTile(){
		return actualTile;
	}

	public TileData GetTileData(){
		return actualTile.GetTileData ();
	}

	public void setPlayer(Player player){
		this.player = player;
		mass = player.slimeCoreData.startingHP;
		initSpriteAnimation ();
		gameObject.GetComponent<SpriteRenderer> ().color = player.GetColor ();
	}

	public Player GetPlayer(){
		return player;
	}

	public int GetMovementRange(){
		return player.slimeCoreData.movementRange;
	}

	public int GetAttackRange(){
		return player.slimeCoreData.attackRange;
	}

	public void changeMass(float q){
		mass += q;
	}

	public float getDamage(){
		return player.slimeCoreData.attack;
	}

	public bool isAlive(){
		return mass > 0.0f ? true : false;
	}

	public void SetMass(float mass){
		this.mass = mass;
	}

	public float GetMass(){
		return mass;
	}
}
