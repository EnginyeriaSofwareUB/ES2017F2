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
	private Color joinColor = new Color (0.54f,1f,0f);
	//private Color conquerColor = new Color (0xce,0x0c,0xc1);
	//private Color specialColor = new Color (1f,0x6a,0f);
	private Color selectedColor = new Color (1f, 0.843f, 0f);

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

	public List<Tile> showSelectedSlime(Slime slime){
		List<Tile> selectedSlimeTile = new List<Tile> ();
		selectedSlimeTile.Add (slime.GetActualTile ());
		slime.GetActualTile().tileUILayer.sprite = TileSprite;
		slime.GetActualTile().tileUILayer.color = selectedColor;
		currentUIRenderer.Add(slime.GetActualTile().tileUILayer);
		return selectedSlimeTile;
	}

	public List<Tile> showJoinRange(Slime slime){
		List<Tile> joinTiles = gameController.GetJoinTile (slime);
		foreach(Tile t in joinTiles){
			t.tileUILayer.sprite = TileSprite;
			t.tileUILayer.color = joinColor;
			currentUIRenderer.Add(t.tileUILayer);
		}
		return joinTiles;
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
