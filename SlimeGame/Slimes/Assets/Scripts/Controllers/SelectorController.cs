using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using SimpleJSON;

public class SelectorController : MonoBehaviour {
    private string sprite;
	private List<string> corePaths;
	private Queue<Color> colors;
	private List<int> coreSelector;
	private int currentColorSelector;
	private int modoVictoria;

	private int currentPlayer;
	private int maxPlayers;

	private GameObject player3;
	private GameObject player4;

	private Image currentSprite;
	private Image sprite1;
	private Image sprite2;
	private Image sprite3;
	private Image sprite4;
	private Image currentCore;
	private Image core1;
	private Image core2;
	private Image core3;
	private Image core4;

	// Use this for initialization
	void Start () {
		maxPlayers = 2;
		currentPlayer = 1;
		modoVictoria = 0;
		coreSelector = new List<int>();
		coreSelector.Add(0);
		coreSelector.Add(0);
		coreSelector.Add(1);

		corePaths = new List<string> ();
		corePaths.Add ("Sprites/Wrath");
		corePaths.Add ("Sprites/Sloth");
		corePaths.Add ("Sprites/Gluttony");
        sprite = "Sprites/slime_sprite";
		currentSprite = GameObject.Find ("CurrentSprite").GetComponent<Image>();
		sprite1 = GameObject.Find ("Sprite1").GetComponent<Image>();
		sprite2 = GameObject.Find ("Sprite2").GetComponent<Image>();
		sprite3 = GameObject.Find ("Sprite3").GetComponent<Image>();
		sprite4 = GameObject.Find ("Sprite4").GetComponent<Image>();
		currentCore = GameObject.Find ("CurrentCore").GetComponent<Image>();
		core1 = GameObject.Find ("Core1").GetComponent<Image>();
		core2 = GameObject.Find ("Core2").GetComponent<Image>();
		core3 = GameObject.Find ("Core3").GetComponent<Image>();
		core4 = GameObject.Find ("Core4").GetComponent<Image>();

		player3 = GameObject.Find ("Panel3");
		player4 = GameObject.Find("Panel4");
		player3.SetActive (false);
		player4.SetActive (false);

		currentSprite.overrideSprite = SpritesLoader.GetInstance().GetResource(sprite);
		sprite1.overrideSprite = SpritesLoader.GetInstance().GetResource(sprite);
		sprite2.overrideSprite = SpritesLoader.GetInstance().GetResource(sprite);
        currentCore.overrideSprite = SpritesLoader.GetInstance().GetResource(corePaths[coreSelector[1]]);
		core1.overrideSprite = SpritesLoader.GetInstance().GetResource(corePaths[coreSelector[1]]);
        core2.overrideSprite = SpritesLoader.GetInstance().GetResource(corePaths[coreSelector[2]]);
        colors = new Queue<Color> ();
		colors.Enqueue (new Color (1, 0, 0));
		colors.Enqueue (new Color (0, 0, 1));
		colors.Enqueue (new Color (0, 1, 0));
		colors.Enqueue (new Color (1, 1, 0));
		colors.Enqueue (new Color (0, 1, 1));
		colors.Enqueue (new Color (1, 0, 1));
		colors.Enqueue (new Color (1, 1, 1));
		currentSprite.color = colors.Dequeue();
		sprite1.color = currentSprite.color;
		sprite2.color = colors.Dequeue();
		GameObject.Find ("PaintStroke").GetComponent<Image> ().color = currentSprite.color;

		GameSelection.playerColors.Add(sprite1.color);
		GameSelection.playerColors.Add(sprite2.color);
		GameSelection.playerCores.Add(SlimeCoreTypes.WRATH);
		GameSelection.playerCores.Add(SlimeCoreTypes.SLOTH);
		GameSelection.playerIAs.Add (false);
		GameSelection.playerIAs.Add (true);
		GameSelection.modoVictoria = modoVictoria;

		}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void newPlayer(){
		if (maxPlayers < 4) {
			maxPlayers++;
			if (maxPlayers == 3) {
				player3.SetActive (true);
				GameObject.Find("Player3").GetComponent<Text>().text = Languages.GetString(GameObject.Find("Player3").GetComponent<Text>().name,GameObject.Find("Player3").GetComponent<Text>().text);
				sprite3.overrideSprite = SpritesLoader.GetInstance().GetResource(sprite);
				if (coreSelector.Count < 4)
					coreSelector.Add (2);
				core3.overrideSprite = SpritesLoader.GetInstance().GetResource(corePaths[coreSelector[3]]);
				sprite3.color = colors.Dequeue ();
				GameSelection.playerColors.Add(sprite3.color);
				GameSelection.playerCores.Add(SlimeCoreTypes.GLUTTONY);
				GameSelection.playerIAs.Add (GameObject.Find("IAToggle3").GetComponent<Toggle>().isOn);
			} else if (maxPlayers == 4) {
				player4.SetActive (true);
				GameObject.Find("Player4").GetComponent<Text>().text = Languages.GetString(GameObject.Find("Player4").GetComponent<Text>().name,GameObject.Find("Player4").GetComponent<Text>().text);				sprite4.overrideSprite = SpritesLoader.GetInstance().GetResource(sprite);
				if (coreSelector.Count < 5)
					coreSelector.Add (0);
				core4.overrideSprite = SpritesLoader.GetInstance().GetResource(corePaths[coreSelector[4]]);
				sprite4.color = colors.Dequeue();
				GameSelection.playerColors.Add(sprite4.color);
				GameSelection.playerCores.Add(SlimeCoreTypes.WRATH);
				GameSelection.playerIAs.Add (GameObject.Find("IAToggle4").GetComponent<Toggle>().isOn);
			}
		}
	}

