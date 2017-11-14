using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    GameObject canvasInfo;
	GameController gameController;

	private Sprite TileMoveSprite;
	private Sprite TileAttackSprite;
	private Sprite TileSplitSprite;

	private List<SpriteRenderer> currentUIRenderer;

    // Use this for initialization
    void Start () {
		
		gameController = Camera.main.GetComponent<GameController>();
        canvasInfo = GameObject.Find("Dialog");
        DisableCanvas();

        //Si clica OK desactiva el canvas
        canvasInfo.GetComponentInChildren<Button>().onClick.AddListener(DisableCanvas);

		TileMoveSprite = Resources.Load<Sprite> ("Test/movementRangeFilter");
		TileAttackSprite = Resources.Load<Sprite> ("Test/attackRangeFilter");
		TileSplitSprite = Resources.Load<Sprite> ("Test/movementRangeFilter");
		currentUIRenderer = new List<SpriteRenderer> ();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Metode que mostra la info que li passis
    public void ShowCanvasInfo(string info)
    {
        canvasInfo.SetActive(true);
        Text t = canvasInfo.GetComponentInChildren<Text>();
        t.text = info;
    }

    //Desactiva el canvas
    public void DisableCanvas()
    {
        canvasInfo.SetActive(false);
    }

	public List<Tile> showSplitRange(Slime slime){
		List<TileData> neighbours = gameController.matrix.getNeighbours (slime.GetTileData());
		List<Tile> splitTiles = new List<Tile> ();
		foreach (TileData td in neighbours) {
			Tile t = td.getTile ();
			t.tileUILayer.sprite = TileSplitSprite;
			currentUIRenderer.Add(t.tileUILayer);
			splitTiles.Add (t);
		}
		return splitTiles;
	}

	public List<Tile> showMoveRange(Slime slime){

		ArrayList tiles = new ArrayList();
		ArrayList distance = new ArrayList ();
		List<Tile> visited = new List<Tile> ();

		List<Tile> moveTiles = new List<Tile> ();

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
		foreach (Tile t in visited) {
			moveTiles.Add (t);
			t.tileUILayer.sprite = TileMoveSprite;
			currentUIRenderer.Add (t.tileUILayer);
		}
		return moveTiles;
	}

	public List<Tile> showAttackRange(Slime slime){
		Player currentPlayer = gameController.GetCurrentPlayer ();
		List<Tile> attackTiles = new List<Tile> ();
		Vector2 myPos = slime.GetActualTile().getPosition();
		foreach(Slime s in gameController.allSlimes){
			if (currentPlayer != s.GetPlayer()){
				Vector2 slPos = s.GetActualTile().getPosition();		
				if (Matrix.GetDistance(slPos, myPos) <= s.GetAttackRange()){
					s.GetActualTile ().tileUILayer.sprite = TileAttackSprite;
					currentUIRenderer.Add (s.GetActualTile ().tileUILayer);
					attackTiles.Add (s.GetActualTile ());
				}
			}
		}
		return attackTiles;
	}

	public void hideCurrentUITiles(){
		foreach (SpriteRenderer sp in currentUIRenderer) {
			sp.sprite = null;
		}
		currentUIRenderer.Clear ();
	}
}
