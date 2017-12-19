
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
        
        double score = 1000;
        RawPlayer winner = state.IsGameEndedAndWinner();
        if(winner != null && winner.GetId() == playerId) score += 10000; // if it wins
        else if(winner != null || state.GetPlayerById(playerId) == null) score -= 10000; // if it loses
        else{
            RawPlayer player = state.GetPlayerById(playerId);
            int plConquered = player.GetConqueredTiles().Count;
            int plSlimes = player.GetSlimes().Count;

            double totalPlayerMass = 0;
            int playerSlimesSplitable = 0;

            foreach(RawSlime sl in player.GetSlimes()){
                totalPlayerMass += sl.GetMass();
                if(sl.canSplit) playerSlimesSplitable++;
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

            score += plConquered * 12;
            score += plSlimes * 10;
            score += playerSlimesSplitable * 5;

            score -= 200 * (enemies.Count); // Cuanto menos jugadores, mejor.
            score -= totalEnemiesMass; // Predileccio per atacar
            score -= enemiesSlimes * 100; // slime morta
            score -= enemiesThatCanAttackMe * 2; // Com menys enemics puguin atacarme, millor
            score -= minimumMass * 5; // Predileccio per atacar al que esta mes fluix
        }
        return  score * GetAIError();
    }
}