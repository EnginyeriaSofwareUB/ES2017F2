
using System;
using System.Collections.Generic;
using UnityEngine;

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

    public AIGameState GetSuccessor(AIRawSlimeAction action){
        AIGameState succ = GetCopy();
        int actionSlimeId = action.GetMainSlimeId();

        // Aplicamos la accion al sucesor.
		switch(action.GetAction()) {
            case ActionType.ATTACK:
                succ.Attack(actionSlimeId, action.GetTargetSlimeId());
                break;
            case ActionType.CONQUER:
                succ.Conquer(actionSlimeId, action.GetTileVector());
                break;
            case ActionType.SPLIT:
                succ.Split(actionSlimeId, action.GetTileVector());
                break;
            case ActionType.MOVE:
                succ.Move(actionSlimeId, action.GetTileVector());
                break;
            case ActionType.FUSION:
                succ.Fusion(actionSlimeId, action.GetTargetSlimeId());
                break;
		}

        return succ;
	}

    public void Attack(int actionSlimeId, int targetSlimeId){
        RawSlime actionSlime = FindSlimeById(actionSlimeId);
        RawSlime targetSlime = FindSlimeById(targetSlimeId);

        if(targetSlime.changeMass (-actionSlime.getDamage()) <= 0){
            targetSlime.GetPlayer().RemoveSlime(targetSlime);
        }

        SpendActions(1);
    }

    public void Conquer(int actionSlimeId, Vector2 tileVector){
        RawSlime actionSlime = FindSlimeById(actionSlimeId);
        TileData toConquer = matrix.getTile((int)tileVector.x, (int)tileVector.y);
        
        actionSlime.GetPlayer().Conquer(toConquer);

        SpendActions(1);
    }

    public void Split(int actionSlimeId, Vector2 tileVector){
        RawSlime actionSlime = FindSlimeById(actionSlimeId);
        TileData toSplit = matrix.getTile((int)tileVector.x, (int)tileVector.y);

        actionSlime.Split(toSplit);

        SpendActions(1);
    }

    public void Fusion(int actionSlimeId, int targetSlimeId){
        RawSlime actionSlime = FindSlimeById(actionSlimeId);
        RawSlime fusionTargetSlime = FindSlimeById(targetSlimeId);

        fusionTargetSlime.GetPlayer().RemoveSlime(actionSlime);
		fusionTargetSlime.SetMass (actionSlime.GetMass() + fusionTargetSlime.GetMass());

        SpendActions(1);
    }

    public void Move(int actionSlimeId, Vector2 tileVector){
        RawSlime actionSlime = FindSlimeById(actionSlimeId);
        TileData toMove = matrix.getTile((int)tileVector.x, (int)tileVector.y);

        actionSlime.SetTile(toMove);

        SpendActions(1);
    }

    public void SpendActions(int numActions){
        playerActions += numActions;
		if (playerActions >= players [currentPlayer].GetActions ()) {
			currentPlayer++;
            playerActions = 0;
            if (currentPlayer >= players.Count)
            {
                // Tots els jugadors han fet la seva accio, passem al seguent torn.
                currentPlayer = 0;
                currentTurn++;    
            }
            foreach (RawPlayer pl in players) {
                pl.UpdateActions();
            }
		}
	}

    public RawSlime FindSlimeById(int id){
        foreach(RawPlayer pl in players){
            foreach(RawSlime sl in pl.GetSlimes()){
                if(sl.CheckId(id)) return sl;
            }
        }
        return null;
    }

    // Deep copy of AIGameState
    private AIGameState GetCopy(){
        Matrix rawMatrix = matrix.GetRawCopy();

        List<RawPlayer> rawPlayers = new List<RawPlayer>();
        foreach(RawPlayer pl in players){
            RawPlayer rawPl = pl.GetCopy();
            rawPlayers.Add(rawPl);

            // Sync dels conqueredtiles
            foreach(TileData tiledata in pl.GetConqueredTiles()){
				TileData matrixTile = rawMatrix.getTile((int)tiledata.getPosition().x, (int)tiledata.getPosition().y);
                rawPl.Conquer(matrixTile);
            }
        }

		// Recorrem la llista de RawSlimes actualitzant les tiledata de matrix.
		foreach(RawPlayer pl in rawPlayers){
			foreach(RawSlime sl in pl.GetSlimes()){
                // Substituim la RawTileData provisional que te RawSlime per la de la matriu (perque quedin enllaçades)
                TileData slimeTile = sl.GetTileData();
				TileData matrixTile = rawMatrix.getTile((int)slimeTile.getPosition().x, (int)slimeTile.getPosition().y);
                sl.SetTile(matrixTile);
			}
		}
        
        return new AIGameState(rawMatrix, rawPlayers, currentTurn, currentPlayer, playerActions);
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