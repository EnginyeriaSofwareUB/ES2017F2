
using System;
using System.Collections.Generic;
using UnityEngine;

public class AIMasa : AIInterface{

    public AIMasa(GameController gameController) : base(gameController){}

    protected override void ThinkAction(){
        AIRawSlimeAction action = GetActionWithAlphaBeta(gameController.GetGameState());
        // Creamos la AISlimeAction
        thoughtAction = action.CopyToRealAction(gameController);
    }

    protected override double GetStateEvaluation(AIGameState state){
        RawPlayer player = state.GetPlayerById(playerId);

        // INFO sobre el JUGADOR
        int playerSlimes = player.GetSlimes().Count;
        double totalPlayerMass = 0;
        int playerSlimesSplitable = 0;

        foreach(RawSlime sl in player.GetSlimes()){
            totalPlayerMass += sl.GetMass();
            if(sl.canSplit) playerSlimesSplitable++;
        }

        double score = 1000;
        score += playerSlimes * 100;
        score += totalPlayerMass;
        score += playerSlimesSplitable * 10;

        // Contrincantes
        List<RawPlayer> enemies = state.GetPlayers().FindAll(p => true);
        enemies.Remove(player);
        score -= 100 * (enemies.Count); // Cuanto menos jugadores, mejor.

        RawPlayer winner = state.IsGameEndedAndWinner();
        if(winner != null && winner.GetId() == player.GetId()) score += 10000; // if it wins
        else if(winner != null) score -= 10000; // if it loses

        Debug.Log(score);
        return  score;
    }
}