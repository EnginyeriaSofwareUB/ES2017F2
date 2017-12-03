using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {

	public StatsContainer statsCoreInfo;
	private string name;
	private int actions;
    private float actionsPerSlime;
	private List<Slime> slimes;
	private List<Tile> conqueredTiles;
    private List<SlimeAction> tutorialActions;
    private int positionTutorial;
	private Color color;
	//Indica si un player es IA
	private bool isAI;

	private AIInterface brain;

	public Player(string name, float actionsPerSlime,StatsContainer coreInfo){
		this.name = name;
		this.actionsPerSlime = actionsPerSlime;
		slimes = new List<Slime>();
		conqueredTiles = new List<Tile>();

        positionTutorial=0;
        tutorialActions = new List<SlimeAction>();
		this.statsCoreInfo = coreInfo;
        updateActions();
		isAI = false;
	}

	public Player(string name, float actionsPerSlime,StatsContainer coreInfo, AIInterface brain){
		this.name = name;
		//this.actions = actions;
		this.actionsPerSlime = actionsPerSlime;
		slimes = new List<Slime>();
		conqueredTiles = new List<Tile>();
		this.statsCoreInfo = coreInfo;
        updateActions();
		SetBrain(brain);
	}

	public void SetColor(Color c){
		color = c;
	}

    public void setTutorialActions()
    {
        tutorialActions.Add(new SlimeAction(ActionType.SPLIT, MapDrawer.GetTileAt(3, -3)));
        tutorialActions.Add(new SlimeAction(ActionType.MOVE, MapDrawer.GetTileAt(1, -2)));
        tutorialActions.Add(new SlimeAction(ActionType.MOVE, MapDrawer.GetTileAt(1, -1))); //Slime 1
        tutorialActions.Add(new SlimeAction(ActionType.MOVE, MapDrawer.GetTileAt(-1, -1)));
        tutorialActions.Add(new SlimeAction(ActionType.CONQUER, MapDrawer.GetTileAt(-1, -1)));
        tutorialActions.Add(new SlimeAction(ActionType.MOVE, MapDrawer.GetTileAt(0, -1))); //Slime 1
        tutorialActions.Add(new SlimeAction(ActionType.FUSION, MapDrawer.GetTileAt(-1, -1))); //Slime 1
        tutorialActions.Add(new SlimeAction(ActionType.ATTACK, MapDrawer.GetTileAt(-3, 0)));
        //tutorialActions.Add(new SlimeAction(ActionType.CONQUER, MapDrawer.GetTileAt(-1, -1)));
        tutorialActions.Add(new SlimeAction(ActionType.ATTACK, MapDrawer.GetTileAt(-2, 1)));
    }
    public void updateActions()
    {
        actions = (int) actionsPerSlime * slimes.Count;
        if (actions < 1)
            actions = 1;
    }

	public Color GetColor(){
		return color;
	}

	public void AddSlime(Slime slime){
		slimes.Add(slime);
	}

	public bool IsSlimeOwner(Slime slime){

		return slimes.Contains (slime);
	
	}

	public AISlimeAction GetAction(GameController g){
		if (brain != null) {
			return brain.GetAction (g);
		}
		return null;
	}

	public void SetBrain(AIInterface brain){
		isAI = true;
		this.brain = brain;
	}

	public bool isPlayerAI(){
		return isAI;
	}

	public void RemoveSlime(Slime sl){
		this.slimes.Remove(sl);
	}


	public string GetName(){
		return name;
	}
	public int GetNumSlimes(){
		return slimes.Count;
	}
	public int GetActions(){
		return this.actions;
	}
	public List<Slime> GetSlimes(){
		return slimes;
	}

    //Nomes usar per al tutorial
    public bool RightSlime(Slime whoActions)
    {
        if (positionTutorial == 2 || positionTutorial == 5 || positionTutorial == 6)
            return whoActions == slimes[1];
        else
            return whoActions == slimes[0];
    }


    //Nomes usar per al tutorial
    public SlimeAction nextAction()
    {
        return tutorialActions[positionTutorial];
    }

    //Nomes usar per al tutorial
    public bool isTutorialAction(SlimeAction slimeAction, Slime whoActions)
    {
        if (positionTutorial >= tutorialActions.Count)
        {
            return false;
        }
        if (tutorialActions[positionTutorial].GetAction() == ActionType.ATTACK || tutorialActions[positionTutorial].GetAction() == ActionType.FUSION)
            tutorialActions[positionTutorial].ChangeTileForSlime();
        if (slimeAction.IsEqual(tutorialActions[positionTutorial]) && RightSlime(whoActions))
        {
            positionTutorial++;
            return true;
        }
        else
        {
            return false;
        }
    }

	public float GetTotalMass(){
		float totalMass = 0;
		foreach (Slime slime in slimes){
			totalMass+=slime.GetMass();
		}
		return totalMass;
	}

	public void AddConqueredTile(Tile tile){
		if (!HasConqueredTile(tile)) conqueredTiles.Add(tile);
	}

	public bool HasConqueredTile(Tile tile){
		if (NumConqueredTiles()!=0)
			return conqueredTiles.Contains(tile);
		return false;
	}

	public void RemoveConqueredTile(Tile tile){
		if (HasConqueredTile(tile)) conqueredTiles.Remove(tile);
	}

	public int NumConqueredTiles(){
		return conqueredTiles.Count;
	}
}
