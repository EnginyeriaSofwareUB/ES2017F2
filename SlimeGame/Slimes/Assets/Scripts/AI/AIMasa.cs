
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
        double score = 1000;
        RawPlayer winner = state.IsGameEndedAndWinner();
        if(winner != null && winner.GetId() == playerId) score += 10000; // if it wins
        else if(winner != null || state.GetPlayerById(playerId) == null) score -= 10000; // if it loses
        else{
            RawPlayer player = state.GetPlayerById(playerId);

            // INFO sobre el JUGADOR
            int playerSlimes = player.GetSlimes().Count;
            double totalPlayerMass = 0;
            int playerSlimesSplitable = 0;
            int playersOverConquered = 0;

            foreach(RawSlime sl in player.GetSlimes()){
                totalPlayerMass += sl.GetMass();
                if(sl.canSplit) playerSlimesSplitable++;
                if(player.GetConqueredTiles().Contains(sl.GetActualTile())) playersOverConquered++;
            }

            // INFO sobre los ENEMIGOS
            List<RawPlayer> enemies = state.GetPlayers().FindAll(p => true);
            enemies.Remove(player);
            int enemiesSlimes = 0;
            float totalEnemiesMass = 0;
            int enemiesThatCanAttackMe = 0;
            float minimumMass = Int16.MaxValue;
            foreach(RawPlayer enemy in enemies){
                foreach(RawSlime sl in enemy.GetSlimes()){
                    enemiesSlimes++;
                    totalEnemiesMass += sl.GetMass();

                    enemiesThatCanAttackMe += state.GetSlimesInAttackRange(sl).FindAll(r => r.GetPlayer().GetId() == player.GetId()).Count;
                    minimumMass = Mathf.Min(sl.GetMass(), minimumMass);
                }
            }


            score += playerSlimes * 100;
            score += 4 * totalPlayerMass * GetAIError();
            score += playerSlimesSplitable * 40 * GetAIError();
            score += playersOverConquered * 40 * GetAIError();

            score -= 200 * (enemies.Count); // Cuanto menos jugadores, mejor.
            score -= 10 * enemiesSlimes;
            score -= 2*totalEnemiesMass;
            score -= enemiesThatCanAttackMe * 3;
            score -= minimumMass;
        }

        Debug.Log(score);
        return  score * GetAIError();
    }
}