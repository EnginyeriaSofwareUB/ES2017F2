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

    public Slime GetSlime(){
        return this.actionSlime;
    }
}