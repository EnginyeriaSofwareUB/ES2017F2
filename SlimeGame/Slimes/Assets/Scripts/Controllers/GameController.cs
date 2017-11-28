using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SimpleJSON;

public class GameController : MonoBehaviour
{


    private const int MAX_TURNS = 10;
    private Slime selectedSlime;
    public Matrix matrix;
    private List<Player> players;
    private GameObject panelTip, textTip;
    private int currentTurn;
    private int currentPlayer;
    private int playerActions;
	public Material fire;

	private Sprite conquerSprite;

	public List<Slime> allSlimes;

	private GameControllerStatus status;

	public GameObject healthBar;

    public int tutorial;
    private List<string> tutorialTexts;
    private int textTutorialPosition;
    
	public Material tileMaterial;

    private UIController uiController;
    // Use this for initialization
    void Start()
    {
        textTutorialPosition = 0;
        tutorial = 1;
        FloatingTextController.Initialize ();
        uiController = Camera.main.GetComponent<UIController>();
		FloatingTextController.Initialize ();
		string stats = (Resources.Load ("slimeCoreStats") as TextAsset).text;
		List<SlimeCoreData> cores = new List<SlimeCoreData> ();
		JSONNode n = JSON.Parse (stats);
		allSlimes = new List<Slime> ();
		for (int i = 0; i < n.Count; i++) {
			JSONNode slime = n[i.ToString()];
			SlimeCoreData slimeData = new SlimeCoreData (
				slime
			);
			cores.Add (slimeData);
		}

        if(tutorial == 1)
        {
            string textTutorial = (Resources.Load("textTutorial") as TextAsset).text;
            tutorialTexts = new List<string>();
            JSONNode s = JSON.Parse(textTutorial);
            for (int i = 0; i < s.Count; i++)
            {
                JSONNode text = s[i.ToString()];
                tutorialTexts.Add(text["text"]);
            }
        }



        conquerSprite = SpritesLoader.GetInstance().GetResource("Test/conquerTile");
        //MapDrawer.InitTest ();
		status = GameControllerStatus.WAITINGFORACTION;
        panelTip = GameObject.Find("PanelTip"); //ja tenim el panell, per si el necessitem activar, i desactivar amb : panelTip.GetComponent<DialogInfo> ().Active (boolean);
        textTip = GameObject.Find("TextTip"); //ja tenim el textBox, per canviar el text : textTip.GetComponent<Text> ().text = "Text nou";
        panelTip.GetComponent<DialogInfo>().Active(false);
        textTip.GetComponent<Text>().text = "Aquí es mostraran els diferents trucs que pot fer el jugador";
        players = new List<Player>();

        if (tutorial == 1)
        {
            panelTip.GetComponent<DialogInfo>().Active(true);
            ShowTutorialTip();
            players.Add(new Player("Jugador 1", 1, cores[3]));
            players.Add(new Player("IA", 1, cores[4]));
            players[0].SetColor(GameSelection.player1Color); //Perque el set color esta fora del constructor si no funciona el instantiate slime sense aixo
            players[1].SetColor(GameSelection.player2Color);
            matrix = new Matrix(11, 0.3f, 1234567);
            MapDrawer.instantiateMap(matrix.getIterable());
            instantiateSlime(cores[3], players[0], 3, -4);
            instantiateSlime(cores[4], players[1], -4, 1);
            players[1].SetBrain(new TutorialIA());
            players[0].setTutorialActions();
        }
        else
        {
            players.Add(new Player("Jugador 1", 2, cores[GameSelection.player1Core])); // Test with 2 players
            players.Add(new Player("Jugador 2", 2, cores[GameSelection.player2Core]));
            players[0].SetColor(GameSelection.player1Color);
            players[1].SetColor(GameSelection.player2Color);
            matrix = GameSelection.map;//new Matrix(11, 0.3f, 1234567);
            if (matrix == null) matrix = new Matrix(11, 0.3f, 1234567);
            MapDrawer.instantiateMap(matrix.getIterable());
            Vector2 slime1 = matrix.GetRandomTile();
            instantiateSlime(cores[0], players[0], (int)slime1.x, (int)slime1.y);
            Vector2 slime2 = matrix.GetRandomTile();
            instantiateSlime(cores[0], players[0], (int)slime2.x, (int)slime2.y);
            Vector2 slime3 = matrix.GetRandomTile();
            instantiateSlime(cores[1], players[1], (int)slime3.x, (int)slime3.y);
            Vector2 slime4 = matrix.GetRandomTile();
            instantiateSlime(cores[1], players[1], (int)slime4.x, (int)slime4.y);
        }

		//matrix = new Matrix(MapParser.ReadMap(MapTypes.Medium));

        currentTurn = 0;
        currentPlayer = 0;
        playerActions = 0;
		foreach(Player p in players){
            p.updateActions();
            foreach (Slime s in p.GetSlimes()) {
				allSlimes.Add (s);
			}
		}

        //iniciem la informacio de game over
        GameOverInfo.Init();

    }

