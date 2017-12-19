using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    protected GameObject canvasInfo;
	protected GameController gameController;

	protected Sprite TileSprite;

	protected Color moveColor = new Color (0f,0xa5,1f);
	protected Color attackColor = new Color (1f,0f,0f);
	protected Color splitColor = new Color (1f,0xdd,0f);
	protected Color joinColor = new Color (0.54f,1f,0f);
	//private Color conquerColor = new Color (0xce,0x0c,0xc1);
	//private Color specialColor = new Color (1f,0x6a,0f);
	protected Color selectedColor = new Color (1f, 0.843f, 0f);

	protected List<SpriteRenderer> currentUIRenderer;

	protected GameObject round;
	protected GameObject playerColor;
	protected GameObject actionsLeft;

	protected GameObject health;
	protected GameObject healthT;
	protected GameObject range;
	protected GameObject rangeT;
	protected GameObject movement;
	protected GameObject movementT;
	protected GameObject attack;
	protected GameObject attackT;
	protected GameObject defense;
	protected GameObject defenseT;

	public GameObject turnPanel;
	public GameObject roundPanel;
	public GameObject infoPanel;

	public GameObject growButton;

	protected float currentTime;
	protected float maxTime;
	protected float normalizedValue;
	protected Vector3 startPosT;
	protected Vector3 endPosT;
	protected Vector3 startPosR;
	protected Vector3 endPosR;
	protected Vector3 startPosI;
	protected Vector3 endPosI;
	protected RectTransform rectTransformT;
	protected RectTransform rectTransformR;
	protected RectTransform rectTransformI;

	protected int tempRound;
	protected Color tempColor;
	protected int tempAct;
	protected int tempMaxAct;
	protected Slime tempSlime;
	protected Tile tempTerrain;

	protected int state;
	/* state 0: do nothing
	 * state 1: hide turn panel
	 * state 2: show turn panel
	 * state 3: hide both panels
	 * state 4: show both panels
	 * state 5: show info panel
	 * state 6: hide info panel
	 * state 7: hide and show info panel
	*/

	public int xLimit;
    public int yLimit;
    public int minZoom;
    public int maxZoom;
    public int speed;

	public bool selected;

    // Use this for initialization
    void Start () {
		gameController = Camera.main.GetComponent<GameController>();
        //canvasInfo = GameObject.Find("Dialog");
        //DisableCanvas();
		/*
        RectTransform rt = canvasInfo.GetComponent(typeof(RectTransform)) as RectTransform;
        rt.sizeDelta =  new Vector2(200, 150); ;

        RectTransform rt2 = canvasInfo.GetComponentInChildren<Text>().GetComponent(typeof(RectTransform)) as RectTransform;
        rt2.sizeDelta = new Vector2(200, 150);*/
        //Si clica OK desactiva el canvas
		if (canvasInfo != null) {
			canvasInfo.GetComponentInChildren<Button> ().onClick.AddListener (DisableCanvas);
		}
		TileSprite = SpritesLoader.GetInstance().GetResource("Tiles/new_border");
		currentUIRenderer = new List<SpriteRenderer> ();
		round = GameObject.Find ("RoundNum");
		playerColor = GameObject.Find ("PlayerColor");
		actionsLeft = GameObject.Find ("ActionsNum");
		health = GameObject.Find ("Health");
		healthT = GameObject.Find ("HealthT");
		range = GameObject.Find ("Range");
		rangeT = GameObject.Find ("RangeT");
		movement = GameObject.Find ("Movement");
		movementT = GameObject.Find ("MovementT");
		attack = GameObject.Find ("Attack");
		attackT = GameObject.Find ("AttackT");
		defense = GameObject.Find ("Defense");
		defenseT = GameObject.Find ("DefenseT");
		turnPanel = GameObject.Find ("TurnPanel");
		roundPanel = GameObject.Find ("RoundPanel");
		infoPanel = GameObject.Find ("InfoPanel");
		rectTransformT = turnPanel.GetComponent<RectTransform> ();
		rectTransformR = roundPanel.GetComponent<RectTransform> ();
		rectTransformI = infoPanel.GetComponent<RectTransform> ();
		growButton = GameObject.Find ("GrowButton");
		state = 0;
		selected = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (state == 1 || state == 2) {
			if (currentTime < maxTime) {
				currentTime += Time.deltaTime;
				normalizedValue = currentTime / maxTime;
				rectTransformT.anchoredPosition = Vector3.Lerp (startPosT, endPosT, normalizedValue);
			} else {
				if (state == 1) {
					UpdatePlayer (tempColor);
					UpdateActions (tempAct, tempMaxAct);
					ShowTurnPanel ();
				} else {
					state = 0;
					gameController.updateStatus (GameControllerStatus.WAITINGFORACTION);
				}
			}
		} else if (state == 3 || state == 4) {
			if (currentTime < maxTime) {
				currentTime += Time.deltaTime;
				normalizedValue = currentTime / maxTime;
				rectTransformT.anchoredPosition = Vector3.Lerp (startPosT, endPosT, normalizedValue);
				rectTransformR.anchoredPosition = Vector3.Lerp (startPosR, endPosR, normalizedValue);
			} else {
				if (state == 3) {
					UpdateRound (tempRound);
					UpdatePlayer (tempColor);
					UpdateActions (tempAct, tempMaxAct);
					ShowBothPanels ();
				} else {
					state = 0;
					gameController.updateStatus (GameControllerStatus.WAITINGFORACTION);
				}
			}
		} else if (state == 5 || state == 6 || state == 7) {
			if (currentTime < maxTime) {
				currentTime += Time.deltaTime;
				normalizedValue = currentTime / maxTime;
				rectTransformI.anchoredPosition = Vector3.Lerp (startPosI, endPosI, normalizedValue);
			} else {
				if (state == 7){
					state = 5;
					ShowInfoPanel(tempSlime, tempTerrain);
				} else {
					state = 0;
				}

				gameController.updateStatus (GameControllerStatus.WAITINGFORACTION);
				/*if (state == 5) {
					state = 0;
					//UpdateInfo ();
					//UpdateRound (tempRound);
					//UpdatePlayer (tempColor);
					//UpdateActions (tempAct, tempMaxAct);
					//ShowInfoPanel ();
				} else {
					state = 0;
					gameController.updateStatus (GameControllerStatus.WAITINGFORACTION);
				}*/
				//state = 0;
				//gameController.updateStatus (GameControllerStatus.WAITINGFORACTION);
			}
		}
	}


	public void UpdateRound(int r){
		round.GetComponent<Text> ().text = r.ToString();
	}

	public void UpdatePlayer(Color c){
		playerColor.GetComponent<RawImage>().color = c;
	}
		
	public void UpdateActions(int a, int max){
		actionsLeft.GetComponent<Text> ().text = a.ToString()+" / "+max.ToString();
	}

	public void NextPlayer(Color c, int a, int max){
		tempColor = c;
		tempAct = a;
		tempMaxAct = max;
		HideTurnPanel ();
	}

	public void NextRound(int r, Color c, int a, int max){
		tempRound = r;
		tempColor = c;
		tempAct = a;
		tempMaxAct = max;
		HideBothPanels ();
	}

	protected void HideTurnPanel(){
		state = 1;
		currentTime = 0;
		maxTime = 0.25f;
		startPosT = new Vector3 (230, -80, 0);
		endPosT = new Vector3 (230, 120, 0);
	}

	protected void ShowTurnPanel(){
		state = 2;
		currentTime = 0;
		maxTime = 0.25f;
		startPosT = new Vector3 (230, 120, 0);
		endPosT = new Vector3 (230, -80, 0);
	}
		
	protected void HideBothPanels(){
		state = 3;
		currentTime = 0;
		maxTime = 0.25f;
		startPosT = new Vector3 (230, -80, 0);
		endPosT = new Vector3 (230, 120, 0);
		startPosR = new Vector3 (0, -40, 0);
		endPosR = new Vector3 (0, 80, 0);
	}

	public void ShowBothPanels(){
		state = 4;
		currentTime = 0;
		maxTime = 0.25f;
		startPosT = new Vector3 (230, 120, 0);
		endPosT = new Vector3 (230, -80, 0);
		startPosR = new Vector3 (0, 80, 0);
		endPosR = new Vector3 (0, -40, 0);
	}

	public void HideInfoPanel(){
		state = 5;
		selected = false;
		currentTime = 0;
		maxTime = 0.25f;
		startPosI = new Vector3 (-205, -200, 0);
		endPosI = new Vector3 (10, -200, 0);
	}

	public void ShowInfoPanel(Slime slime, Tile terrain){
		UpdateInfo (slime, terrain);
		state = 6;
		selected = true;
		tempSlime = slime;
		tempTerrain = terrain;
		maxTime = 0.25f;
		currentTime = 0;
		startPosI = new Vector3 (10, -200, 0);
		endPosI = new Vector3 (-205, -200, 0);
	}

	public void HideAndShowInfoPanel(Slime slime, Tile terrain){
		state = 7;
		selected = false;
		tempSlime = slime;
		tempTerrain = terrain;
		currentTime = 0;
		maxTime = 0.25f;
		startPosI = new Vector3 (-205, -200, 0);
		endPosI = new Vector3 (10, -200, 0);
	}



	public void UpdateInfo(Slime slime, Tile terrain){

		if (slime != null) {
			health.GetComponent<Text>().text = slime.GetMass().ToString();
			range.GetComponent<Text>().text = slime.GetAttackRange ().ToString();
			movement.GetComponent<Text>().text = slime.GetMovementRange ().ToString();
			attack.GetComponent<Text> ().text = slime.getDamage.ToString ()+" ("+slime.selfDamage+")";
			defense.GetComponent<Text> ().text = Math.Round((slime.damageReduction*100)).ToString ();
		} else {
			health.GetComponent<Text>().text = "";
			range.GetComponent<Text>().text = "";
			movement.GetComponent<Text>().text = "";
			attack.GetComponent<Text>().text = "";
			defense.GetComponent<Text>().text = "";
		}

		if (terrain != null) {
			healthT.GetComponent<Text>().text = terrain.GetMass();
			rangeT.GetComponent<Text>().text = terrain.GetAttackRange ().ToString();
			movementT.GetComponent<Text>().text = terrain.GetMovementRange ().ToString();
			attackT.GetComponent<Text>().text = terrain.GetDamage().ToString();
			defenseT.GetComponent<Text>().text = terrain.GetDamage().ToString();
		} else {
			healthT.GetComponent<Text>().text = "";
			rangeT.GetComponent<Text>().text = "";
			movementT.GetComponent<Text>().text = "";
			attackT.GetComponent<Text>().text = "";
			defenseT.GetComponent<Text>().text = "";
		}
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
		if (canvasInfo != null) {
			canvasInfo.SetActive (false);
		}
    }

	public void markTiles(List<Tile> tiles,ActionType at){
		foreach (Tile t in tiles) {
			t.tileUILayer.sprite = TileSprite;
			switch (at) {
			case ActionType.ATTACK:
				t.tileUILayer.color = attackColor;
				break;
			case ActionType.CONQUER:
				t.tileUILayer.color = tempColor;
				break;
			case ActionType.EAT:
				t.tileUILayer.color = tempColor;
				break;
			case ActionType.FUSION:
				t.tileUILayer.color = joinColor;
				break;
			case ActionType.MOVE:
				t.tileUILayer.color = moveColor;
				break;
			case ActionType.SPLIT:
				t.tileUILayer.color = splitColor;
				break;
			default:
				break;
			}
			currentUIRenderer.Add(t.tileUILayer);
		}
	}

	public List<Tile> showSelectedSlime(Slime slime){
		List<Tile> selectedSlimeTile = new List<Tile> ();
		selectedSlimeTile.Add (slime.GetActualTile ());
		slime.GetActualTile().tileUILayer.sprite = TileSprite;
		slime.GetActualTile().tileUILayer.color = selectedColor;
		currentUIRenderer.Add(slime.GetActualTile().tileUILayer);
		return selectedSlimeTile;
	}

	public void hideCurrentUITiles(){
		foreach (SpriteRenderer sp in currentUIRenderer) {
			sp.sprite = null;
		}
		currentUIRenderer.Clear ();
	}
}
