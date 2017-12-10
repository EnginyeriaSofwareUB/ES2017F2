using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialIA : AIInterface {

    private List<SlimeAction> actions;
    private int position;

    public TutorialIA(GameController gameController) : base(gameController)
    {
        
        position = -1;
        actions = new List<SlimeAction>();

        //Init actions
        actions.Add(new SlimeAction(ActionType.CONQUER, MapDrawer.GetTileAt(-4, 1)));
        actions.Add(new SlimeAction(ActionType.MOVE, MapDrawer.GetTileAt(-3, 2)));
        actions.Add(new SlimeAction(ActionType.SPLIT, MapDrawer.GetTileAt(-2,1)));
        actions.Add(new SlimeAction(ActionType.ATTACK, MapDrawer.GetTileAt(-1,-1))); //Slime 1
        actions.Add(new SlimeAction(ActionType.MOVE, MapDrawer.GetTileAt(-3, 0)));
        actions.Add(new SlimeAction(ActionType.ATTACK, MapDrawer.GetTileAt(-1, -1))); 
        actions.Add(new SlimeAction(ActionType.ATTACK, MapDrawer.GetTileAt(-1, -1))); 
    }

    protected override void ThinkAction(){
        position++;
        Slime actionSlime = null;
        if (position == 3)
            actionSlime = gameController.GetCurrentPlayer().GetSlimes()[1];
        else
            actionSlime = gameController.GetCurrentPlayer().GetSlimes()[0];


        //Solucio cutre
        if (actions[position].GetAction() == ActionType.ATTACK)
        {
            actions[position] = new SlimeAction(ActionType.ATTACK, MapDrawer.GetTileAt(-1, -1).GetSlimeOnTop());
        }
        if (position >= actions.Count)
        {
            thoughtAction = new AISlimeAction(actionSlime, ActionType.CONQUER, gameController.GetSelectedSlime().actualTile);
        }
        thoughtAction = new AISlimeAction(actionSlime, actions[position]);
    }


    protected override double GetStateEvaluation(AIGameState state){
        return 0;
    }
}