    // Update is called once per frame
    void Update()
    {
		if (status == GameControllerStatus.CHECKINGLOGIC) {
			checkLogic ();
		}

        foreach (Player player in players)
        {
            if (player.GetNumSlimes() == 0)
            {
                //This player loses
                GameOverInfo.SetLoser(player);
                players.Remove(player);
            }
        }

        bool ended = IsGameEnded();

        if (ended)
        {
            GameOverInfo.SetWinner(players[0]);
            SceneManager.LoadScene("GameOver");
        }

        if (players [currentPlayer].isPlayerAI () && playerActions < players[currentPlayer].GetActions() && !ended) {
			DoAction (players [currentPlayer].GetAction (this));
            //Hi ha bugs si trec aquesta linea
            selectedSlime = null;
        }
    }

	public void checkLogic(){
		if (playerActions >= players [currentPlayer].GetActions ()) {
			currentPlayer++;
            foreach (Player pl in players) {
                pl.updateActions();
            }
            if (currentPlayer >= players.Count) {
				currentPlayer = 0;
				currentTurn++;
			}
            //uiController.ChangeCamera(players[currentPlayer].GetSlimes());
			playerActions = 0;
            if(currentPlayer == 0 && tutorial == 1)
            {
                MarkAndShowInfoTutorial();
            }
		}
		status = GameControllerStatus.WAITINGFORACTION;
	}

    void OnGUI()
    {
        if (players != null)
        {
            GUI.Label(new Rect(10, 10, 100, 20), "TURN:  " + (currentTurn + 1).ToString());
            GUI.Label(new Rect(10, 30, 200, 40), "PLAYER:  " + getCurrentPlayer().GetName());
            GUI.Label(new Rect(10, 50, 200, 40), "ACTIONS:  " + (getCurrentPlayer().GetActions() - playerActions));
        }
    }

	public GameControllerStatus getStatus(){
		return status;
	}

	public void updateStatus(GameControllerStatus s){
		status = s;
	}

    /*
    Retorna el jugador que li toca fer una acció.
     */
    private Player getCurrentPlayer()
    {
        return players[currentPlayer % players.Count];
    }

    /*
	Funció que retorna True si s'ha acabat la partida o False sino.
	 */
    private bool IsGameEnded()
    {
        return currentTurn >= MAX_TURNS || players.Count == 1; //Player who wins
    }

    /*
    Funció que comprova si hi ha accions suficients i si n'hi ha les utilitza.
    TODO no s'utilitza
     */
    private bool UseActions(int numberOfActions)
    {
        if (playerActions + numberOfActions > getCurrentPlayer().GetActions()) return false; // Accions insuficients

        playerActions += numberOfActions;

        if (playerActions >= getCurrentPlayer().GetActions())
        {
            NextPlayer();
        }

        return true;
    }

    public void PrepareAction(ActionType action)
    {
        /*
        if (selectedSlime.tag.Equals("Slime"))
        {
            Slime slime = selectedSlime.GetComponent<Slime>();
            switch (action)
            {
                case ActionType.Move:
					break;
                case ActionType.Attack:
                    break;
                case ActionType.Divide:
                    MapDrawer.ShowDivisionRange(slime, matrix);
                    break;
                case ActionType.Fusion:
                    MapDrawer.ShowFusionRange(slime, matrix);
                    break;
                default:
                    UseActions(1);
                    break;
            }
        }
         */
    }

    public void PrepareAction(int action)
    {
        PrepareAction((ActionType)action);
    }


