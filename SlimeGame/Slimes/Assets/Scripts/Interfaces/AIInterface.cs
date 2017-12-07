
using System.Collections.Generic;

public abstract class AIInterface{
    abstract public AISlimeAction GetAction(GameController gameController);
    //abstract public double GetStateEvaluation(GameController gameController);

    /*protected AIGameState CreateGameState(GameController gameController){
        int currentTurn = gameController.GetCurrentTurn();
        return new AIGameState();
    }*/

    /*
    Función que devuelve todas las posibles acciones en forma de lista de AISlimeAction (slime, accion)
     */
    protected List<AIRawSlimeAction> GetLegalActions(AIGameState gameState){
        /* Cada slime pot:
            - Atacar un rival: que hi hagi una slime enemiga al rang d'atac.
            - Conquerir: CAP? (o que la casella no estigui conquerida?)
            - Dividirse: que tingui massa > LIMIT i tingui casella lliure al lateral
            - Fusionarse: si te alguna slime al costat
            - Menjar: cap condicio
            - Moure's: que tingui alguna casella lliure on moure's
         */

         List<AIRawSlimeAction> legalActions = new List<AIRawSlimeAction>();
         List<RawSlime> slimes = gameState.GetCurrentPlayer().GetSlimes();

         foreach(RawSlime slime in slimes){
             //legalActions.AddRange(gameState.GetAttackActions(slime));
             legalActions.AddRange(gameState.GetMoveActions(slime));
             //legalActions.AddRange(gameState.GetConquerActions(slime));
             //legalActions.AddRange(gameState.GetSplitActions(slime));
             //legalActions.AddRange(gameState.GetFusionActions(slime));
             //legalActions.AddRange(gameState.GetGrowActions(slime));
         }

         return legalActions;
    }
}