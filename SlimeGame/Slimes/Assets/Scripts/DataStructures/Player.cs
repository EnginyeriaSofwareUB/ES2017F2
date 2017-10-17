using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {

	private string name;
	private int actions;
	private List<GameObject> slimes;

	public Player(string name, int actions){
		this.name = name;
		this.actions = actions;
		slimes = new List<GameObject>();
	}

	public void AddSlime(GameObject slime){
		slimes.Add(slime);
	}

	public bool IsSlimeOwner(GameObject slime){
		bool found = false;
		int i = 0;
		while(!found && i<slimes.Count){
			found = slimes[i] == slime;
			i++;
		}
		return found;
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
}