    /*
	Funció que avança al seguent jugador.
    TODO no se usa
	 */
    private void NextPlayer()
    {
        currentPlayer++;
        playerActions = 0;
        //uiController.ChangeCamera(players[currentPlayer].GetSlimes());
        if (currentPlayer >= players.Count)
        {
            // Tots els jugadors han fet la seva accio, passem al seguent torn.
            NextTurn();
            
        }
    }

    /*
	Funció que avança al seguent torn.
	 */
    public void NextTurn()
    {
        if (tutorial != 1)
        {
            currentPlayer = 0;
            playerActions = 0;
            currentTurn++;
        }
    }

    private Slime instantiateSlime(SlimeCoreData core, Player pl, int x0, int y0)
    {

        GameObject slime = new GameObject("Slime " + (pl.GetNumSlimes() + 1).ToString() + " - " + pl.GetName());
        slime.AddComponent<SpriteRenderer>();
        slime.tag = "Slime";
        slime.AddComponent<Slime>();
		slime.GetComponent<SpriteRenderer>().sprite = SpritesLoader.GetInstance().GetResource(core.picDirection+0);
        slime.GetComponent<SpriteRenderer>().sortingLayerName = "Slime";
        slime.AddComponent<BoxCollider2D>();
        slime.AddComponent<SlimeMovement>();

		pl.AddSlime(slime.GetComponent<Slime>());

		slime.GetComponent<Slime> ().changeScaleSlime ();
		Tile tile = MapDrawer.GetTileAt(x0, y0);
		Vector2 tileWorldPosition = tile.GetTileData().GetRealWorldPosition();//MapDrawer.drawInternCoordenates(new Vector2(x0, y0));
        slime.transform.position = new Vector3(tileWorldPosition.x, tileWorldPosition.y, 0f);
        slime.GetComponent<Slime>().SetActualTile(tile);
        slime.GetComponent<Slime>().setPlayer(pl);
		//CONFIGURACIO BARRA VIDA PER SLIME
		//afegim canvas al gameObject per despres posar les imatges de la barra de vida
		GameObject newCanvas = new GameObject("Canvas");
		newCanvas.layer = 8;
		Canvas c = newCanvas.AddComponent<Canvas>();
		c.renderMode = RenderMode.WorldSpace;
		//es el fill del slime 
		newCanvas.transform.SetParent (slime.transform);
		RectTransform rect = newCanvas.GetComponent<RectTransform> ();
		//posicion del canvas, dins hi haura la barra de vida
		rect.localPosition = new Vector3 (0f,1f,0f);
		rect.sizeDelta = new Vector2 (1.5f,0.25f);

        return slime.GetComponent<Slime> ();
    }

    public Slime GetSelectedSlime()
    {
        return selectedSlime;
    }

	public void SetSelectedSlime(Slime s){
		selectedSlime = s;
	}

	public Player GetCurrentPlayer(){
		return players [currentPlayer];
	}

	public void DoAction(SlimeAction action){
		if (action == null) {
			return;
		}
        if(tutorial == 1 && currentPlayer == 0)
        {
            Player pl = players[0];
            if (!pl.isTutorialAction(action, selectedSlime))
            {
                return;
            }
            else
            {
                UnmarkTiles();
                if(playerActions < pl.GetActions() -1)
                {
                    MarkAndShowInfoTutorial();
                }
            }
        }
		switch(action.GetAction()) {
		case ActionType.ATTACK:
			AttackSlime (action.GetSlime());
			break;
		case ActionType.CONQUER:
			ConquerTile (action.GetTile());
			break;
		case ActionType.SPLIT:
			SplitSlime (action.GetTile());
			break;
		case ActionType.EAT:
			break;
		case ActionType.MOVE:
			MoveSlime (action.GetTile());
			break;
		case ActionType.FUSION:
			FusionSlime (action.GetSlime());
			break;
		}
	}

    private void MoveSlime(Tile tile)
    {
		TileData tileTo = tile.GetTileData ();
		//Debug.Log("userHitOnTile");
		//TODO: Refactor this to only search one path
		Dictionary<TileData,List<TileData>> moves = matrix.possibleCoordinatesAndPath(
			(int)selectedSlime.actualTile.getPosition().x, (int)selectedSlime.actualTile.getPosition().y, selectedSlime.GetMovementRange());

		List<TileData> path = moves[tileTo];
		path [path.Count-1].SetSlimeOnTop (selectedSlime.gameObject);
		selectedSlime.SetActualTile (tile);
		selectedSlime.gameObject.GetComponent<SlimeMovement>().SetBufferAndPlay(path);

		selectedSlime.gameObject.GetComponent<Slime>().rangeUpdated = false;
		status = GameControllerStatus.PLAYINGACTION;
		playerActions++;
    }