	public void deletePlayer(){
		if (maxPlayers > 2) {
			if (currentPlayer == maxPlayers)
				changeCurrentPlayer(1);
			if (maxPlayers == 4) {
				colors.Enqueue (sprite4.color);
				player4.SetActive (false);
			} else if (maxPlayers == 3) {
				colors.Enqueue (sprite3.color);
				player3.SetActive (false);
			}
			maxPlayers--;
			GameSelection.playerColors.RemoveAt (GameSelection.playerColors.Count - 1);
			GameSelection.playerCores.RemoveAt (GameSelection.playerCores.Count - 1);
			GameSelection.playerIAs.RemoveAt (GameSelection.playerIAs.Count - 1);
		}
	}

	public void toggleIAOn(int player){
		GameSelection.playerIAs [player - 1] = !GameSelection.playerIAs [player - 1];
		/*if (player == 1)
			GameSelection.player1IA = b;
		else if (player == 2)
			GameSelection.player2IA = b;
		else if (player == 3)
			GameSelection.player3IA = b;
		else if (player == 4)
			GameSelection.player4IA = b;*/
	}

	public void toggleIAOff(int player){
		GameSelection.playerIAs [player - 1] = false;
	}

	private void changeInfo(){
		StatsContainer stats;
		if (coreSelector [currentPlayer] == 0) {
			stats = StatsFactory.GetStat (SlimeCoreTypes.WRATH);
		} else if (coreSelector [currentPlayer] == 1) {
			stats = StatsFactory.GetStat (SlimeCoreTypes.SLOTH);
		} else if (coreSelector [currentPlayer] == 2) {
			stats = StatsFactory.GetStat (SlimeCoreTypes.GLUTTONY);
		} else {
			stats = StatsFactory.GetStat (SlimeCoreTypes.GLUTTONY);
		}
		int dmg = stats.maxBaseAttack;
		int hp = stats.baseMass;
		int rng = stats.range;
		int move = stats.movement;
		GameObject.Find ("CoreInfo").GetComponent<Text> ().text = "  " + hp + "     " + dmg + "\n   " + rng + "      " + move;
	}
	public void changeCurrentPlayer(int cursor){
		currentPlayer = cursor;
		currentCore.overrideSprite = SpritesLoader.GetInstance ().GetResource (corePaths [coreSelector [cursor]]);
		currentSprite.color = GameObject.Find ("Sprite"+cursor).GetComponent<Image> ().color;
		GameObject.Find ("PaintStroke").GetComponent<Image> ().color = currentSprite.color;
		//GameObject.Find ("CoreInfo").GetComponent<Text> ().text = coresInfo [coreSelector [currentPlayer]];
		changeInfo();
		//GameObject.Find ("PlayerText").GetComponent<Text> ().text = "PLAYER " + cursor;
		GameObject.Find ("PlayerText").GetComponent<Text> ().text = GameObject.Find("Player" + cursor).GetComponent<Text>().text;
	}

