
using System;
using System.Collections.Generic;

public class AIAggressive : AIInterface{

    public override AISlimeAction GetAction(GameController gameController){
        List<AISlimeAction> actions = GetAttackActions(gameController);
        if(actions.Count == 0) actions = GetLegalActions(gameController);
        if(actions.Count == 0) return null;
        return actions[(int)((new Random()).Next(actions.Count))];
    }

}