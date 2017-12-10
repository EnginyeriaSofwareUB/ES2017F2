
using System.Collections.Generic;
using UnityEngine;

public abstract class AIInterface{
    protected AISlimeAction thoughtAction;
    private int maxDepth = 3;
    private int minDepth = 1;
    private float depthSlimeFactor = 2;
    private int depth = 1;
    protected int playerId = 0;
    private int turn = 0;

    abstract public void ThinkAction(GameController gameController);
    //abstract public AISlimeAction GetAction(GameController gameController);
    abstract protected double GetStateEvaluation(AIGameState state);


    public AISlimeAction PopAction(){
        if(thoughtAction != null){
            Debug.Log(thoughtAction.ToString());
            AISlimeAction toReturn = thoughtAction;
            thoughtAction = null;
            return toReturn;
        }
        return null;
    }    

    protected AIRawSlimeAction GetActionWithAlphaBeta(AIGameState state){
        playerId = state.GetCurrentPlayer().GetId();
        turn = state.GetCurrentTurn();

        float playerSlimes = state.GetCurrentPlayer().GetSlimes().Count;
        depth = (int)Mathf.Max(minDepth, maxDepth-Mathf.Floor(playerSlimes/depthSlimeFactor));
        //Debug.Log("DEPTH: " + depth);

        return GetMaxValueAction(state, 0, double.MinValue, double.MaxValue).Key;
    }

    private KeyValuePair<AIRawSlimeAction, double> GetMaxValueAction(AIGameState state, int depth, double alpha, double beta){
        //Debug.Log("MAX_VALUE");
        /*Si nos pasamos de profundidad o el fantasma no puede hacer ninguna acción, estamos ante una hoja y devolvemos
        la puntuación del estado actual y ninguna acción, obviamente.*/
        List<AIRawSlimeAction> legalActions = state.GetLegalActions();
        if(depth >= this.depth || legalActions.Count <= 0) return new KeyValuePair<AIRawSlimeAction, double>(null, GetStateEvaluation(state));
        
        AIRawSlimeAction bestAction = null;
        double bestValue = double.MinValue;

        foreach(AIRawSlimeAction action in legalActions){
            AIGameState successor = state.GetSuccessor(action);
            int succDepth = depth+1;//successor.GetCurrentTurn() - turn;
            double succValue;

            // Si aun es el turno del jugador de la IA, maximizamos, sino minimizamos.
            if(playerId == successor.GetCurrentPlayer().GetId()) succValue = GetMaxValueAction(successor, succDepth, alpha, beta).Value;
            else succValue = GetMinValueAction(successor, succDepth, alpha, beta).Value;

            // Actualizamos el maximo si el actual es mayor.
            if(succValue > bestValue){
                bestValue = succValue;
                bestAction = action;
            }

            // Si es valor mayor que beta (minimo actual del minValue), no hace falta seguir
            if(bestValue > beta) return new KeyValuePair<AIRawSlimeAction, double>(bestAction, bestValue);

            // Actualizamos el mejor valor del maxValue
            if(bestValue > alpha) alpha = bestValue;

        }

        return new KeyValuePair<AIRawSlimeAction, double>(bestAction, bestValue);
    }

    private KeyValuePair<AIRawSlimeAction, double> GetMinValueAction(AIGameState state, int depth, double alpha, double beta){
        //Debug.Log("MIN_VALUE");
        /*Si nos pasamos de profundidad o el fantasma no puede hacer ninguna acción, estamos ante una hoja y devolvemos
        la puntuación del estado actual y ninguna acción, obviamente.*/
        List<AIRawSlimeAction> legalActions = state.GetLegalActions();
        if(depth >= this.depth || legalActions.Count <= 0) return new KeyValuePair<AIRawSlimeAction, double>(null, GetStateEvaluation(state));
        
        AIRawSlimeAction bestAction = null;
        double bestValue = double.MaxValue;

        foreach(AIRawSlimeAction action in legalActions){
            AIGameState successor = state.GetSuccessor(action);
            int succDepth = depth+1;//successor.GetCurrentTurn() - turn;
            double succValue;

            // Si aun es el turno del jugador de la IA, maximizamos, sino minimizamos.
            if(playerId == successor.GetCurrentPlayer().GetId()) succValue = GetMaxValueAction(successor, depth, alpha, beta).Value;
            else succValue = GetMinValueAction(successor, succDepth, alpha, beta).Value;

            // Actualizamos el maximo si el actual es mayor.
            if(succValue < bestValue){
                bestValue = succValue;
                bestAction = action;
            }

            // Si es valor menor que alpha (maximo actual del maxValue), no hace falta seguir
            if(bestValue < alpha) return new KeyValuePair<AIRawSlimeAction, double>(bestAction, bestValue);

            // Actualizamos el mejor valor del minValue
            if(bestValue < beta) beta = bestValue;


        }

        return new KeyValuePair<AIRawSlimeAction, double>(bestAction, bestValue);

    }
}