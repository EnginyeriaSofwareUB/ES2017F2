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

	public List<Slime> allSlimes;

	private GameControllerStatus status;


    // Use this for initialization
    void Start()
    {
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
        //MapDrawer.InitTest ();
		status = GameControllerStatus.WAITINGFORACTION;
        panelTip = GameObject.Find("PanelTip"); //ja tenim el panell, per si el necessitem activar, i desactivar amb : panelTip.GetComponent<DialogInfo> ().Active (boolean);
        textTip = GameObject.Find("TextTip"); //ja tenim el textBox, per canviar el text : textTip.GetComponent<Text> ().text = "Text nou";
        panelTip.GetComponent<DialogInfo>().Active(false);
        textTip.GetComponent<Text>().text = "Aquí es mostraran els diferents trucs que pot fer el jugador";
        players = new List<Player>();
        players.Add(new Player("Jugador 1", 2,cores[0])); // Test with 2 players
		players.Add(new Player("Jugador 2", 3,cores[1])); // Test with 2 players
		//matrix = new Matrix(MapParser.ReadMap(MapTypes.Medium));
        matrix = new Matrix(13, 0.5f);
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
    }

    // Update is called once per frame
    void Update()
    {
        bool ended = IsGameEnded();

        if (ended)
        {
            SceneManager.LoadScene("GameOver");
        }
        foreach ( Player player in players)
        {
            if(player.GetNumSlimes() == 0)
            {
                //This player loses
                players.Remove(player);
            }
        }
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

    private void instantiateSlime(SlimeCoreData core, Player pl, int x0, int y0)
    {

        GameObject slime = new GameObject("Slime " + (pl.GetNumSlimes() + 1).ToString() + " - " + pl.GetName());
        slime.AddComponent<SpriteRenderer>();
        slime.tag = "Slime";
        slime.AddComponent<Slime>();
		slime.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(core.picDirection);
        slime.GetComponent<SpriteRenderer>().sortingLayerName = "SlimeBorder";
        slime.AddComponent<BoxCollider2D>();
        slime.AddComponent<SlimeMovement>();
		pl.AddSlime(slime.GetComponent<Slime>());
        slime.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
		Tile tile = MapDrawer.GetTileAt(x0, y0);
		Vector2 tileWorldPosition = tile.GetTileData().GetRealWorldPosition();//MapDrawer.drawInternCoordenates(new Vector2(x0, y0));
        slime.transform.position = new Vector3(tileWorldPosition.x, tileWorldPosition.y, 0f);
        slime.GetComponent<Slime>().SetActualTile(tile);
        slime.GetComponent<Slime>().setPlayer(pl);
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

    public void MoveSlime(TileData tileTo)
    {
        //Debug.Log("userHitOnTile");
		GameObject slime = selectedSlime.gameObject;
        if (!selectedSlime.name.Equals("Empty") && !selectedSlime.GetComponent<SlimeMovement>().moving)
        {
            Dictionary<TileData, List<TileData>> listdic = slime.GetComponent<Slime>().possibleMovements;

            //if (listdic.ContainsKey(tilehit) && UseActions(1))
            if (UseActions(1))
            {
                //List<Vector2> listvec = new List<Vector2>();
                List<TileData> path = listdic[tileTo];

                slime.GetComponent<SlimeMovement>().SetBufferAndPlay(path);

                slime.GetComponent<Slime>().rangeUpdated = false;
                //positionSlime = slime.GetComponent<Slime>().GetActualTile().getPosition();
			}
        }
    }

    public void AttackSlime(Slime toAttack)
    {
        // GESTIONAR ATAQUE
        if (UseActions(1))
        {
            Debug.Log("ATTACK");
        }
    }

    public void DivideSlime(Tile posToDivide)
    {
        // GESTIONAR DIVISION
        if (UseActions(1))
        {
            Debug.Log("DIVISION");
        }
    }

    public void FusionSlime(Tile posToFusion)
    {
        // GESTIONAR FUSION
        if (UseActions(1))
        {
            Debug.Log("FUSION");
        }
    }

}
