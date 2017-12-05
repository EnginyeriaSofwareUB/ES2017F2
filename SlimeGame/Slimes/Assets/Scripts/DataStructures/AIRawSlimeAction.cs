using UnityEngine;

public class AIRawSlimeAction {
    private int actionSlime;
	private ActionType action;
	private object data;

    public AIRawSlimeAction(int slime, ActionType action, Vector2 data) {
        this.actionSlime = slime;
	}

	public AIRawSlimeAction(int slime, ActionType action, int data) {
        this.actionSlime = slime;
	}

    public int GetMainSlimeId(){
        return this.actionSlime;
    }

	public Vector2 GetTileVector(){
		return (Vector2)this.data;	
	}

	public int GetTargetSlimeId(){
		return (int)this.data;
	}

	public ActionType GetAction(){
		return this.action;
	}
}