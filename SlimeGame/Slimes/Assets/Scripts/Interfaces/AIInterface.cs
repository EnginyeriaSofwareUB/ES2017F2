
using System.Collections.Generic;

public abstract class AIInterface{
    abstract public AISlimeAction GetAction(GameController gameController);
    //abstract public double GetStateEvaluation(GameController gameController);

    protected List<AISlimeAction> GetLegalActions(GameController gameController){
        /* Cada slime pot:
            - Atacar un rival: que hi hagi una slime enemiga al rang d'atac.
            - Conquerir: CAP? (o que la casella no estigui conquerida?)
            - Dividirse: que tingui massa > LIMIT i tingui casella lliure al lateral
            - Fusionarse: si te alguna slime al costat
            - Menjar: cap condicio
            - Moure's: que tingui alguna casella lliure on moure's
         */

         List<AISlimeAction> legalActions = new List<AISlimeAction>();
         List<Slime> slimes = gameController.GetCurrentPlayer().GetSlimes();

         foreach(Slime slime in slimes){
             legalActions.AddRange(GetAttackActions(gameController, slime));
             legalActions.AddRange(GetMoveActions(gameController, slime));
             legalActions.AddRange(GetConquerActions(gameController, slime));
             legalActions.AddRange(GetSplitActions(gameController, slime));
             legalActions.AddRange(GetFusionActions(gameController, slime));
             //legalActions.AddRange(GetEatActions(gameController, slime));
         }

         return legalActions;
    }

    public List<AISlimeAction> GetAttackActions(GameController gameController, Slime slime){
        List<AISlimeAction> actions = new List<AISlimeAction>();
        foreach(Slime toAttack in gameController.GetSlimesInAttackRange(slime)){
            actions.Add(new AISlimeAction(slime, ActionType.ATTACK, toAttack));
        }
        return actions;
    }

    public List<AISlimeAction> GetAttackActions(GameController gameController){
        List<AISlimeAction> actions = new List<AISlimeAction>();
        foreach(Slime slime in gameController.GetCurrentPlayer().GetSlimes()){
            foreach(Slime toAttack in gameController.GetSlimesInAttackRange(slime)){
                actions.Add(new AISlimeAction(slime, ActionType.ATTACK, toAttack));
            }
        }
        return actions;
    }

    public List<AISlimeAction> GetMoveActions(GameController gameController, Slime slime){
        List<AISlimeAction> actions = new List<AISlimeAction>();
        foreach(Tile tile in gameController.GetPossibleMovements(slime)){
            actions.Add(new AISlimeAction(slime, ActionType.MOVE, tile));
        }
        return actions;
    }

    public List<AISlimeAction> GetConquerActions(GameController gameController, Slime slime){
        List<AISlimeAction> actions = new List<AISlimeAction>();
        actions.Add(new AISlimeAction(slime, ActionType.CONQUER, slime.GetActualTile()));
        return actions;
    }

    public List<AISlimeAction> GetSplitActions(GameController gameController, Slime slime){
        List<AISlimeAction> actions = new List<AISlimeAction>();
        foreach(Tile tile in gameController.GetSplitRangeTiles(slime)){
            actions.Add(new AISlimeAction(slime, ActionType.SPLIT, tile));
        }
        return actions;
    }

    public List<AISlimeAction> GetFusionActions(GameController gameController, Slime slime){
        List<AISlimeAction> actions = new List<AISlimeAction>();
        foreach(Slime sl in gameController.GetFusionTargets(slime)){
            actions.Add(new AISlimeAction(slime, ActionType.FUSION, sl));
        }
        return actions;
    }

    private List<AISlimeAction> GetEatActions(GameController gameController, Slime slime){
        List<AISlimeAction> actions = new List<AISlimeAction>();
        return actions;
    }
}