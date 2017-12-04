
using System;
using System.Collections.Generic;

public class AIGameState {
    private Matrix matrix;
    private List<RawPlayer> players;
    private int currentTurn;
    private int currentPlayer;
    private int playerActions;

    public AIGameState(Matrix matrix, List<RawPlayer> players, int currentTurn, int currentPlayer, int playerActions){
        this.matrix = matrix;
        this.players = players;
        this.currentTurn = currentTurn;
        this.currentPlayer = currentPlayer;
        this.playerActions = playerActions;
    }

    public override string ToString() {
        string toReturn = "GAMESTATE (CurrentTurn: " + currentTurn + ", CurrentPlayer: " + currentPlayer + ", playerActions: " + playerActions + ")\n";
        toReturn += matrix.ToString() + "\n";
        foreach(RawPlayer pl in players){
            toReturn += pl.ToString() + "\n";
        }
        return toReturn;
    }
}