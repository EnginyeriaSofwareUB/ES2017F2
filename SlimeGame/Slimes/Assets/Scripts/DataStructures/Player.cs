using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {

	public SlimeCoreData slimeCoreData;
	private string name;
	private int actions;
	private List<Slime> slimes;
	private Color color;

	public Player(string name, int actions,SlimeCoreData slimeCoreData){
		this.name = name;
		this.actions = actions;
		slimes = new List<Slime>();
		this.slimeCoreData = slimeCoreData;
	}

	public void SetColor(Color c){
		color = c;
	}

	public Color GetColor(){
		return color;
	}

	public void AddSlime(Slime slime){
		slimes.Add(slime);
	}

	public bool IsSlimeOwner(Slime slime){

		return slimes.Contains (slime);
	
	}

	public string GetName(){
		return name;
	}
	public int GetNumSlimes(){
		return slimes.Count;
	}
	public int GetActions(){
		return this.actions;
	}
	public List<Slime> GetSlimes(){
		return slimes;
	}
}
