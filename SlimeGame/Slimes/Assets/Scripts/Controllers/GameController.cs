using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    private const int MAX_TURNS = 5;

    private GameObject selectedSlime;
    private Matrix matrix;
    private List<Player> players;
    private GameObject panelTip, textTip;
    private int currentTurn;
    private int currentPlayer;
    private int playerActions;


    // Use this for initialization
    void Start()
    {

        Text go = GameObject.Find("DebugText").GetComponent<Text>();

        //MapDrawer.InitTest ();
        panelTip = GameObject.Find("PanelTip"); //ja tenim el panell, per si el necessitem activar, i desactivar amb : panelTip.GetComponent<DialogInfo> ().Active (boolean);
        textTip = GameObject.Find("TextTip"); //ja tenim el textBox, per canviar el text : textTip.GetComponent<Text> ().text = "Text nou";
        panelTip.GetComponent<DialogInfo>().Active(false);
        textTip.GetComponent<Text>().text = "Aquí es mostraran els diferents trucs que pot fer el jugador";

        players = new List<Player>();
        players.Add(new Player("Jugador 1", 2)); // Test with 2 players
        players.Add(new Player("Jugador 2", 3)); // Test with 2 players
        go.text = "gameController";
        matrix = new Matrix(MapParser.ReadMap(MapTypes.Medium));

        MapDrawer.instantiateMap(matrix.getIterable());
        SlimeCore agileCore = SlimeCore.Create(SlimeCoreTypes.Agile);
        SlimeCore aggressiveCore = SlimeCore.Create(SlimeCoreTypes.Aggressive);
        instantiateSlime(aggressiveCore, players[0], 0, -1);
        instantiateSlime(aggressiveCore, players[0], 1, 0);
        instantiateSlime(agileCore, players[1], 2, 2);
        instantiateSlime(agileCore, players[1], -1, 2);
        selectedSlime = new GameObject("Empty"); //Init selected item as Empty
        currentTurn = 0;
        currentPlayer = 0;
        playerActions = 0;
    }

    // Update is called once per frame
    void Update()
    {
        bool ended = IsGameEnded();

        if (ended)
        {
            SceneManager.LoadScene("GameOver");
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
        return currentTurn >= MAX_TURNS;
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
                    MapDrawer.ShowMovementRange(slime);
                    break;
                case ActionType.Attack:
                    MapDrawer.ShowAttackRange(slime, players);
                    break;
                case ActionType.Divide:
                    HideAnyRange();
                    MapDrawer.ShowDivisionRange(slime, matrix);
                    break;
                case ActionType.Fusion:
                    HideAnyRange();
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
        DeselectItem();
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

    private void instantiateSlime(SlimeCore core, Player pl, int x0, int y0)
    {

        GameObject slime = new GameObject("Slime " + (pl.GetNumSlimes() + 1).ToString() + " - " + pl.GetName());
        slime.AddComponent<SpriteRenderer>();
        slime.tag = "Slime";
        slime.AddComponent<Slime>();
        slime.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(SlimeCoreTypesCtrl.GetSprite(core.GetCoreType()));
        slime.GetComponent<SpriteRenderer>().sortingOrder = 2;
        slime.AddComponent<BoxCollider2D>();
        slime.AddComponent<SlimeMovement>();
        pl.AddSlime(slime);
        slime.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        TileData tile = matrix.getTile(x0, y0);
        Vector2 tileWorldPosition = tile.GetRealWorldPosition();//MapDrawer.drawInternCoordenates(new Vector2(x0, y0));
        slime.transform.position = new Vector3(tileWorldPosition.x, tileWorldPosition.y, 0f);
        slime.GetComponent<Slime>().SetActualTile(tile);
        slime.GetComponent<Slime>().setPlayer(pl);
        slime.GetComponent<Slime>().SetCore(core);
    }

    public GameObject GetSelectedSlime()
    {
        return selectedSlime;
    }
    public void SetSelectedSlime(GameObject gameObject)
    {
        // Seleccionem sempre que no sigui una slime o si es una slime, que sigui la SEVA slime.
        if (!gameObject.tag.Equals("Slime") || getCurrentPlayer().IsSlimeOwner(gameObject))
        {
            HideAnyRange();

            if (selectedSlime.name.Equals("Empty")) Destroy(selectedSlime);
            selectedSlime = gameObject;

            if (selectedSlime.tag.Equals("Slime"))
            {  
                Slime slime = selectedSlime.GetComponent<Slime>();
                // Actualitzem el rang si estem seleccionant una slime sense el rang actualitzat.
                if (!slime.rangeUpdated)
                {
                    //Debug.Log("Updating range...");
                    int range = slime.GetMovementRange();
                    Vector2 positionSlime = selectedSlime.GetComponent<Slime>().GetActualTile().getPosition();
                    slime.possibleMovements = matrix.possibleCoordinatesAndPath(
                        (int)positionSlime.x, (int)positionSlime.y, range);

                    slime.rangeUpdated = true;
                }
                PrepareAction(ActionType.Move);
                PrepareAction(ActionType.Attack);

                //show info
                Camera.main.GetComponent<UIController>().ShowCanvasInfo(slime.ToString());
            }
        }
    }
    public void DeselectItem()
    {
        //Debug.Log("deselecting");
        HideAnyRange();
        SetSelectedSlime(new GameObject("Empty"));
    }


    public void MoveSlime(TileData tileTo)
    {
        //Debug.Log("userHitOnTile");
        GameObject slime = selectedSlime;
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

                DeselectItem();
                HideAnyRange();
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

    public void HideAnyRange()
    {
        foreach (GameObject gObj in GameObject.FindGameObjectsWithTag("MovementRange"))
        {
            Destroy(gObj);
        }
        foreach (GameObject gObj in GameObject.FindGameObjectsWithTag("AttackRange"))
        {
            Destroy(gObj);
        }
        foreach (GameObject gObj in GameObject.FindGameObjectsWithTag("DivisionRange"))
        {
            Destroy(gObj);
        }
        foreach (GameObject gObj in GameObject.FindGameObjectsWithTag("FusionRange"))
        {
            Destroy(gObj);
        }

    }

}
