using SimpleJSON;

public class SlimeCoreData
{
	//Game stats
	public int attack;
	public int movementRange;
	public int attackRange;
	public int startingHP;
	public int attackCost;
	public float splitCost;
	public float splitStartHP;

	//SpritesData
	public string picDirection;

	public SlimeCoreData (JSONNode data)
	{
		this.attack = data["attack"];
		this.movementRange = data["movementRange"];
		this.attackRange = data["attackRange"];
		this.startingHP = data["startingHP"];
		this.attackCost = data["attackCost"];
		this.splitCost = 0.5f;
		this.splitStartHP = 0.35f;
		this.picDirection = data["picDirection"];
	}

}