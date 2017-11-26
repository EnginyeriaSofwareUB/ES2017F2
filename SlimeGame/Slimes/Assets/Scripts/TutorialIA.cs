using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialIA : AIInterface {

    private List<SlimeAction> actions;
    private int position;

    public TutorialIA()
    {
        position = -1;
        actions = new List<SlimeAction>();

        //Init actions
        actions.Add(new SlimeAction(ActionType.CONQUER, MapDrawer.GetTileAt(-4, 1)));
        actions.Add(new SlimeAction(ActionType.MOVE, MapDrawer.GetTileAt(-3, 2)));
        actions.Add(new SlimeAction(ActionType.SPLIT, MapDrawer.GetTileAt(-2,1)));
        actions.Add(new SlimeAction(ActionType.ATTACK, MapDrawer.GetTileAt(-1,-1).GetSlimeOnTop())); //Slime 1
        actions.Add(new SlimeAction(ActionType.MOVE, MapDrawer.GetTileAt(-3, 0)));
        actions.Add(new SlimeAction(ActionType.ATTACK, MapDrawer.GetTileAt(-1, -1).GetSlimeOnTop())); //Slime 1
        actions.Add(new SlimeAction(ActionType.ATTACK, MapDrawer.GetTileAt(-1, -1).GetSlimeOnTop())); //Slime 1
    }

    public SlimeAction GetAction(GameController gameController)
    {
        position++;
        //Set selected slime (cutre)
        if (position == 3 || position == 5 || position == 6)
            gameController.SetSelectedSlime(gameController.GetCurrentPlayer().GetSlimes()[1]);
        else
            gameController.SetSelectedSlime(gameController.GetCurrentPlayer().GetSlimes()[0]);

        //Solucio cutre
        if (actions[position].GetAction() == ActionType.ATTACK)
        {
            actions[position] = new SlimeAction(ActionType.ATTACK, MapDrawer.GetTileAt(-1, -1).GetSlimeOnTop());
        }
        Debug.Log(actions[position].GetAction());
        return actions[position];
    }
}
