using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {

	private string name;
	private int actions;

	public Player(string name, int actions){
		this.name = name;
		this.actions = actions;
	}

	public string getName(){
		return name;
	}

	public int getActions(){
		return this.actions;
	}
}
