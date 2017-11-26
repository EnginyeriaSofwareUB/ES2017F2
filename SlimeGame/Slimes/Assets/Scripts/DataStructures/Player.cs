using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {

	public SlimeCoreData slimeCoreData;
	private string name;
	private int actions;
    private float actionsPerSlime;
	private List<Slime> slimes;
    private List<SlimeAction> tutorialActions;
    private int positionTutorial;
	private Color color;
	//Indica si un player es IA
	private bool isAI;

	private AIInterface brain;

	public Player(string name, float actionsPerSlime,SlimeCoreData slimeCoreData){
		this.name = name;
		this.actionsPerSlime = actionsPerSlime;
		slimes = new List<Slime>();

        positionTutorial=0;
        tutorialActions = new List<SlimeAction>();
		this.slimeCoreData = slimeCoreData;
        updateActions();
		isAI = false;
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
        tutorialActions.Add(new SlimeAction(ActionType.CONQUER, MapDrawer.GetTileAt(-1, -1)));
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

	public SlimeAction GetAction(GameController g){
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
    public bool isTutorialAction(SlimeAction slimeAction)
    {
        if (positionTutorial >= tutorialActions.Count)
        {
            return false;
        }
        if (tutorialActions[positionTutorial].GetAction() == ActionType.ATTACK || tutorialActions[positionTutorial].GetAction() == ActionType.FUSION)
            tutorialActions[positionTutorial].ChangeTileForSlime();
        if (slimeAction.IsEqual(tutorialActions[positionTutorial]))
        {
            positionTutorial++;
            return true;
        }
        else
        {
            return false;
        }
    }
}
