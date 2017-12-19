
using System;
using System.Collections.Generic;
using UnityEngine;

public class AIMurder : AIInterface{

    public AIMurder(GameController gameController) : base(gameController){}

    protected override void ThinkAction(){
        AIRawSlimeAction action = GetActionWithAlphaBeta(gameController.GetGameState());
        // Creamos la AISlimeAction
        thoughtAction = action.CopyToRealAction(gameController);
    }

    protected override double GetStateEvaluation(AIGameState state){
        /*
        CONTRINCANTES:
        - Cuanto más masa, peor
        - Cuanto menos slimes, mejor (me atacaran menos)
        - Cuanto menos me puedan atacar, mejor
        YO:
        - Cuanto más masa, mejor
        - Cuanto más slimes
        - Cuanto más cerca de una slime enemiga con menos masa, mejor
        CONQUERIR???
         */
        //bool hasRange = false;

        
        double score = 1000;
        RawPlayer winner = state.IsGameEndedAndWinner();
        if(winner != null && winner.GetId() == playerId) {
            score += 10000; // if it wins
        }
        else if(winner != null) {
            score -= 10000; // if it loses
        }
        else{
            RawPlayer player = state.GetPlayerById(playerId);

            // Contrincantes
            List<RawPlayer> enemies = state.GetPlayers().FindAll(p => true);
            enemies.Remove(player);

            // INFO sobre el JUGADOR
            int playerSlimes = player.GetSlimes().Count;
            double totalPlayerMass = 0;

            int distanceToEnemy = Int16.MaxValue;
            foreach(RawSlime sl in player.GetSlimes()){
                totalPlayerMass += sl.GetMass();
                distanceToEnemy = Mathf.Min(distanceToEnemy, state.GetDistanceToCloserEnemy(sl));
            }


            // INFO sobre los ENEMIGOS
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

            //Debug.Log("TOTAL AI MASS: " + totalPlayerMass);
            score += totalPlayerMass; // Ens interessa tenir molta massa.
            score += playerSlimes*5; // Si pot dividirse per arribar al objectiu, ho fara
            score -= totalEnemiesMass * 10; // Predileccio per atacar
            score -= enemiesSlimes * 200; // slime morta
            score -= distanceToEnemy; // Com menys distancia millor
            score -= enemiesThatCanAttackMe * 2; // Com menys enemics puguin atacarme, millor
            score -= minimumMass * 5; // Predileccio per atacar al que esta mes fluix
        }

        return  score * GetAIError();
    }
}