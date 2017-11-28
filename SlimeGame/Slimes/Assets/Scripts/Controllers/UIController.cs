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

	public int xLimit;
    public int yLimit;
    public int minZoom;
    public int maxZoom;
    public int speed;
    // Use this for initialization
    void Start () {
		speed = 30;
        minZoom = 3;
        maxZoom = 13;
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
		List<TileData> neighbours = gameController.matrix.getNeighbours (slime.GetTileData());
		List<Tile> splitTiles = new List<Tile> ();
		foreach (TileData td in neighbours) {
			Tile t = td.getTile ();
			t.tileUILayer.sprite = TileSprite;
			t.tileUILayer.color = splitColor;
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
			t.tileUILayer.sprite = TileSprite;
			t.tileUILayer.color = moveColor;
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
					s.GetActualTile ().tileUILayer.sprite = TileSprite;
					s.GetActualTile ().tileUILayer.color = attackColor;
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
	public void InitMapSize(Vector2 mapSize){
		xLimit = (int) mapSize.x;
        yLimit = (int) mapSize.y;
	}
	public void ZoomIn(){
		if(this.GetComponent<Camera> ().orthographicSize > minZoom){
			this.GetComponent<Camera> ().orthographicSize--;
		}
	}
	public void ZoomOut(){
		float horzExtent = 0.8f* this.GetComponent<Camera> ().orthographicSize * this.GetComponent<Camera> ().aspect;
		float vertExtent = 0.8f* this.GetComponent<Camera> ().orthographicSize;
		if(vertExtent<yLimit || horzExtent<xLimit){
			this.GetComponent<Camera> ().orthographicSize++;
		}
	}
	public void MoveUp(){
		float orto = 1f/this.GetComponent<Camera> ().orthographicSize;
		if (this.transform.position.y < yLimit) {
			this.transform.position += (new Vector3 (0, speed*orto*2, 0) * Time.deltaTime);
		}
	}
	public void MoveDown(){
		float orto = 1f/this.GetComponent<Camera> ().orthographicSize;
		if (this.transform.position.y > -yLimit) {
			this.transform.position -= (new Vector3 (0, speed*orto*2, 0) * Time.deltaTime);
		}
	}
	public void MoveLeft(){
		float orto = 1f/this.GetComponent<Camera> ().orthographicSize;
		if (this.transform.position.x > -xLimit) {
			this.transform.position -= (new Vector3 (speed*orto*2, 0, 0) * Time.deltaTime);
		}
	}
	public void MoveRight(){
		float orto = 1f/this.GetComponent<Camera> ().orthographicSize;
		if (this.transform.position.x < xLimit) {
			this.transform.position += (new Vector3 (speed*orto*2, 0, 0) * Time.deltaTime);
		}
	}

	public Rect GetRectWithAllPoints(List<Vector2> vectors, float aspect){
		if(vectors.Count<=0)return new Rect(0,0,0,0);
		float invaspect = 1.0f/aspect;
		Rect rect = new Rect(vectors[0].x,vectors[0].y,aspect,1);
		foreach(Vector2 vect in vectors){
			if(!rect.Contains(vect)){
				float diff;			
				if((diff = vect.x-rect.xMax)>0f){
					rect.xMax = vect.x;
					rect.yMax += diff*invaspect;
				}
				if((diff = rect.xMin-vect.x)>0f){
					rect.xMin = vect.x;
					rect.yMin -= diff*invaspect;
				}
				if((diff = vect.y-rect.yMax)>0f){
					rect.yMax = vect.y;
					rect.xMax += diff*invaspect;
				}
				if((diff = rect.yMin-vect.y)>0f){
					rect.yMin = vect.y;
					rect.xMin -= diff*invaspect;
				}
			}
			
		}		
		return rect;
	}
	public void ChangeCamera(List<Slime> slimes){
		List<Vector2> vects = new List<Vector2>();
		foreach(Slime slime in slimes){
			vects.Add(slime.actualTile.getPosition());
		}
		Rect rect = GetRectWithAllPoints(vects,this.GetComponent<Camera> ().aspect);
		//GUI.Label(rect,"465");
		//this.transform.position+=(new Vector3(rect.center.x,rect.center.y,0)* Time.deltaTime*speed);
	}
	
	
}
