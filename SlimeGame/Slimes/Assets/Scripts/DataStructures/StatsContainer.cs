using System;

public class StatsContainer
{
	
	public string picDirection;
	public int picCount;

	public string name;
	public int minBaseAttack;
	public int maxBaseAttack;
	public float minAttackDrain;
	public float maxAttackDrain;
	public float minAttackMultiplier;
	public float maxAttackMultiplier;
	public float splitDrain;
	public int maxSlimes;
	public int plainMassGain;
	public float scalingMassGain;
	public float conquerMassDrain;
	public float slimeCountActionGain;
	public int minCalcMass;
	public int maxCalcMass;
	public float minDamageReduction;
	public float maxDamageReduction;
	public int maxMass;
	public int movement;
	public int range;
	public int baseActions;
	public int baseMass;

	public StatsContainer (SimpleJSON.JSONNode data)
	{
		name = data["name"];
		picDirection = data ["picDirection"];
		picCount = data ["picCount"];

		minBaseAttack = data["minBaseAttack"];
		maxBaseAttack = data["maxBaseAttack"];
		minAttackDrain = data["minAttackDrain"];
		maxAttackDrain = data["maxAttackDrain"];
		minAttackMultiplier = data["minAttackMultiplier"];
		maxAttackMultiplier = data["maxAttackMultiplier"];
		splitDrain = data["splitDrain"];
		maxSlimes = data["maxSlimes"];
		plainMassGain = data["plainMassGain"];
		scalingMassGain = data["scalingMassGain"];
		conquerMassDrain = data["conquerMassDrain"];
		slimeCountActionGain = data["slimeCountActionGain"];
		minCalcMass = data["minCalcMass"];
		maxCalcMass = data["maxCalcMass"];
		minDamageReduction = data["minDamageReduction"];
		maxDamageReduction = data["maxDamageReduction"];
		maxMass = data["maxMass"];
		movement = data["movement"];
		range = data["range"];
		baseActions = data["baseActions"];
		baseMass = data["baseMass"];
	
	}
}