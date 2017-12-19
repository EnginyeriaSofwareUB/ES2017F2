using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SimpleJSON;

public class GameController : MonoBehaviour
{
    
    protected Slime selectedSlime;
    public Matrix matrix;
	protected List<Player> players;
	//protected GameObject panelTip, textTip;
	protected int currentTurn;
	protected Player currentPlayer;
	protected int playerActions;
	public Material fire;
	public Material tileMaterial;

	protected Sprite conquerSprite;

	protected GameControllerStatus status = GameControllerStatus.CHECKINGLOGIC;

	protected SoundController soundController;
	protected UIController uiController;
	protected CameraController camController;

	protected ModosVictoria condicionVictoria;
	protected float massToWin;
	protected int totalTiles;
	protected float percentageTilesToWin;
    protected int MAX_TURNS;

    // Use this for initialization
    void Start()
    {
		/*
		TipDialog a = new TipDialog ();
		a.SetButtonImage(SpritesLoader.GetInstance ().GetResource ("Buttons/button_template"));
		a.SetBackgroundImage(SpritesLoader.GetInstance ().GetResource ("Panels/emergent"));
		a.SetInfoTextText ("Has aceptado");
		a.Hide ();
		TwoOptionsDialog t = new TwoOptionsDialog();
		t.SetButtonsImage(SpritesLoader.GetInstance ().GetResource ("Buttons/button_template"));
		t.SetBackgroundImage(SpritesLoader.GetInstance ().GetResource ("Panels/emergent"));
		t.SetAceptButtonColor (new Color(0f,0f,1f));
		t.SetDeclineButtonColor (new Color(1f,0f,0f));
		t.SetOnClickAceptFunction(()=>{
			a.Show();
		});
		*/

        /*
		Time.timeScale = 1;
		ChainTextDialog ctd = new ChainTextDialog ();
		ctd.SetButtonImage(SpritesLoader.GetInstance ().GetResource ("Buttons/button_template"));
		ctd.SetBackgroundImage(SpritesLoader.GetInstance ().GetResource ("Panels/emergent"));
        */

		TileFactory.tileMaterial = tileMaterial;
		//InGameMarker igm = new InGameMarker ();
		//igm.SetSprite (SpritesLoader.GetInstance().GetResource("Test/testTileSlim"));
        FloatingTextController.Initialize ();
        uiController = Camera.main.GetComponent<UIController>();
		soundController = gameObject.GetComponent<SoundController>();
        camController = Camera.main.GetComponent<CameraController>();
        conquerSprite = SpritesLoader.GetInstance().GetResource("Tiles/conquest_flag");
        //panelTip = GameObject.Find("PanelTip"); //ja tenim el panell, per si el necessitem activar, i desactivar amb : panelTip.GetComponent<DialogInfo> ().Active (boolean);
        //textTip = GameObject.Find("TextTip"); //ja tenim el textBox, per canviar el text : textTip.GetComponent<Text> ().text = "Text nou";
        //panelTip.GetComponent<DialogInfo>().Active(false);
        //textTip.GetComponent<Text>().text = "Aquí es mostraran els diferents trucs que pot fer el jugador";
        players = new List<Player>();

        if (ModosVictoria.IsDefined(typeof (ModosVictoria),GameSelection.modoVictoria)){
            condicionVictoria =  (ModosVictoria) GameSelection.modoVictoria;
        }else{
            condicionVictoria = ModosVictoria.ASESINATO; //por defecto
        }

        MAX_TURNS = GameSelection.MAX_TURNS;

        int maxPlayers = GameSelection.playerColors.Count;

		if (maxPlayers == 0) {
			GameSelection.playerColors.Add (new Color (0, 0, 1));
			GameSelection.playerColors.Add (new Color (1, 0, 0));
			GameSelection.playerCores.Add(SlimeCoreTypes.SLOTH);
			GameSelection.playerCores.Add(SlimeCoreTypes.WRATH);
			GameSelection.playerIAs.Add (false);
			GameSelection.playerIAs.Add (true);
			maxPlayers = 2;
		}
		for (int i=0;i<maxPlayers;i++){
            if (GameSelection.playerIAs [i]) {
				//players.Add(new Player("Jugador "+(i+1),StatsFactory.GetStat(GameSelection.playerCores[i])));
				Debug.Log("Ia true");
                players.Add(new Player("Jugador "+(i+1),StatsFactory.GetStat(GameSelection.playerCores[i]),AIManager.GetAIByVictoryCondition(this,condicionVictoria)));
            } else {
                players.Add(new Player("Jugador "+(i+1),StatsFactory.GetStat(GameSelection.playerCores[i])));
            }
            players[i].SetColor(GameSelection.playerColors[i]);
        }
        matrix = GameSelection.map;//new Matrix(11, 0.3f, 1234567);
		if (matrix == null) matrix = new Matrix(20, 0.3f, Random.Range(0,10000));
        MapDrawer.instantiateMap(matrix.getIterable());
        int numSlimesPerPlayer = 2;
        List<List<Vector2>> positions = matrix.GetPositions(players.Count,numSlimesPerPlayer);
        int j = 0;
        foreach(Player player in players){
            List<Vector2> positionsSlimes = positions[j];
            foreach(Vector2 positionSlime in positionsSlimes){
                SlimeFactory.instantiateSlime(player,positionSlime);
            }
            j++;
        }
		if(players.Count == 0){
			players.Add(new Player("Jugador 1", StatsFactory.GetStat(SlimeCoreTypes.WRATH), AIManager.GetAIByVictoryCondition(this,condicionVictoria))); // Test with 2 players
			players.Add(new Player("Jugador 2", StatsFactory.GetStat(SlimeCoreTypes.GLUTTONY)));
			players[0].SetColor(Color.blue);
			players[1].SetColor(Color.red);
			positions = matrix.GetPositions(players.Count,1);
			SlimeFactory.instantiateSlime(players[0],positions[0][0]);
			SlimeFactory.instantiateSlime(players[1],positions[1][0]);

		}
		//matrix = new Matrix(MapParser.ReadMap(MapTypes.Medium));

        currentTurn = 0;
        currentPlayer = players[0];
        playerActions = 0;

		uiController.UpdateRound(currentTurn+1);
		uiController.UpdatePlayer(GetCurrentPlayer().GetColor());

		uiController.UpdateActions(playerActions,GetCurrentPlayer().actions);
		uiController.ShowBothPanels ();
        //iniciem la informacio de game over
        totalTiles = matrix.TotalNumTiles();
        //Debug.Log("TILES TOTALS: "+ totalTiles);
        
        // La condicio de victoria s'assigna mes amunt, aqui nomes s'actualitzen els requisits.
        switch(condicionVictoria){
            case ModosVictoria.CONQUISTA:
                //define percentage tiles to win
                if(MAX_TURNS == 0)
                {
                    percentageTilesToWin = 0.25f;
                }
                else
                {
                    percentageTilesToWin = 0.75f;
                }
                
                //Debug.Log("Porcentaje de conquista para ganar: "+percentageTilesToWin);
                break;
            case ModosVictoria.MASA:
                //define mass to win
                massToWin = 0;
                foreach(Player player in players){
                    if (player.GetTotalMass()>massToWin) massToWin = player.GetTotalMass();
                }
                massToWin*=2;
                //Debug.Log("Masa total del jugador para ganar: "+massToWin);
                break;
        }

        GameOverInfo.Init();
        AudioClip clip = SoundsLoader.GetInstance().GetResource("Sounds/MainGameTheme");
        soundController.PlayLoop(clip);
		camController.InitMaxZoom();


        status = GameControllerStatus.CHECKINGLOGIC;
    }