	public void changeCore(int cursor){
		coreSelector[currentPlayer] += cursor;
		if (coreSelector[currentPlayer] > corePaths.Count - 1)
			coreSelector[currentPlayer] = 0;
		else if (coreSelector[currentPlayer] < 0)
			coreSelector[currentPlayer] = corePaths.Count - 1;
		currentCore.overrideSprite = SpritesLoader.GetInstance().GetResource(corePaths [coreSelector[currentPlayer]]);
		changeInfo ();
		//GameObject.Find ("CoreInfo").GetComponent<Text> ().text = coresInfo [coreSelector[currentPlayer]];
		GameObject.Find("Core"+currentPlayer).GetComponent<Image> ().overrideSprite = SpritesLoader.GetInstance().GetResource(corePaths [coreSelector[currentPlayer]]);

		if (coreSelector [currentPlayer] == 0) {
			GameSelection.playerCores [currentPlayer - 1] = SlimeCoreTypes.WRATH;
		} else if (coreSelector [currentPlayer] == 1) {
			GameSelection.playerCores [currentPlayer - 1] = SlimeCoreTypes.SLOTH;
		} else if (coreSelector [currentPlayer] == 2) {
			GameSelection.playerCores [currentPlayer - 1] = SlimeCoreTypes.GLUTTONY;
		}	
		/*else if (currentPlayer == 2)
			GameSelection.player2Core = coreSelector[currentPlayer];
		else if (currentPlayer == 3)
			GameSelection.player3Core = coreSelector[currentPlayer];
		else if (currentPlayer == 4)
			GameSelection.player4Core = coreSelector[currentPlayer];*/
	}

