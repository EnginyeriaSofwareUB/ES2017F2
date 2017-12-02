using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    GameObject canvasInfo;
	GameController gameController;

	private Sprite TileSprite;

	private Color moveColor = new Color (0f,0xa5,1f);
	private Color attackColor = new Color (1f,0f,0f);
	private Color splitColor = new Color (1f,0xdd,0f);
	private Color joinColor = new Color (0x21,1f,0f);
	private Color conquerColor = new Color (0xce,0x0c,0xc1);
	private Color specialColor = new Color (1f,0x6a,0f);

	private List<SpriteRenderer> currentUIRenderer;
    // Use this for initialization
    void Start () {
		gameController = Camera.main.GetComponent<GameController>();
        canvasInfo = GameObject.Find("Dialog");
        DisableCanvas();

        //Si clica OK desactiva el canvas
        canvasInfo.GetComponentInChildren<Button>().onClick.AddListener(DisableCanvas);

		TileSprite = SpritesLoader.GetInstance().GetResource("Tiles/tile_border");
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
		List<Tile> splitTiles = gameController.GetSplitRangeTiles(slime);
		foreach (Tile t in splitTiles) {
			t.tileUILayer.sprite = TileSprite;
			t.tileUILayer.color = splitColor;
			currentUIRenderer.Add(t.tileUILayer);
		}
		return splitTiles;
	}

	public List<Tile> showMoveRange(Slime slime){
		List<Tile> moveTiles = gameController.GetPossibleMovements(slime);
		foreach (Tile t in moveTiles) {
			t.tileUILayer.sprite = TileSprite;
			t.tileUILayer.color = moveColor;
			currentUIRenderer.Add (t.tileUILayer);
		}
		return moveTiles;
	}

	public List<Tile> showAttackRange(Slime slime){
		List<Slime> slimesInRange = gameController.GetSlimesInAttackRange(slime);
		List<Tile> attackTiles = new List<Tile>();
		foreach(Slime s in slimesInRange){
			s.GetActualTile ().tileUILayer.sprite = TileSprite;
			s.GetActualTile ().tileUILayer.color = attackColor;
			currentUIRenderer.Add (s.GetActualTile ().tileUILayer);
			attackTiles.Add (s.GetActualTile ());
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