    // Update is called once per frame
    void Update()
    {
		if (status == GameControllerStatus.CHECKINGLOGIC) {
			checkLogic ();
		}

		//S'ha de posar despres de la comprovacio de ended
		// Si estamos en modo "espera accion" y el jugador es una IA, calculamos la accion.
		if (currentPlayer.isPlayerAI())
		{
			if(status == GameControllerStatus.WAITINGFORACTION){
				status = GameControllerStatus.AILOGIC;
				GetCurrentPlayer().ThinkAction();
			}else if(status == GameControllerStatus.AILOGIC && !GetCurrentPlayer().IsThinking()){
				status = GameControllerStatus.PLAYINGACTION;
				AISlimeAction action = currentPlayer.GetThoughtAction();
				if(action != null){
					SetSelectedSlime(action.GetMainSlime()); // Simulamos la seleccion de la slime que hace la accion.
					DoAction((SlimeAction)action); // Hacemos la accion.
				}else {
					Debug.Log("IA returned NULL action");
					NextPlayer(); // No pot fer cap accio
				}
			}
		}
    }

	public void checkLogic(){
		for (int i = players.Count-1;i>=0;i--){
            if (players[i].GetNumSlimes() == 0)
            {
                //This player loses
                GameOverInfo.SetLoser(players[i]);
				//si li tocava al que ha mort i aquest era l'ultim de la llista, el torn es el del primer de la llista
                players.RemoveAt(i); //definitivament el borrem de la llista
				break;
            }
        }

        Player winner = IsGameEndedAndWinner();
        if (winner!=null)
        {
            GameOverInfo.SetWinner(winner);
			foreach(Player p in players){
				if(p!=winner) GameOverInfo.SetLoser(p);
			}
			GameSelection.playerColors.Clear ();
			GameSelection.playerCores.Clear ();
			GameSelection.playerIAs.Clear ();
			//players.Clear ();
            SceneManager.LoadScene("GameOver");
        }

		if (playerActions >= currentPlayer.actions) {
			NextPlayer ();
		} else {
			status = GameControllerStatus.WAITINGFORACTION;
		}
	}

