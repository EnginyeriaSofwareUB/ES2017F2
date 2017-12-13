using UnityEngine;

public class AIRawSlimeAction {
    private int actionSlime;
	private ActionType action;
	private object data;

    public AIRawSlimeAction(int slime, ActionType action, Vector2 data) {
        this.actionSlime = slime;
		this.action = action;
		this.data = data;
	}

	public AIRawSlimeAction(int slime, ActionType action, int data) {
        this.actionSlime = slime;
		this.action = action;
		this.data = data;
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

	

    public AISlimeAction CopyToRealAction(GameController gameController){
        Slime actionSlime = gameController.FindSlimeById(GetMainSlimeId());
        switch(GetAction()){
            case ActionType.ATTACK:
            case ActionType.FUSION:
                Slime targetSlime = gameController.FindSlimeById(GetTargetSlimeId());
                return new AISlimeAction(actionSlime, GetAction(), targetSlime);
            default:
                Tile tile = MapDrawer.GetTileAt((int)GetTileVector().x, (int)GetTileVector().y);
                return new AISlimeAction(actionSlime, GetAction(), tile);
        }
    }

	public override string ToString(){
		string toReturn = GetMainSlimeId() + " - ";
		switch(GetAction()){
            case ActionType.ATTACK:
			toReturn += "attack - " + GetTargetSlimeId();
			break;
            case ActionType.FUSION:
			toReturn += "fusion with - " + GetTargetSlimeId();
			break;
            case ActionType.CONQUER:
			toReturn += "conquers - " + GetTileVector();
			break;
            case ActionType.MOVE:
			toReturn += "moves to - " + GetTileVector();
			break;
            case ActionType.SPLIT:
			toReturn += "splits to - " + GetTileVector();
			break;

			default:
			toReturn += "NULL: " + GetAction();
			break;
                
        }
		return toReturn;
	}
}