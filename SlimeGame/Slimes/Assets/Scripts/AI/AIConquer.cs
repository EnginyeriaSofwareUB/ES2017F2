
using System;
using System.Collections.Generic;
using UnityEngine;

public class AIConquer : AIInterface{

    public AIConquer(GameController gameController) : base(gameController){}

    protected override void ThinkAction(){
        AIRawSlimeAction action = GetActionWithAlphaBeta(gameController.GetGameState());
        // Creamos la AISlimeAction
        thoughtAction = action.CopyToRealAction(gameController);
    }

    protected override double GetStateEvaluation(AIGameState state){
        RawPlayer AIPlayer = state.GetPlayerById(playerId);
        int plConquered = AIPlayer.GetConqueredTiles().Count;
        int plSlimes = AIPlayer.GetSlimes().Count;
        return  plConquered * 12 + plSlimes * 5;
    }
}