	public GameControllerStatus getStatus(){
		return status;
	}

	public void updateStatus(GameControllerStatus s){
		status = s;
	}

    /*
	Funció que retorna True si s'ha acabat la partida o False si no.
	 */
	protected virtual Player IsGameEndedAndWinner()
    {
        Player ret = null; //si no trobem guanyador retornem null
        int index;
        bool find;
        //sempre que no estiguem en un reto comprovem la condicio de asesinato
        if (players.Count == 1 && MAX_TURNS == 0){
            return players[0];
        }
        if(MAX_TURNS != 0 && currentTurn >= MAX_TURNS)
        {
            return new Player("0", null);
        }
        //return currentTurn >= MAX_TURNS || players.Count == 1; //Player who wins
        switch(condicionVictoria){
            case ModosVictoria.CONQUISTA:
                find = false;
                index = 0;
                while (!find && index<players.Count){
                    if (players[index].NumConqueredTiles()/percentageTilesToWin>=totalTiles){
                        find = true;
                        ret = players[index];
                    }
                    index++;
                }
                break;

            case ModosVictoria.MASA:
                find = false;
                index = 0;
                while (!find && index<players.Count){
                    if (players[index].GetTotalMass()>=massToWin){
                        find = true;
                        ret = players[index];
                    }
                    index++;
                }
                break;
            case ModosVictoria.ASESINATO: //necessari per als retos de assassinat
                if(players.Count == 1)
                {
                    ret = players[0];
                }
                break;
        }
        return ret; //si no troba guanyador retornem null, si hi ha guanyador retornem el guanyador

    }
	
    /*
	Funció que avança al seguent jugador.
	 */
    private void NextPlayer()
    {
		selectedSlime = null;
		int playerIndex = players.IndexOf(currentPlayer);
		playerIndex++;
        currentPlayer = players[playerIndex%players.Count];
        playerActions = 0;
		
        
		if (playerIndex >= players.Count) {
			// Tots els jugadors han fet la seva accio, passem al seguent torn.
			NextTurn ();
		} else {
			status = GameControllerStatus.PLAYINGACTION;
			uiController.NextPlayer(GetCurrentPlayer().GetColor(),playerActions,GetCurrentPlayer().actions);
		}
		camController.AllTilesInCamera(GetPossibleMovements(currentPlayer.GetSlimes()));
		
		//camController.GlobalCamera();
		//Debug.Log("SLIMES: " + players [currentPlayer].GetSlimes ().Count);
    }

