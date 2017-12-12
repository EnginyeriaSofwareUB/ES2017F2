public class AISlimeAction : SlimeAction{
    private Slime actionSlime;

    public AISlimeAction(Slime slime, ActionType action, Tile data) : base(action, data){
        this.actionSlime = slime;
	}

	public AISlimeAction(Slime slime, ActionType action, Slime data) : base(action, data){
        this.actionSlime = slime;
	}

	public AISlimeAction(Slime actionSlime, SlimeAction action) : base(action.GetAction(), action.GetData()){
        this.actionSlime = actionSlime;
	}

    public Slime GetMainSlime(){
        return this.actionSlime;
    }

    public override string ToString(){
		string toReturn = actionSlime.GetId() + " - ";
		switch(GetAction()){
            case ActionType.ATTACK:
			toReturn += "attack - " + GetSlime().GetId();
			break;
            case ActionType.FUSION:
			toReturn += "fusion with - " + GetSlime().GetId();
			break;
            case ActionType.CONQUER:
			toReturn += "conquers - " + GetTile().getPosition();
			break;
            case ActionType.MOVE:
			toReturn += "moves to - " + GetTile().getPosition();
			break;
            case ActionType.SPLIT:
			toReturn += "splits to - " + GetTile().getPosition();
			break;

			default:
			toReturn += "NULL: " + GetAction();
			break;
                
        }
		return toReturn;
	}
}