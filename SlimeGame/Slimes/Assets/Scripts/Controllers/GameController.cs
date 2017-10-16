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
    private ActionType turnAction;


    // Use this for initialization
    void Start()
    {
        //MapDrawer.InitTest ();

        players = new List<Player>();
        players.Add(new Player("Jugador 1")); // Test with only 1 player

        matrix = new Matrix(MapParser.ReadMap(MapTypes.Small));
        MapDrawer.instantiateMap(matrix.getIterable());
        instantiateSlime();
        selectedItem = new GameObject("Empty"); //Init selected item as Empty
        setupSlime();

        currentTurn = 0;
        currentPlayer = 0;
		turnAction = ActionType.Null;
    }

    // Update is called once per frame
    void Update()
    {
		bool ended = IsGameEnded();
        if (!ended && turnAction != ActionType.Null)
        {
			// Comprovem quina accio hem de realitzar.
            switch (turnAction)
            {
                case ActionType.Attack:
                    break;
                case ActionType.Conquer:
                    break;
                case ActionType.Divide:
                    break;
                case ActionType.Eat:
                    break;
                case ActionType.Move:
                    break;
                default:
                    break;
            }
			currentPlayer++;
			if(currentPlayer == players.Count){
				// Tots els jugadors han fet la seva accio, passem al seguent torn.
				NextTurn();
			}
        }
        else if(ended)
        {
            SceneManager.LoadScene("GameOver");
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), "TURN:  " + (currentTurn + 1).ToString());
        GUI.Label(new Rect(10, 30, 200, 40), "PLAYER:  " + players[currentPlayer].getName());
    }

	/*
	Funció que retorna True si s'ha acabat la partida o False sino.
	 */
    private bool IsGameEnded()
    {
        return currentTurn >= MAX_TURNS;
    }

	/*
	Funció que avança al seguent torn.
	 */
    public void NextTurn()
    {
		currentPlayer = 0;
        currentTurn++;
        turnAction = ActionType.Null;
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
