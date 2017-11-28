
using System;
using System.Collections.Generic;

public class AIRandom : AIInterface{

    public override AISlimeAction GetAction(GameController gameController){
        List<AISlimeAction> legalActions = GetLegalActions(gameController);
        if(legalActions.Count == 0) return null;
        return legalActions[(int)((new Random()).Next(legalActions.Count))];
    }
}