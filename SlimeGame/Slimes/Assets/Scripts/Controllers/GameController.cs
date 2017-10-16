using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    private const int MAX_TURNS = 5;

    private GameObject selectedSlime;
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
        instantiateSlime(players[0], 0, 0);
        instantiateSlime(players[1], 2, 2);
        selectedSlime = new GameObject("Empty"); //Init selected item as Empty

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
        GUI.Label(new Rect(10, 30, 200, 40), "PLAYER:  " + getCurrentPlayer().GetName());
        GUI.Label(new Rect(10, 50, 200, 40), "ACTIONS:  " + (getCurrentPlayer().GetActions()-playerActions));
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
        if(playerActions + numberOfActions > getCurrentPlayer().GetActions()) return false; // Accions insuficients

        playerActions += numberOfActions;
        
        if(playerActions >= getCurrentPlayer().GetActions()){
            NextPlayer();
        }

        return true;
    }

    public void UseActionsPROVA(int numberOfActions){
        playerActions += numberOfActions;
        
        if(playerActions >= getCurrentPlayer().GetActions()){
            NextPlayer();
        }
    }

    /*
	Funció que avança al seguent jugador.
	 */
    private void NextPlayer(){
        currentPlayer++;
        playerActions = 0;
        DeselectItem();
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

    private void instantiateSlime(Player pl, int x0, int y0)
    {
              
        GameObject slime = new GameObject("Slime "+(pl.GetNumSlimes()+1).ToString()+" - "+pl.GetName());
        slime.AddComponent<SpriteRenderer>();
        slime.tag = "Slime";
        slime.AddComponent<Slime>();
        slime.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Test/slime");
        slime.GetComponent<SpriteRenderer>().sortingOrder = 1;
        slime.AddComponent<BoxCollider2D>();
        slime.AddComponent<SlimeMovement>();
        pl.AddSlime(slime);  
        slime.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        Vector2 tileWorldPosition = MapDrawer.drawInternCoordenates(new Vector2(x0,y0));
        slime.transform.position = new Vector3 (tileWorldPosition.x, tileWorldPosition.y, 0f);
        slime.GetComponent<Slime>().actualTile = matrix.getTile(x0,y0);

    }

    public GameObject GetSelectedSlime()
    {
        return selectedSlime;
    }
    public void SetSelectedSlime(GameObject gameObject)
    {
        if (selectedSlime.name.Equals("Empty"))
            Destroy(selectedSlime);
        
        if(!gameObject.tag.Equals("Slime") || getCurrentPlayer().IsSlimeOwner(gameObject))
            selectedSlime = gameObject;
    }
    public void DeselectItem()
    {
        SetSelectedSlime(new GameObject("Empty"));
    }
   

    public void userHitOnTile(TileData tilehit){
        GameObject slime = selectedSlime;
        if(!selectedSlime.name.Equals("Empty")){
            Vector2 positionSlime = slime.GetComponent<Slime>().actualTile.getPosition();
            //s'ha de calcular un cop (al començar torn i recalcular al fer qualsevol accio (ja que el range hauria de ser en referencia a aixo))
            //guardar a slime.possibleMovements i a aqui només executar
            //Dictionary<TileData, List<TileData>> listdic =  slime.GetComponent<Slime>().possibleMovements
            //enlloc de:
            Dictionary<TileData, List<TileData>> listdic = matrix.possibleCoordinatesAndPath((int)positionSlime.x, (int)positionSlime.y, 4);
            
            if(listdic.ContainsKey(tilehit) && UseActions(1)){
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
