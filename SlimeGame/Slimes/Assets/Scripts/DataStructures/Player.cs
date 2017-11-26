using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {

	public SlimeCoreData slimeCoreData;
	private string name;
	private int actions;
	private List<Slime> slimes;
	private Color color;
	//Indica si un player es IA
	private bool isAI;

	private AIInterface brain;

	public Player(string name, int actions,SlimeCoreData slimeCoreData){
		this.name = name;
		this.actions = actions;
		slimes = new List<Slime>();
		this.slimeCoreData = slimeCoreData;
		isAI = false;
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

	public SlimeAction GetAction(GameController g){
		if (brain != null) {
			return brain.GetAction (g);
		}
		return null;
	}

	public void SetBrain(AIInterface brain){
		isAI = true;
		this.brain = brain;
	}

	public bool isPlayerAI(){
		return isAI;
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
