using UnityEngine;

public class SlimeAction{
    
	private ActionType action;
	private object data;

	public SlimeAction(ActionType action, Tile data){
		this.action = action;
		this.data = data;
	}

	public SlimeAction(ActionType action, Slime data){
		this.action = action;
		this.data = data;
	}

	public ActionType GetAction(){
		return this.action;
	}

	public Tile GetTile(){
		return (Tile)this.data;	
	}

	public Slime GetSlime(){
		return (Slime)this.data;
	}

}