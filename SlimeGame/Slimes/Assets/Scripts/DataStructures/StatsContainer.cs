using System;

public class StatsContainer
{
	public int maxHP = 300;
	public int attack;
	public int move;
	public int range;
	public int attackCost;
	public int startingHP;
	public string picDirection;

	public StatsContainer (SimpleJSON.JSONNode data)
	{
		attack = data ["attack"];
		move = data ["movementRange"];
		range = data ["attackRange"];
		attackCost = data ["attackCost"];
		startingHP = data ["startingHP"];
		picDirection = data ["picDirection"];
	}

}