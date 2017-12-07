
using System;
using System.Collections.Generic;

public class AIAggressive : AIInterface{

    public override AISlimeAction GetAction(GameController gameController){

        // Damos prioridad a atacar.
        /*List<AISlimeAction> actions = GetAttackActions(gameController);
        // Si no podemos atacar, devolvemos una accion aleatoria.
        if(actions.Count == 0) actions = GetLegalActions(gameController);
        // Si no hay acciones que hacer, retornamos null y acabara el turno.
        if(actions.Count == 0) return null;
        return actions[(int)((new Random()).Next(actions.Count))];*/
        return null;
    }

}