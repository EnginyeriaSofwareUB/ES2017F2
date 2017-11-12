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

		//TileMoveSprite = Resources.Load ();
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

	public void showSplitRange(Slime slime){
		List<TileData> neighbours = gameController.matrix.getNeighbours (slime.GetTileData());
		foreach (TileData td in neighbours) {
			Tile t = td.getTile ();
			t.tileUILayer.sprite = TileSplitSprite;
			currentUIRenderer.Add(t.tileUILayer);
		}
	}

	public void showMoveRange(Slime slime){
		
	}

	public void showAttackRange(Slime slime){
		Player currentPlayer = gameController.GetCurrentPlayer ();
		Vector2 myPos = slime.GetActualTile().getPosition();
		foreach(Slime s in gameController.allSlimes){
			if (currentPlayer != s.GetPlayer()){
				Vector2 slPos = s.GetActualTile().getPosition();		
				if (Matrix.GetDistance(slPos, myPos) <= s.GetAttackRange()){
					s.GetActualTile ().tileUILayer.sprite = TileAttackSprite;
					currentUIRenderer.Add (s.GetActualTile ().tileUILayer);
				}
			}
		}
	}

	public void hideCurrentUITiles(){
		foreach (SpriteRenderer sp in currentUIRenderer) {
			sp.sprite = null;
		}
		currentUIRenderer.Clear ();
	}
}
