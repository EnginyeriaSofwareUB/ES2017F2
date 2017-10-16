using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    private const int MAX_TURNS = 5;

    private GameObject selectedItem;
    private Matrix matrix;
    private List<Player> players;
    private int currentTurn;
    private int currentPlayer;
    private int playerActions;


    // Use this for initialization
    void Start()
    {
        //MapDrawer.InitTest ();

        players = new List<Player>();
        players.Add(new Player("Jugador 1", 2)); // Test with 2 players
        players.Add(new Player("Jugador 2", 3)); // Test with 2 players

        matrix = new Matrix(MapParser.ReadMap(MapTypes.Small));
        MapDrawer.instantiateMap(matrix.getIterable());
        instantiateSlime();
        selectedItem = new GameObject("Empty"); //Init selected item as Empty
        setupSlime();

        currentTurn = 0;
        currentPlayer = 0;
        playerActions = 0;
    }

    // Update is called once per frame
    void Update()
    {
		bool ended = IsGameEnded();

        if(ended)
        {
            SceneManager.LoadScene("GameOver");
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), "TURN:  " + (currentTurn + 1).ToString());
        GUI.Label(new Rect(10, 30, 200, 40), "PLAYER:  " + getCurrentPlayer().getName());
        GUI.Label(new Rect(10, 50, 200, 40), "ACTIONS:  " + (getCurrentPlayer().getActions()-playerActions));
    }

    /*
    Retorna el jugador que li toca fer una acció.
     */
    private Player getCurrentPlayer(){
        return players[currentPlayer];
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
    private bool UseActions(int numberOfActions){
        if(playerActions + numberOfActions > getCurrentPlayer().getActions()) return false; // Accions insuficients

        playerActions += numberOfActions;
        
        if(playerActions >= getCurrentPlayer().getActions()){
            NextPlayer();
        }

        return true;
    }

    public void UseActionsPROVA(int numberOfActions){
        playerActions += numberOfActions;
        
        if(playerActions >= getCurrentPlayer().getActions()){
            NextPlayer();
        }
    }

    /*
	Funció que avança al seguent jugador.
	 */
    private void NextPlayer(){
        currentPlayer++;
        playerActions = 0;
        if(currentPlayer >= players.Count){
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

    private void instantiateSlime()
    {
        GameObject slime = new GameObject("Slime");
        slime.AddComponent<SpriteRenderer>();
        slime.tag = "Slime";
        slime.AddComponent<Slime>();
        slime.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Test/slime");
        slime.GetComponent<SpriteRenderer>().sortingOrder = 1;
        slime.AddComponent<BoxCollider2D>();
        slime.AddComponent<SlimeMovement>();

    }

    public GameObject GetSelectedItem()
    {
        return selectedItem;
    }
    public void SetSelectedItem(GameObject gameObject)
    {
        if (selectedItem.name.Equals("Empty"))
            Destroy(selectedItem);
        selectedItem = gameObject;
    }
    public void DeselectItem()
    {
        SetSelectedItem(new GameObject("Empty"));
    }
    private void setupSlime()
    {
        GameObject slime = GameObject.FindGameObjectWithTag("Slime");
        slime.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        slime.GetComponent<Slime>().actualTile = matrix.getTile(0,0);
    }

    public void userHitOnTile(TileData tilehit){
        // Aquest 1 es el nombre d'accions que necessites per moure't.
        if(UseActions(1)){
            GameObject slime = GameObject.FindGameObjectWithTag("Slime");
            Vector2 positionSlime = slime.GetComponent<Slime>().actualTile.getPosition();
            //s'ha de calcular un cop (al començar torn i recalcular al fer qualsevol accio (ja que el range hauria de ser en referencia a aixo))
            //guardar a slime.possibleMovements i a aqui només executar
            //Dictionary<TileData, List<TileData>> listdic =  slime.GetComponent<Slime>().possibleMovements
            //enlloc de:
            Dictionary<TileData, List<TileData>> listdic = matrix.possibleCoordinatesAndPath((int)positionSlime.x, (int)positionSlime.y, 4);
            
            if(listdic.ContainsKey(tilehit)){
                List<Vector2> listvec = new List<Vector2>();
                List<TileData> path = listdic[tilehit];
                foreach (TileData tile in path)
                {
                    listvec.Add(MapDrawer.drawInternCoordenates(tile.getPosition()));
                }
                slime.GetComponent<SlimeMovement>().SetBufferAndPlay(listvec);
                slime.GetComponent<Slime>().actualTile=path[path.Count-1];
                positionSlime = slime.GetComponent<Slime>().actualTile.getPosition();
                positionSlime = slime.GetComponent<Slime>().actualTile.getPosition();
            }
        }
    }
}
