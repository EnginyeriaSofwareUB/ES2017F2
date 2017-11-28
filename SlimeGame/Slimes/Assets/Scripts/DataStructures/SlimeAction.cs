using UnityEngine;

public class SlimeAction{
    
	private ActionType action;
	private object data;
    private bool changed;

	public SlimeAction(ActionType action, Tile data){
		this.action = action;
		this.data = data;
        changed = false;
	}

	public SlimeAction(ActionType action, Slime data){
		this.action = action;
		this.data = data;
	}

	public ActionType GetAction(){
		return this.action;
	}

    public object GetData()
    {
        return data;
    }

	public Tile GetTile(){
		return (Tile)this.data;	
	}

	public Slime GetSlime(){
		return (Slime)this.data;
	}

    public bool IsEqual(SlimeAction other)
    {
        return this.action == other.GetAction() && this.data == other.GetData();
    }

    public void ChangeTileForSlime()
    {
        if (!changed)
        {
            if (this.GetTile().GetSlimeOnTop() != null && this.data != null)
            {
                changed = true;
                this.data = this.GetTile().GetSlimeOnTop();
            }
        }

    }
}