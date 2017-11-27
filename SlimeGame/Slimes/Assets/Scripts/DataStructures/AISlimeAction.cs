public class AISlimeAction : SlimeAction{
    private Slime actionSlime;

    public AISlimeAction(Slime slime, ActionType action, Tile data) : base(action, data){
        this.actionSlime = slime;
	}

	public AISlimeAction(Slime slime, ActionType action, Slime data) : base(action, data){
        this.actionSlime = slime;
	}

    public Slime GetSlime(){
        return this.actionSlime;
    }
}