    /*
	Funció que avança al seguent torn.
	 */
    public void NextTurn()
    {
        currentPlayer = players[0];
        playerActions = 0;
        currentTurn++;
		status = GameControllerStatus.PLAYINGACTION;
		uiController.NextRound (currentTurn + 1, GetCurrentPlayer ().GetColor (), playerActions, GetCurrentPlayer ().actions);
    }

    public Slime GetSelectedSlime()
    {
        return selectedSlime;
    }

	public void SetSelectedSlime(Slime s){
		selectedSlime = s;
	}

	public Player GetCurrentPlayer(){
		return currentPlayer;
	}

	public void DoAction(SlimeAction action){
		if (action == null) {
			return;
		}
		switch(action.GetAction()) {
		case ActionType.ATTACK:
			AttackSlime (action.GetSlime());
			break;
		case ActionType.CONQUER:
			//Debug.Log("CONQUER");
			ConquerTile (action.GetTile());
			break;
		case ActionType.SPLIT:
			//Debug.Log("SPLIT");
			SplitSlime (action.GetTile());
			break;
		case ActionType.EAT:
			GrowSlime (action.GetSlime());
			break;
		case ActionType.MOVE:
			MoveSlime (action.GetTile());
			break;
		case ActionType.FUSION:
			//Debug.Log("FUSION");
			FusionSlime (action.GetSlime());
			break;
		}
		SetSelectedSlime(null);
		uiController.UpdateActions(playerActions,GetCurrentPlayer().actions);
	}

    private void MoveSlime(Tile tile)
    {
		TileData tileTo = tile.GetTileData ();
        /*if(tileTo.GetSlimeOnTop() != null) {
            Debug.Log("WARNING: trying to move to tile with a slime");
            Debug.Log("ID:" + tileTo.GetSlimeOnTop().GetId());
        }
        if(tileTo.getTileType() == TileType.Null) Debug.Log("WARNING: trying to move to BLOCK");
        if(Matrix.GetDistance(selectedSlime.GetActualTile().getPosition(), tileTo.getPosition()) > selectedSlime.GetMovementRange())
            Debug.Log("WARNING: trying to move to tile too far");*/

		Dictionary<TileData,List<TileData>> moves = matrix.possibleCoordinatesAndPath(
			(int)selectedSlime.actualTile.getPosition().x, (int)selectedSlime.actualTile.getPosition().y, selectedSlime.GetMovementRange());
        
        //if(moves[tileTo] == null) Debug.Log("WARNING!!!\n" + moves.Keys);
		List<TileData> path = moves[tileTo];
		path [path.Count-1].SetSlimeOnTop (selectedSlime);
		selectedSlime.SetActualTile (tile);
		selectedSlime.gameObject.GetComponent<SlimeMovement>().SetBufferAndPlay(path);

		status = GameControllerStatus.PLAYINGACTION;
		playerActions++;
    }

	private void SplitSlime(Tile targetTile){
		if (selectedSlime.canSplit) {
			Slime newSlime = SlimeFactory.instantiateSlime (selectedSlime.GetPlayer (), new Vector2 (targetTile.GetTileData ().getPosition ().x, targetTile.GetTileData ().getPosition ().y));
			newSlime.InitMass(); //posem vida a 0, i a la seguent linia li posem la vida real, d'aquesta manera es veu el popup amb '+'
			newSlime.SetMass ((int)(selectedSlime.GetMass () / 2.0f),true);
			selectedSlime.SetMass ((int)(selectedSlime.GetMass () / 2.0f),true);
			newSlime.ChangeElement (selectedSlime.GetElementType());
			playerActions++;
			status = GameControllerStatus.CHECKINGLOGIC;
		}
    }

	private void AttackSlime(Slime targetSlime){
		if (selectedSlime.canAttack) {
			playerActions++;
			RangedAttack (targetSlime);
		}
	}

