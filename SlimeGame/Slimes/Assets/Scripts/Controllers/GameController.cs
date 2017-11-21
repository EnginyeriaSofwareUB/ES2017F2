using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SimpleJSON;

public class GameController : MonoBehaviour
{

    private const int MAX_TURNS = 5;

    private Slime selectedSlime;
    public Matrix matrix;
    private List<Player> players;
    private GameObject panelTip, textTip;
    private int currentTurn;
    private int currentPlayer;
    private int playerActions;
	public Sprite healthBarImage;

	private Sprite conquerSprite;

	public List<Slime> allSlimes;

	private GameControllerStatus status;

	public GameObject healthBar;
	public Material tileMaterial;

    // Use this for initialization
    void Start()
    {

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
		conquerSprite = Resources.Load<Sprite> ("Test/conquerTile");
        //MapDrawer.InitTest ();
		status = GameControllerStatus.WAITINGFORACTION;
        panelTip = GameObject.Find("PanelTip"); //ja tenim el panell, per si el necessitem activar, i desactivar amb : panelTip.GetComponent<DialogInfo> ().Active (boolean);
        textTip = GameObject.Find("TextTip"); //ja tenim el textBox, per canviar el text : textTip.GetComponent<Text> ().text = "Text nou";
        panelTip.GetComponent<DialogInfo>().Active(false);
        textTip.GetComponent<Text>().text = "Aquí es mostraran els diferents trucs que pot fer el jugador";
        players = new List<Player>();
		players.Add(new Player("Jugador 1", 2,cores[GameSelection.player1Core])); // Test with 2 players
		players.Add(new Player("Jugador 2", 3,cores[GameSelection.player2Core])); // Test with 2 players
		players[0].SetColor(GameSelection.player1Color);
		players[1].SetColor(GameSelection.player2Color);
		//matrix = new Matrix(MapParser.ReadMap(MapTypes.Medium));
        matrix = GameSelection.map;//new Matrix(11, 0.3f, 1234567);
        if(matrix==null) matrix= new Matrix(11, 0.3f, 1234567);
        MapDrawer.instantiateMap(matrix.getIterable());
        Vector2 slime1 = matrix.GetRandomTile();
		instantiateSlime(cores[0], players[0], (int)slime1.x, (int)slime1.y);
        Vector2 slime2 = matrix.GetRandomTile();
		instantiateSlime(cores[0], players[0],(int)slime2.x, (int)slime2.y);
        Vector2 slime3 = matrix.GetRandomTile();
		instantiateSlime(cores[1], players[1], (int)slime3.x, (int)slime3.y);
        Vector2 slime4 = matrix.GetRandomTile();
		instantiateSlime(cores[1], players[1], (int)slime4.x, (int)slime4.y);
        currentTurn = 0;
        currentPlayer = 0;
        playerActions = 0;
		foreach(Player p in players){
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
        bool ended = IsGameEnded();

        if (ended)
        {
            GameOverInfo.SetWinner(players[0]);
            SceneManager.LoadScene("GameOver");
        }
        foreach ( Player player in players)
        {
            if(player.GetNumSlimes() == 0)
            {
                //This player loses
                GameOverInfo.SetLoser(player);
                players.Remove(player);
            }
        }
    }

	public void checkLogic(){
		if (playerActions >= players [currentPlayer].GetActions ()) {
			currentPlayer++;
			if (currentPlayer >= players.Count) {
				currentPlayer = 0;
				currentTurn++;
			}
			playerActions = 0;
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
    }

    public void PrepareAction(int action)
    {
        PrepareAction((ActionType)action);
    }


    /*
	Funció que avança al seguent jugador.
	 */
    private void NextPlayer()
    {
        currentPlayer++;
        playerActions = 0;
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
        currentPlayer = 0;
        playerActions = 0;
        currentTurn++;
    }

    private Slime instantiateSlime(SlimeCoreData core, Player pl, int x0, int y0)
    {

        GameObject slime = new GameObject("Slime " + (pl.GetNumSlimes() + 1).ToString() + " - " + pl.GetName());
        slime.AddComponent<SpriteRenderer>();
        slime.tag = "Slime";
        slime.AddComponent<Slime>();
		slime.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(core.picDirection+0);
        slime.GetComponent<SpriteRenderer>().sortingLayerName = "SlimeBorder";
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

    public void MoveSlime(Tile tile)
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

	public void SlplitSlime(Tile targetTile){
		Slime newSlime = instantiateSlime(selectedSlime.GetPlayer().slimeCoreData, selectedSlime.GetPlayer(), (int) targetTile.GetTileData().getPosition().x, (int) targetTile.GetTileData().getPosition().y);
		players [currentPlayer].AddSlime (newSlime);
		allSlimes.Add (newSlime);
		targetTile.SetSlimeOnTop (newSlime.gameObject);
		newSlime.SetActualTile (targetTile);
		newSlime.SetMass (selectedSlime.GetMass()/2.0f);
		selectedSlime.SetMass (selectedSlime.GetMass() / 2.0f);
		playerActions++;
		status = GameControllerStatus.CHECKINGLOGIC;
	}

	public void AttackSlime(Slime targetSlime){
		FloatingTextController.CreateFloatingText ((-selectedSlime.getDamage ()).ToString(),targetSlime.transform);
		targetSlime.changeMass (-selectedSlime.getDamage ());
		if (!targetSlime.isAlive ()) {
			targetSlime.GetTileData ().SetSlimeOnTop (null);
			Destroy (targetSlime.gameObject);
			allSlimes.Remove (targetSlime);
		}
		playerActions++;
		status = GameControllerStatus.CHECKINGLOGIC;
        RangedAttack(targetSlime);
	}

    public void RangedAttack(Slime toAttack)
    {
        GameObject projectile = new GameObject("projectile");
        Sprite sprite = Resources.Load<Sprite>("Sprites/Proj");
        projectile.AddComponent<ProjectileTrajectory>();
        projectile.AddComponent<SpriteRenderer>().sprite = sprite;
        projectile.GetComponent<SpriteRenderer>().sortingLayerName = "SlimeBorder";
		projectile.GetComponent<SpriteRenderer> ().color = selectedSlime.GetPlayer ().GetColor ();
        projectile.GetComponent<Transform>().localScale = new Vector3(0.3f, 0.3f, 1f);
        Vector2 startPos = selectedSlime.GetComponent<Slime>().GetActualTile().GetTileData().GetRealWorldPosition();
        Vector2 endPos = toAttack.GetActualTile().GetTileData().GetRealWorldPosition();
        projectile.GetComponent<ProjectileTrajectory>().SetTrajectoryPoints(startPos, endPos);
    }

    public void FusionSlime(Slime fusionTarget)
	{
		players [currentPlayer].GetSlimes ().Remove(selectedSlime);
		allSlimes.Remove(selectedSlime);
		selectedSlime.GetActualTile ().SetSlimeOnTop (null);
		fusionTarget.SetMass (selectedSlime.GetMass() + fusionTarget.GetMass());

		Destroy (selectedSlime.gameObject);
		playerActions++;
		status = GameControllerStatus.CHECKINGLOGIC;
	}

	public void ConquerTile(Tile tile){
		tile.tileElementLayer.sprite = conquerSprite;
		Color c = selectedSlime.GetPlayer ().GetColor ();
		c.a = 0.5f;
		tile.tileElementLayer.color = c;
		playerActions++;
		status = GameControllerStatus.CHECKINGLOGIC;
	}

}
