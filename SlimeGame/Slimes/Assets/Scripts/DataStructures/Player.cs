using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {

	public static int ID = 0;
	public int id;
	public StatsContainer statsCoreInfo;
	private string name;
	private List<Slime> slimes;
	private List<Tile> conqueredTiles;
    private List<SlimeAction> tutorialActions;
    private int positionTutorial;
	private Color color;
	//Indica si un player es IA
	private bool isAI;

	private AIInterface brain;

	public Player(string name,StatsContainer coreInfo){
		this.name = name;
		slimes = new List<Slime>();
		conqueredTiles = new List<Tile>();

        positionTutorial=0;
        tutorialActions = new List<SlimeAction>();
		this.statsCoreInfo = coreInfo;
		isAI = false;

		this.id = ID;
		ID++;
	}

	public Player(string name,StatsContainer coreInfo, AIInterface brain){
		this.name = name;
		//this.actions = actions;
		slimes = new List<Slime>();
		conqueredTiles = new List<Tile>();
		this.statsCoreInfo = coreInfo;
		SetBrain(brain);
		isAI = true;
		this.id = ID;
		ID++;
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

	public int actions
    {
		get{ 
			return statsCoreInfo.baseActions + (int)(slimes.Count/statsCoreInfo.slimeCountActionGain);
		}
        
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

	public bool ThinkAction(){
		if(!IsThinking()){
			brain.Start();
			return true;
		}
		return false;
	}

	public bool IsThinking(){
		if(brain != null && brain.IsDone){
			return false;
		}
		return true;
	}

	public void StopThinking(){
		brain.Abort();
	}

	public AISlimeAction GetThoughtAction(){
		if (!IsThinking()) {
			return brain.PopAction ();
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

	public List<Tile> GetConqueredTiles(){
		return conqueredTiles;
	}

	public RawPlayer GetRawCopy(){
		RawPlayer rawPlayer = new RawPlayer(id, actions, statsCoreInfo, statsCoreInfo.slimeCountActionGain);
		List<RawSlime> rawSlimes = new List<RawSlime>();
		foreach(Slime sl in slimes){
			RawSlime rawSl = sl.GetRawCopy();
			rawSl.SetPlayer(rawPlayer);
			rawSlimes.Add(rawSl);
		}
		rawPlayer.SetSlimes(rawSlimes);
		return rawPlayer;
	}
}