	private void RangedAttack(Slime toAttack){
        GameObject projectile = new GameObject("projectile");
		Sprite sprite = GetSpriteForElement(selectedSlime.GetElementType(), projectile);
        projectile.AddComponent<ProjectileTrajectory>();
        projectile.AddComponent<SpriteRenderer>().sprite = sprite;
        projectile.GetComponent<SpriteRenderer>().sortingLayerName = "Slime";
		//projectile.GetComponent<SpriteRenderer> ().color = selectedSlime.GetPlayer ().GetColor ();
        projectile.GetComponent<ProjectileTrajectory>().SetTrajectorySlimes(selectedSlime, toAttack);
		/*
		Vector3 horizontal = new Vector3 (1f, 0f, 0f);
		if (MathUtils.Vect3ScalarProduct(horizontal,(toAttack.transform.parent.position - selectedSlime.transform.parent.position)) >= 0) {
			Debug.Log ("positive");
			projectile.transform.Rotate (new Vector3 (0f, 0f, Vector3.Angle (horizontal, toAttack.transform.parent.position - selectedSlime.transform.parent.position)));
		} else {
			Debug.Log ("negative");
			projectile.transform.Rotate (new Vector3 (0f, 0f, -Vector3.Angle (horizontal, toAttack.transform.parent.position - selectedSlime.transform.parent.position)));
		}
		*/
		projectile.transform.Rotate (new Vector3 (0f, 0f, Vector3.SignedAngle (new Vector3(1f,0f,0f), (toAttack.transform.parent.position - selectedSlime.transform.parent.position),new Vector3(0f,0f,1f))));

		status = GameControllerStatus.PLAYINGACTION;
        AudioClip clip = SoundsLoader.GetInstance().GetResource("Sounds/fireball");
        soundController.PlaySingle(clip);
    }

	public Sprite GetSpriteForElement(ElementType type,GameObject p){
		Sprite sp;
		p.transform.localScale = new Vector3 (0.7f, 0.7f, 0f);
		switch (type) {
		case ElementType.EARTH:
			p.transform.localScale = new Vector3 (0.25f, 0.25f, 0f);
			sp = SpritesLoader.GetInstance ().GetResource ("Projectiles/earth_projectile");
			break;
		case ElementType.FIRE:
			p.transform.localScale = new Vector3 (0.9f, 0.9f, 0f);
			sp = SpritesLoader.GetInstance ().GetResource ("Projectiles/fire_projectile");
			break;
		case ElementType.LAVA:
			p.transform.localScale = new Vector3 (0.3f, 0.3f, 0f);
			sp = SpritesLoader.GetInstance ().GetResource ("Projectiles/lava_projectile");
			break;
		case ElementType.MUD:
			p.transform.localScale = new Vector3 (0.3f, 0.3f, 0f);
			sp = SpritesLoader.GetInstance ().GetResource ("Projectiles/mud_projectile");
			break;
		case ElementType.NONE:
			sp = SpritesLoader.GetInstance ().GetResource ("Projectiles/water_projectile");
			break;
		case ElementType.STEAM:
			p.transform.localScale = new Vector3 (0.3f, 0.3f, 0f);
			sp = SpritesLoader.GetInstance ().GetResource ("Projectiles/steam_projectile");
			break;
		case ElementType.WATER:
			p.transform.localScale = new Vector3 (0.9f, 0.9f, 0f);
			sp = SpritesLoader.GetInstance ().GetResource ("Projectiles/water_projectile");
			break;
		default:
			sp = SpritesLoader.GetInstance ().GetResource ("Projectiles/water_projectile");
			break;
		}
		return sp;
	}

	private void FusionSlime(Slime fusionTarget){
		RemoveSlime(selectedSlime);
        //players[currentPlayer].updateActions();
		selectedSlime.GetActualTile ().SetSlimeOnTop (null);
		fusionTarget.SetMass ((int)(selectedSlime.GetMass() + fusionTarget.GetMass()),true);

		Destroy (selectedSlime.gameObject);
		playerActions++;
		status = GameControllerStatus.CHECKINGLOGIC;
	}

