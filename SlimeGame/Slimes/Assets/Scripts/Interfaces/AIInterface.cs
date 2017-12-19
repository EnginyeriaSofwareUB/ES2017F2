
using System.Collections.Generic;
using UnityEngine;

public abstract class AIInterface : ThreadedJob{
    protected GameController gameController;
    protected AISlimeAction thoughtAction;
    //private int maxDepth = 3;
    //private int minDepth = 1;
    //private float depthSlimeFactor = 3; // cada 2 slimes en propiedad, se restará 1 accion de profundidad, hasta 1
    private int depth = 3; // profundidad de acciones que se analizará.
    protected int playerId = 0; // jugador propietario de la IA
    private int turn = 0; // turno sobre el que se esta calculando la accion
    private int counter;

    abstract protected void ThinkAction();
    abstract protected double GetStateEvaluation(AIGameState state);


    public AIInterface(GameController gameController){
        this.gameController = gameController;
    }

    protected override void ThreadFunction()
     {
         //Debug.Log("THINKING");
         counter = 0;
         ThinkAction();
         //Debug.Log("State evaluations: " + counter);
         //Debug.Log("FIN");
     }

    // Devuelve la accion pensada.
    public AISlimeAction PopAction(){
        AISlimeAction toReturn = thoughtAction;
        thoughtAction = null;
        return toReturn;
    }    



    // Aplica algoritmo minmax con poda alpha beta para calcular la mejor accion.
    protected AIRawSlimeAction GetActionWithAlphaBeta(AIGameState state){
        playerId = state.GetCurrentPlayer().GetId();
        turn = state.GetCurrentTurn();

        float playerSlimes = state.GetCurrentPlayer().GetSlimes().Count;
        //depth = (int)Mathf.Max(minDepth, maxDepth-Mathf.Floor(playerSlimes/depthSlimeFactor));

        // Normalmente trabajaremos con profundidad 2. Si en cualquiera de estos dos pasos de profundidad hay que analizar mas de 3 slimes,
        // solamente bajaremos 1 grado de profundidad (sino demasiado coste).
        if(playerSlimes > 2 ||
        (state.GetRemainingActions() <= 1 && state.GetNextPlayer().GetSlimes().Count > 3)) depth = 2;

        /*if(playerSlimes > 3 ||
        (state.GetRemainingActions() <= 1 && state.GetNextPlayer().GetSlimes().Count > 4)) depth = 2;*/
        
        //Debug.Log("DEPTH: " + depth);

        return GetMaxValueAction(state, 0, double.MinValue, double.MaxValue).Key;
    }

    private KeyValuePair<AIRawSlimeAction, double> GetMaxValueAction(AIGameState state, int depth, double alpha, double beta){
        counter++;
        /*Si nos pasamos de profundidad o el fantasma no puede hacer ninguna acción, estamos ante una hoja y devolvemos
        la puntuación del estado actual y ninguna acción, obviamente.*/
        List<AIRawSlimeAction> legalActions = state.GetLegalActions(depth > 0);
        if(depth >= this.depth || legalActions.Count <= 0) return new KeyValuePair<AIRawSlimeAction, double>(null, GetStateEvaluation(state));
        //Debug.Log("MAXING: " + legalActions.Count + "actions (DEPTH=" + depth + ")");
        
        AIRawSlimeAction bestAction = null;
        double bestValue = double.MinValue;

        foreach(AIRawSlimeAction action in legalActions){
            AIGameState successor = state.GetSuccessor(action);
            int succDepth = depth+1;//successor.GetCurrentTurn() - turn;
            double succValue;

            // Si aun es el turno del jugador de la IA, maximizamos, sino minimizamos.
            if(successor.IsGameEndedAndWinner() != null)
                succValue = GetStateEvaluation(successor);
            else if(playerId == successor.GetCurrentPlayer().GetId())
                succValue = GetMaxValueAction(successor, succDepth, alpha, beta).Value;
            else 
                succValue = GetMinValueAction(successor, succDepth, alpha, beta).Value;

            // Actualizamos el maximo si el actual es mayor.
            if(succValue > bestValue){
                bestValue = succValue;
                bestAction = action;
            }

            // Si es valor mayor que beta (minimo actual del minValue), no hace falta seguir
            if(bestValue > beta) return new KeyValuePair<AIRawSlimeAction, double>(bestAction, bestValue);

            // Actualizamos el mejor valor del maxValue
            if(bestValue > alpha) alpha = bestValue;

            //if(depth == 0) Debug.Log("[MAX] " + succValue + "-" + action);

        }

        //if(depth == 0) Debug.Log("[MAX] CHOSEN: " + bestValue + "-" + bestAction);
        return new KeyValuePair<AIRawSlimeAction, double>(bestAction, bestValue);
    }

    private KeyValuePair<AIRawSlimeAction, double> GetMinValueAction(AIGameState state, int depth, double alpha, double beta){
        counter++;
        /*Si nos pasamos de profundidad o el fantasma no puede hacer ninguna acción, estamos ante una hoja y devolvemos
        la puntuación del estado actual y ninguna acción, obviamente.*/
        List<AIRawSlimeAction> legalActions = state.GetLegalActions(depth > 0);
        if(depth >= this.depth || legalActions.Count <= 0) return new KeyValuePair<AIRawSlimeAction, double>(null, GetStateEvaluation(state));
        
        //Debug.Log("MINING: " + legalActions.Count + "actions (DEPTH=" + depth + ")");
        AIRawSlimeAction bestAction = null;
        double bestValue = double.MaxValue;

        foreach(AIRawSlimeAction action in legalActions){
            AIGameState successor = state.GetSuccessor(action);
            int succDepth = depth+1;//successor.GetCurrentTurn() - turn;
            double succValue;

            // Si aun es el turno del jugador de la IA, maximizamos, sino minimizamos.            
            if(successor.IsGameEndedAndWinner() != null) succValue = GetStateEvaluation(successor);
            else if(playerId == successor.GetCurrentPlayer().GetId()) succValue = GetMaxValueAction(successor, succDepth, alpha, beta).Value;
            else succValue = GetMinValueAction(successor, succDepth, alpha, beta).Value;

            // Actualizamos el maximo si el actual es mayor.
            if(succValue < bestValue){
                bestValue = succValue;
                bestAction = action;
                

            //Debug.Log("[MIN] " + succValue + "-" + action);
            }

            // Si es valor menor que alpha (maximo actual del maxValue), no hace falta seguir
            if(bestValue < alpha) return new KeyValuePair<AIRawSlimeAction, double>(bestAction, bestValue);

            // Actualizamos el mejor valor del minValue
            if(bestValue < beta) beta = bestValue;
        }

        //Debug.Log("[MIN] CHOSEN: " + bestValue + "-" + bestAction);
        return new KeyValuePair<AIRawSlimeAction, double>(bestAction, bestValue);

    }

    protected float GetAIError(){
        int randomSign = (new System.Random()).Next(2);
        if(randomSign == 0) randomSign = -1;
        float random = (new System.Random()).Next(100);
        if(random < 50) return 1;
        else if(random < 80) return 1 - randomSign * (new System.Random()).Next(5) / 100;
        else if(random < 90) return 1 - randomSign * (new System.Random()).Next(10) / 100;
        else if(random < 95) return 1 - randomSign * (new System.Random()).Next(20) / 100;
        else if(random < 98) return 1 - randomSign * (new System.Random()).Next(30) / 100;
        else return 1 - randomSign * (new System.Random()).Next(40) / 100;
    }
}