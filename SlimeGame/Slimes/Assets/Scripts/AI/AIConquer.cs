
using System;
using System.Collections.Generic;
using UnityEngine;

public class AIConquer : AIInterface{

    public override AISlimeAction GetAction(GameController gameController){
        // Retornamos una accion aleatoria.
        AIGameState gameState = gameController.GetGameState();
        List<AIRawSlimeAction> legalActions = GetLegalActions(gameState);
        if(legalActions.Count == 0) return null;
        AIRawSlimeAction picked = legalActions[(int)((new System.Random()).Next(legalActions.Count))];
        // Creamos la AISlimeAction
        Debug.Log(picked.ToString());
        return picked.CopyToRealAction(gameController);
    }
}