	private void ConquerTile(Tile tile){
		tile.tileConquerLayer.sprite = conquerSprite;
		tile.tileConquerLayer.transform.localScale = new Vector3(0.3f, 0.3f, 0f);
		tile.tileConquerLayer.transform.localPosition = new Vector3(-2.4f, 3.5f, 0f);
        //borrem la tile conquerida de qui la tenia abans
        foreach(Player player in players){
            if (player.HasConqueredTile(tile)) player.RemoveConqueredTile(tile);
        }
        //afegim la tile conquerida
        currentPlayer.AddConqueredTile(tile);
		Color c = selectedSlime.GetPlayer ().GetColor ();
		//c.a = 0.5f;
		tile.tileConquerLayer.color = c;
		tile.SetOwner (currentPlayer);
		playerActions++;
		selectedSlime.ChangeElement (tile.elementType);
		tile.RemoveElement ();
		status = GameControllerStatus.CHECKINGLOGIC;
	}

	private void GrowSlime(Slime slime){
		selectedSlime.GrowSlime();
		playerActions++;
		status = GameControllerStatus.CHECKINGLOGIC;
	}
    
	public void RemoveSlime(Slime slimeToRemove){
		foreach (Player player in players){
			if (player.IsSlimeOwner(slimeToRemove)) player.RemoveSlime(slimeToRemove);
            //player.updateActions();
            }
	}

	public void OnProjectileAnimationEnded(){
		status = GameControllerStatus.CHECKINGLOGIC;
	}
	public List<Tile> GetPossibleMovements(List<Slime> slimes){
		List<Tile> tiles = new List<Tile>();
		foreach(Slime slime in slimes){
			tiles.AddRange(GetPossibleMovements(slime));
			tiles.AddRange(GetSlimesInAttackRange(slime));			
		}
		//debug
		//uiController.hideCurrentUITiles ();
		//uiController.markTiles(tiles, ActionType.MOVE);
		return tiles;
	}
	public virtual List<Tile> GetPossibleMovements(Slime slime){
		ArrayList tiles = new ArrayList();
		ArrayList distance = new ArrayList ();
		List<Tile> visited = new List<Tile> ();

		//List<Tile> moveTiles = new List<Tile> ();

		int moveRange = slime.GetMovementRange ();

		List<Vector2> directions = new List<Vector2> {
			new Vector2 (0, -1),new Vector2 (1, -1), new Vector2 (1, 0),new Vector2 (0, 1),new Vector2 (-1,1),new Vector2 (-1, 0)
		};

		tiles.Add (slime.GetActualTile ());
		distance.Add (0);
		int counter = 0;
		while(tiles.Count>0){
			Tile t = (Tile) tiles[0];
			tiles.RemoveAt (0);
			int prevD = (int) distance[0];
			distance.RemoveAt (0);
			counter++;
			visited.Add (t);
			foreach(Vector2 vec in directions){
				int x = (int) (t.getPosition ().x + vec.x);
				int y = (int) (t.getPosition ().y + vec.y);
				Tile newT = MapDrawer.GetTileAt(x ,y);
				if (!visited.Contains(newT) && !tiles.Contains(newT) && newT!=null && prevD+1<=moveRange && !newT.GetTileData().isBlocking()) {
					tiles.Add(newT);
					distance.Add (prevD + 1);
				}
			}
		}
		visited.RemoveAt(0);
		return visited;
	}

	public virtual List<Tile> GetSlimesInAttackRange(Slime slime){
		List<Tile> canAttack = new List<Tile> ();
		Vector2 myPos = slime.GetActualTile().getPosition();
		foreach(Player p in players){
			if (p != slime.GetPlayer()){
				foreach(Slime s in p.GetSlimes()){
					Vector2 slPos = s.GetActualTile().getPosition();		
					if (Matrix.GetDistance(slPos, myPos) <= slime.GetAttackRange()){
						canAttack.Add(s.actualTile);
					}
				}
			}
		}
		return canAttack;
	}