	public void changeColor(){
		colors.Enqueue (currentSprite.color);
		currentSprite.color = colors.Dequeue ();
		GameObject.Find ("Sprite"+currentPlayer).GetComponent<Image> ().color = currentSprite.color;
		GameObject.Find ("PaintStroke").GetComponent<Image> ().color = currentSprite.color;

		GameSelection.playerColors [currentPlayer - 1] = currentSprite.color;
		/*if (currentPlayer == 1)
			GameSelection.player1Color = currentSprite.color;
		else if (currentPlayer == 2)
			GameSelection.player2Color = currentSprite.color;
		else if (currentPlayer == 3)
			GameSelection.player3Color = currentSprite.color;
		else if (currentPlayer == 4)
			GameSelection.player4Color = currentSprite.color;
		*/
	}	
	/*
	public void changeMap(int cursor){
		mapSelector+=cursor;
		if (mapSelector > mapTypes.Count-1) {
			mapSelector = 0;
		} else if (mapSelector < 0) {
			mapSelector = mapTypes.Count - 1;
		}
		loadMap ();
	}*/
	/*
	public void loadMap(){
		foreach (GameObject elem in GameObject.FindGameObjectsWithTag ("Tile")){
			GameObject.Destroy (elem);
		}
		MapTypeSelection sel = mapTypes[mapSelector];
		if(sel.GetTypeSel()==MapTypeSelectionTypes.Manual){
			map = new Matrix(MapParser.ReadMap(sel.GetTypeManual()));
		}else if(sel.GetTypeSel()==MapTypeSelectionTypes.Seeded){
			map=new Matrix(sel.GetTypeSeeded().maxim,sel.GetTypeSeeded().nullProbability,sel.GetTypeSeeded().seed);
		}else{
			System.Random rnd = new System.Random();
			map=new Matrix(rnd.Next(7,35),(float)rnd.NextDouble(),Guid.NewGuid().GetHashCode());
		}
		
		Vector2 size = MapDrawer.instantiateMap(map.getIterable(),-1000,0);
		size.x -= 1000;
		GameObject.FindGameObjectWithTag("PreviewCamera").GetComponent<PreviewCameraController>().SetSize(size);
		GameSelection.map=map;
	}
	private List<SeededMap> GetAllInterestingSeededMaps(){
		string stats = (Resources.Load ("levels") as TextAsset).text;
		JSONNode n = JSON.Parse (stats);
		List<SeededMap> allSeeded = new List<SeededMap> ();
		for (int i = 0; i < n.Count; i++) {
			JSONNode json = n[i.ToString()];
			SeededMap slimeData = new SeededMap(json);
			allSeeded.Add(slimeData);
		}
		//List<SeededMap> seededMap = new List<SeededMap>();
		//seededMap.Add(new SeededMap(11,0.3f,00000));
		//seededMap.Add(new SeededMap(25,0.4f,000011));
		//seededMap.Add(new SeededMap(35,0.5f,0007887));
		//seededMap.Add(new SeededMap(17,0.9f,000712341237));
		return allSeeded;
	}*/

	public void ModeSelection(int value){
		//GameObject s = GameObject.Find("ModeSelection");
		modoVictoria = value;
		GameSelection.modoVictoria = modoVictoria;
		float yPos = 0;
		switch(modoVictoria){
		case 0:
			//s.transform.Find ("TextMode").GetComponent<Text> ().text = Languages.GetString ("Death", "Death");
			yPos = GameObject.Find ("ModePanel1").GetComponent<RectTransform> ().anchoredPosition.y;
			break;
		case 1:
			//s.transform.Find("TextMode").GetComponent<Text>().text = Languages.GetString("Conquest","Conquest");
			yPos = GameObject.Find ("ModePanel2").GetComponent<RectTransform> ().anchoredPosition.y;
			break;
		case 2:
			//s.transform.Find("TextMode").GetComponent<Text>().text = Languages.GetString("Mass","Mass");
			yPos = GameObject.Find ("ModePanel3").GetComponent<RectTransform> ().anchoredPosition.y;
			break;
		}
		GameObject.Find ("Pin").GetComponent<RectTransform> ().anchoredPosition = new Vector3 (140, yPos, 0);
	}
}
/* enum MapTypeSelectionTypes{
	Random,
	Seeded,
	Manual
}
class SeededMap{
	public int seed;
	public float nullProbability;
	public int maxim;
	public SeededMap(int maxim, float nullProbability, int seed){
		this.maxim=maxim;
		this.nullProbability=nullProbability;
		this.seed=seed;
	}
	public SeededMap(JSONNode data){
		this.maxim = data["maxim"];
		this.nullProbability = data["nullProbability"];
		this.seed = data["seed"];		
	}
}

class MapTypeSelection{
	MapTypeSelectionTypes type;
	SeededMap seed;
	MapTypes manual;
	public MapTypeSelection(MapTypes typeMap){
		type=MapTypeSelectionTypes.Manual;
		manual=typeMap;
	}
	public MapTypeSelection(SeededMap seed){
		type=MapTypeSelectionTypes.Seeded;
		this.seed=seed;
	}
	public MapTypeSelection(){
		type=MapTypeSelectionTypes.Random;
	}
	public MapTypeSelectionTypes GetTypeSel(){
		return type;
	}
	public MapTypes GetTypeManual(){
		return manual;
	}
	public SeededMap GetTypeSeeded(){
		return seed;
	}
}*/