
using System;
using System.Collections.Generic;
using UnityEngine;

public class AIConquer : AIInterface{

    public AIConquer(GameController gameController) : base(gameController){}

    protected override void ThinkAction(){
        // Retornamos una accion aleatoria.
        AIGameState gameState = gameController.GetGameState();
        
        AIRawSlimeAction action = GetActionWithAlphaBeta(gameState);

        // Creamos la AISlimeAction
        thoughtAction = action.CopyToRealAction(gameController);
    }

    /*public override AISlimeAction GetAction(GameController gameController){
        // Retornamos una accion aleatoria.
        AIGameState gameState = gameController.GetGameState();
        
        AIRawSlimeAction action = GetActionWithAlphaBeta(gameState);

        // Creamos la AISlimeAction
        Debug.Log(action.ToString());
        return action.CopyToRealAction(gameController);
    }*/

    protected override double GetStateEvaluation(AIGameState state){
        RawPlayer AIPlayer = state.GetPlayerById(playerId);
        int conqueredCount = AIPlayer.GetConqueredTiles().Count;
        int slimesCount = AIPlayer.GetSlimes().Count;
        return  conqueredCount * 12 + slimesCount * 5;
    }
}