	public virtual List<Tile> GetSplitRangeTiles(Slime slime){
		List<Tile> splitTiles = new List<Tile> ();
		foreach (TileData td in matrix.getNeighbours (slime.GetTileData(), true)) {
			if(td.getTile().GetSlimeOnTop() == null)
				splitTiles.Add (td.getTile());
		}
		return splitTiles;
	}

	public List<Slime> GetFusionTargets(Slime slime){
		List<Slime> fusionSlimes = new List<Slime> ();
		foreach (TileData tile in matrix.getNeighbours(slime.GetTileData(), true))
        {
            Slime overSlime = tile.GetSlimeOnTop();
            if (overSlime != null && overSlime.GetPlayer() == slime.GetPlayer())
            {
                fusionSlimes.Add(overSlime);
            }
        }
		return fusionSlimes;
	}

	public virtual List<Tile> GetJoinTile(Slime slime){
		List<Vector2> directions = new List<Vector2> {
			new Vector2 (0, -1),new Vector2 (1, -1), new Vector2 (1, 0),new Vector2 (0, 1),new Vector2 (-1,1),new Vector2 (-1, 0)
		};
		List<Tile> tileList = new List<Tile>();
		foreach (Vector2 v in directions) {
			int x = (int) (slime.actualTile.getPosition ().x + v.x);
			int y = (int) (slime.actualTile.getPosition ().y + v.y);
			if (MapDrawer.GetTileAt (x, y) != null &&
			    MapDrawer.GetTileAt (x, y).GetSlimeOnTop () != null &&
			    MapDrawer.GetTileAt (x, y).GetSlimeOnTop ().GetPlayer () == slime.GetPlayer ()) {
				tileList.Add (MapDrawer.GetTileAt (x,y));
			}
				
		}
		return tileList;
	}

    public AIGameState GetGameState(){
        Matrix rawMatrix = matrix.GetRawCopy();

        List<RawPlayer> rawPlayers = new List<RawPlayer>();
        foreach(Player pl in players){
            RawPlayer rawPl = pl.GetRawCopy();
            rawPlayers.Add(rawPl);
            // Sync dels conqueredtiles
            foreach(Tile tile in pl.GetConqueredTiles()){
                TileData tiledata = tile.GetTileData();
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
                //Debug.Log("SLIME " + sl.GetId() + " ON (" + matrixTile.getPosition().x + " " + matrixTile.getPosition().y + ")");
			}
		}

        /*if(matrix.EqualsTo(rawMatrix)){
            Debug.Log("MATRIX COPIED CORRECTLY");
        }*/
        
        switch(condicionVictoria){
			case ModosVictoria.CONQUISTA:
			return new AIGameState(condicionVictoria, rawMatrix, rawPlayers, currentTurn, players.IndexOf(currentPlayer), playerActions, percentageTilesToWin);
			case ModosVictoria.MASA:
			return new AIGameState(condicionVictoria, rawMatrix, rawPlayers, currentTurn, players.IndexOf(currentPlayer), playerActions, massToWin);
			default:
			return new AIGameState(condicionVictoria, rawMatrix, rawPlayers, currentTurn, players.IndexOf(currentPlayer), playerActions, 0);
		} 
    }

	public void ApplyDamage(Slime attacker,Slime defender){
		int damage = attacker.getDamage;
		attacker.ChangeMass (attacker.selfDamage);
		defender.ChangeMass ((int)(-damage*(1-defender.damageReduction)));
		//FloatingTextController.CreateFloatingText (((int)-damage).ToString(),defender.transform);
	}

    public Slime FindSlimeById(int id){
        foreach(Player pl in players){
            foreach(Slime sl in pl.GetSlimes()){
                if(sl.CheckId(id)) return sl;
            }
        }
        return null;
    }
    
    void OnDestroy(){
        foreach(Player pl in players){
            if(pl.isPlayerAI() && pl.IsThinking()) pl.StopThinking();
        }
    }

}
