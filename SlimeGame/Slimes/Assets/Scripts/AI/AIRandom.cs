
using System;
using System.Collections.Generic;
using UnityEngine;

public class AIRandom : AIInterface{

    public AIRandom(GameController gameController) : base(gameController){}

    protected override void ThinkAction(){
        // Retornamos una accion aleatoria.
        AIGameState gameState = gameController.GetGameState();
        List<AIRawSlimeAction> legalActions = gameState.GetLegalActions();
        if(legalActions.Count != 0) thoughtAction = legalActions[(int)((new System.Random()).Next(legalActions.Count))].CopyToRealAction(gameController);
    }

    protected override double GetStateEvaluation(AIGameState state){
        return 0;
    }
}