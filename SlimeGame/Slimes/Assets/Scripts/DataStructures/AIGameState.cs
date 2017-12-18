using System.Collections;
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

    public RawPlayer GetCurrentPlayer()
    {
        return players[currentPlayer % players.Count];
    }

    public RawPlayer GetNextPlayer(){
        return players[(currentPlayer+1) % players.Count];
    }

    public int GetRemainingActions(){
        return GetCurrentPlayer().GetActions() - playerActions;
    }

    /*
    Función que devuelve todas las posibles acciones en forma de lista de AISlimeAction (slime, accion)
     */
    public List<AIRawSlimeAction> GetLegalActions(){
        /* Cada slime pot:
            - Atacar un rival: que hi hagi una slime enemiga al rang d'atac.
            - Conquerir: CAP? (o que la casella no estigui conquerida?)
            - Dividirse: que tingui massa > LIMIT i tingui casella lliure al lateral
            - Fusionarse: si te alguna slime al costat
            - Menjar: cap condicio
            - Moure's: que tingui alguna casella lliure on moure's
         */

         List<AIRawSlimeAction> legalActions = new List<AIRawSlimeAction>();
         List<RawSlime> slimes = GetCurrentPlayer().GetSlimes();

         foreach(RawSlime slime in slimes){
             legalActions.AddRange(GetAttackActions(slime));
             legalActions.AddRange(GetMoveActions(slime));
             legalActions.AddRange(GetConquerActions(slime));
             legalActions.AddRange(GetSplitActions(slime));
             legalActions.AddRange(GetFusionActions(slime));
             legalActions.AddRange(GetGrowActions(slime));
         }

         return legalActions;
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
            case ActionType.EAT:
                succ.Eat(actionSlimeId);
                break;
		}

        return succ;
	}

    public void Attack(int actionSlimeId, int targetSlimeId){
        RawSlime actionSlime = FindSlimeById(actionSlimeId);
        RawSlime targetSlime = FindSlimeById(targetSlimeId);

        int damage = actionSlime.getDamage;
		if(actionSlime.changeMass ((int)-actionSlime.GetMass()*actionSlime.attackDrain) <= 0){
            actionSlime.GetPlayer().RemoveSlime(actionSlime);
        }
		if(targetSlime.changeMass ((int)-damage*targetSlime.GetDamageReduction()) <= 0){
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

		fusionTargetSlime.SetMass (actionSlime.GetMass() + fusionTargetSlime.GetMass());
        fusionTargetSlime.GetPlayer().RemoveSlime(actionSlime);

        SpendActions(1);
    }

    public void Move(int actionSlimeId, Vector2 tileVector){
        RawSlime actionSlime = FindSlimeById(actionSlimeId);
        TileData toMove = matrix.getTile((int)tileVector.x, (int)tileVector.y);

        actionSlime.SetTile(toMove);

        SpendActions(1);
    }

    public void Eat(int actionSlimeId){
        RawSlime actionSlime = FindSlimeById(actionSlimeId);
        
        actionSlime.GrowSlime();

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

    public int GetCurrentTurn(){
        return currentTurn;
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

    public List<TileData> GetPossibleMovements(RawSlime slime){
		List<TileData> moves = new List<TileData> ();
        if(matrix == null) Debug.Log("MATRIX NULL");
        if(slime == null) Debug.Log("SLIME NULL");
        if(slime.GetPlayer().statsCoreInfo == null) Debug.Log("NULL");
        Dictionary<TileData, List<TileData>> possible = matrix.possibleCoordinatesAndPath((int)slime.GetActualTile().getPosition().x, (int)slime.GetActualTile().getPosition().y, slime.GetMovementRange());
		foreach(TileData move in possible.Keys){
            moves.Add(move);
        }
        return moves;
	}

	public List<RawSlime> GetSlimesInAttackRange(RawSlime slime){
		List<RawSlime> canAttack = new List<RawSlime> ();
		Vector2 myPos = slime.GetActualTile().getPosition();
		foreach(RawPlayer p in players){
			if (p != slime.GetPlayer()){
				foreach(RawSlime s in p.GetSlimes()){
					Vector2 slPos = s.GetActualTile().getPosition();		
					if (Matrix.GetDistance(slPos, myPos) <= slime.GetAttackRange()){
						canAttack.Add(s);
					}
				}
			}
		}
		return canAttack;
	}

	public List<TileData> GetSplitRangeTiles(RawSlime slime){
		List<TileData> splitTiles = new List<TileData> ();
		foreach (TileData td in matrix.getNeighbours (slime.GetTileData(), true)) {
			if(td.GetRawSlimeOnTop() == null)
				splitTiles.Add (td);
		}
		return splitTiles;
	}

	public List<RawSlime> GetFusionTargets(RawSlime slime){
		List<RawSlime> fusionSlimes = new List<RawSlime> ();
		foreach (TileData tile in matrix.getNeighbours(slime.GetTileData(), true))
        {
            RawSlime overSlime = tile.GetRawSlimeOnTop();
            if (overSlime != null && overSlime.GetPlayer() == slime.GetPlayer())
            {
                fusionSlimes.Add(overSlime);
            }
        }
		return fusionSlimes;
	}


    

    public List<AIRawSlimeAction> GetAttackActions(RawSlime slime){
        // Devolvemos las acciones que puede hacer para atacar a otro jugador con ESA slime
        List<AIRawSlimeAction> actions = new List<AIRawSlimeAction>();
        if(slime.canAttack){
            foreach(RawSlime toAttack in GetSlimesInAttackRange(slime)){
                actions.Add(new AIRawSlimeAction(slime.GetId(), ActionType.ATTACK, toAttack.GetId()));
            }
        }
        return actions;
    }

    public List<AIRawSlimeAction> GetAttackActions(){
        // Devolvemos las acciones que puede hacer para atacar a otro jugador con cualquier slime que tenga
        List<AIRawSlimeAction> actions = new List<AIRawSlimeAction>();
        foreach(RawSlime slime in GetCurrentPlayer().GetSlimes()){
            if(slime.canAttack){
                foreach(RawSlime toAttack in GetSlimesInAttackRange(slime)){
                    actions.Add(new AIRawSlimeAction(slime.GetId(), ActionType.ATTACK, toAttack.GetId()));
                }
            }
        }
        return actions;
    }

    public List<AIRawSlimeAction> GetMoveActions(RawSlime slime){
        // Devolvemos las acciones de movimiento que puede hacer esa slime
        List<AIRawSlimeAction> actions = new List<AIRawSlimeAction>();
        foreach(TileData tile in GetPossibleMovements(slime)){
            actions.Add(new AIRawSlimeAction(slime.GetId(), ActionType.MOVE, tile.getPosition()));
        }
        return actions;
    }

    public List<AIRawSlimeAction> GetConquerActions(RawSlime slime){
        // Devolvemos la accion de conquerir el terreno sobre el que esta esa slime
        List<AIRawSlimeAction> actions = new List<AIRawSlimeAction>();
        // Si no l'ha conquerit ja, pot conquerirla.
        if(!slime.GetPlayer().GetConqueredTiles().Contains(slime.GetActualTile())) actions.Add(new AIRawSlimeAction(slime.GetId(), ActionType.CONQUER, slime.GetActualTile().getPosition()));
        return actions;
    }

    public List<AIRawSlimeAction> GetSplitActions(RawSlime slime){
        // Devolvemos las acciones de dividirse que puede hacer con esa slime
        List<AIRawSlimeAction> actions = new List<AIRawSlimeAction>();
        if(slime.canSplit){
            foreach(TileData tile in GetSplitRangeTiles(slime)){
                actions.Add(new AIRawSlimeAction(slime.GetId(), ActionType.SPLIT, tile.getPosition()));
            }
        }
        return actions;
    }

    public List<AIRawSlimeAction> GetFusionActions(RawSlime slime){
        // Devolvemos las acciones de fusionarse que puede hacer con esa slime
        List<AIRawSlimeAction> actions = new List<AIRawSlimeAction>();
        foreach(RawSlime sl in GetFusionTargets(slime)){
            actions.Add(new AIRawSlimeAction(slime.GetId(), ActionType.FUSION, sl.GetId()));
        }
        return actions;
    }

    private List<AIRawSlimeAction> GetGrowActions(RawSlime slime){
        List<AIRawSlimeAction> actions = new List<AIRawSlimeAction>();
        if(slime.canGrow) actions.Add(new AIRawSlimeAction(slime.GetId(), ActionType.EAT, slime.GetId()));
        return actions;
    }

    public RawPlayer GetPlayerById(int id){
        foreach(RawPlayer pl in players){
            if(pl.GetId() == id) return pl;
        }
        return null;
    }

    public List<RawPlayer> GetPlayers(){
        return players;
    }

    public int GetDistanceToCloserEnemy(RawSlime slime){
        int distance = Int16.MaxValue;
		Vector2 myPos = slime.GetActualTile().getPosition();
		foreach(RawPlayer p in players){
			if (p != slime.GetPlayer()){
				foreach(RawSlime s in p.GetSlimes()){
					Vector2 slPos = s.GetActualTile().getPosition();		
					distance = Mathf.Min(distance, Matrix.GetDistance(slPos, myPos));
				}
			}
		}
		return distance;
	}
}