	private void SplitSlime(Tile targetTile){
		Slime newSlime = instantiateSlime(selectedSlime.GetPlayer().slimeCoreData, selectedSlime.GetPlayer(), (int) targetTile.GetTileData().getPosition().x, (int) targetTile.GetTileData().getPosition().y);
		//players [currentPlayer].AddSlime (newSlime);
		allSlimes.Add (newSlime);
		targetTile.SetSlimeOnTop (newSlime.gameObject);
		newSlime.SetActualTile (targetTile);
		newSlime.SetMass (selectedSlime.GetMass()/2.0f);
		selectedSlime.SetMass (selectedSlime.GetMass() / 2.0f);
		playerActions++;
		status = GameControllerStatus.CHECKINGLOGIC;
    }

	private void AttackSlime(Slime targetSlime){
		playerActions++;
		status = GameControllerStatus.CHECKINGLOGIC;
        RangedAttack(targetSlime);
	}

	private void RangedAttack(Slime toAttack)
    {
        GameObject projectile = new GameObject("projectile");
		Sprite sprite = SpritesLoader.GetInstance().GetResource("Sprites/Proj");
        projectile.AddComponent<ProjectileTrajectory>();
        projectile.AddComponent<SpriteRenderer>().sprite = sprite;
        projectile.GetComponent<SpriteRenderer>().sortingLayerName = "SlimeBorder";
		projectile.GetComponent<SpriteRenderer> ().color = selectedSlime.GetPlayer ().GetColor ();
        projectile.GetComponent<Transform>().localScale = new Vector3(0.3f, 0.3f, 1f);
        projectile.GetComponent<ProjectileTrajectory>().SetTrajectorySlimes(selectedSlime, toAttack);
    }

	private void FusionSlime(Slime fusionTarget)
	{
		players [currentPlayer].GetSlimes ().Remove(selectedSlime);
        players[currentPlayer].updateActions();
		allSlimes.Remove(selectedSlime);
		selectedSlime.GetActualTile ().SetSlimeOnTop (null);
		fusionTarget.SetMass (selectedSlime.GetMass() + fusionTarget.GetMass());

		Destroy (selectedSlime.gameObject);
		playerActions++;
		status = GameControllerStatus.CHECKINGLOGIC;
	}

	private void ConquerTile(Tile tile){
		tile.tileConquerLayer.sprite = conquerSprite;
		Color c = selectedSlime.GetPlayer ().GetColor ();
		c.a = 0.5f;
		tile.tileConquerLayer.color = c;
		playerActions++;
		status = GameControllerStatus.CHECKINGLOGIC;
	}

    //nomes cridar quan sigui torn les player 0 en el tutorial
    private void MarkAndShowInfoTutorial()
    {
        Player pl = players[0];
        ShowTutorialTip();
        if (pl.RightSlime(pl.GetSlimes()[0]))
        {
            MarkTile(pl.GetSlimes()[0].actualTile);
        }
        else
        {
            MarkTile(pl.GetSlimes()[1].actualTile);
        }
        if (pl.nextAction().GetAction() == ActionType.MOVE || pl.nextAction().GetAction() == ActionType.ATTACK
            || pl.nextAction().GetAction() == ActionType.SPLIT)
        {
            MarkTile((Tile)pl.nextAction().GetData());
        }
    }

    private void ShowTutorialTip()
    {
        if (textTutorialPosition < tutorialTexts.Count)
        {
            textTip.GetComponent<Text>().text = tutorialTexts[textTutorialPosition];
            textTutorialPosition++;
        }
    }

    private void UnmarkTiles()
    {
        //Aqui es desmarcaran totes les casselles marcades
    }
    private void MarkTile(Tile tile)
    {
        //Aqui es marxara la tile que entra per parametre
        //Debug.Log(tile);
    }

	public void RemoveSlime(Slime slimeToRemove){
		allSlimes.Remove (slimeToRemove);
		foreach (Player player in players){
			if (player.GetSlimes().Contains(slimeToRemove)) player.GetSlimes().Remove(slimeToRemove);
            player.updateActions();
		}
	}
}
