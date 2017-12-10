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
	//private List<string> coresInfo;
	private List<MapTypeSelection> mapTypes;
	private List<Color> colors;
	private List<int> colorSelector;
	private List<int> coreSelector;
	private int currentColorSelector;
	private int mapSelector;
	private int modoVictoria;

	private int currentPlayer;
	private int maxPlayers;

	private GameObject player3;
	private GameObject player4;

	public Matrix map;

	// Use this for initialization
	void Start () {
		maxPlayers = 2;
		currentPlayer = 1;
		modoVictoria = 0;
		coreSelector = new List<int>();
		coreSelector.Add(0);
		coreSelector.Add(0);
		coreSelector.Add(1);
		mapSelector = 0;
		colorSelector = new List<int>();
		colorSelector.Add(0);
		colorSelector.Add(0);
		colorSelector.Add(1);
		mapTypes = new List<MapTypeSelection> ();
		/*foreach(MapTypes type in Enum.GetValues(typeof(MapTypes))){
			mapTypes.Add(new MapTypeSelection(type));
		}*/
		foreach(SeededMap seed in GetAllInterestingSeededMaps()){
			mapTypes.Add(new MapTypeSelection(seed));
		}
		player3 = GameObject.Find ("Panel3");
		player4 = GameObject.Find("Panel4");
		player3.SetActive (false);
		player4.SetActive (false);

		mapTypes.Add(new MapTypeSelection());
		corePaths = new List<string> ();
		corePaths.Add ("Sprites/Wrath");
		corePaths.Add ("Sprites/Sloth");
		corePaths.Add ("Sprites/Gluttony");
        sprite = "Sprites/slime_sprite";
		GameObject.Find ("CurrentSprite").GetComponent<Image> ().overrideSprite = SpritesLoader.GetInstance().GetResource(sprite);
		GameObject.Find ("Sprite1").GetComponent<Image> ().overrideSprite = SpritesLoader.GetInstance().GetResource(sprite);
		GameObject.Find ("Sprite2").GetComponent<Image> ().overrideSprite = SpritesLoader.GetInstance().GetResource(sprite);
        GameObject.Find("CurrentCore").GetComponent<Image>().overrideSprite = SpritesLoader.GetInstance().GetResource(corePaths[coreSelector[1]]);
		GameObject.Find("Core1").GetComponent<Image>().overrideSprite = SpritesLoader.GetInstance().GetResource(corePaths[coreSelector[1]]);
        GameObject.Find("Core2").GetComponent<Image>().overrideSprite = SpritesLoader.GetInstance().GetResource(corePaths[coreSelector[2]]);
        colors = new List<Color> ();
		colors.Add (new Color (1, 0, 0));
		colors.Add (new Color (0, 0, 1));
		colors.Add (new Color (0, 1, 0));
		colors.Add (new Color (1, 1, 0));
		colors.Add (new Color (0, 1, 1));
		colors.Add (new Color (1, 0, 1));
		colors.Add (new Color (1, 1, 1));
		//coresInfo = new List<string> ();
		//coresInfo.Add ("Info de SlimeCore0");
		//coresInfo.Add ("Info de SlimeCore1");
		//coresInfo.Add ("Info de SlimeCore2");
		GameObject.Find ("CurrentSprite").GetComponent<Image> ().color = colors[colorSelector[1]];
		GameObject.Find ("Sprite1").GetComponent<Image> ().color = colors[colorSelector[1]];
		GameObject.Find ("Sprite2").GetComponent<Image> ().color = colors[colorSelector[2]];

		//GameObject.Find ("CoreInfo").GetComponent<Text> ().text = coresInfo [coreSelector[1]];
		loadMap ();

		GameSelection.player1Color = colors [colorSelector[1]];
		GameSelection.player2Color = colors [colorSelector[2]];
		GameSelection.player1Core = coreSelector[1];
		GameSelection.player2Core = coreSelector[2];
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
				GameObject.Find ("Sprite3").GetComponent<Image> ().overrideSprite = SpritesLoader.GetInstance().GetResource(sprite);
				if (coreSelector.Count < 4) {
					coreSelector.Add (2);
					colorSelector.Add (2);
				}
				GameObject.Find("Core3").GetComponent<Image>().overrideSprite = SpritesLoader.GetInstance().GetResource(corePaths[coreSelector[3]]);
				GameObject.Find ("Sprite3").GetComponent<Image> ().color = colors[colorSelector[3]];
				//Asegurarse de que el color elegido no lo tienen otros jugadores!
			} else if (maxPlayers == 4) {
				player4.SetActive (true);
				GameObject.Find ("Sprite4").GetComponent<Image> ().overrideSprite = SpritesLoader.GetInstance().GetResource(sprite);
				if (coreSelector.Count < 5) {
					coreSelector.Add (0);
					colorSelector.Add (3);
				}
				GameObject.Find("Core4").GetComponent<Image>().overrideSprite = SpritesLoader.GetInstance().GetResource(corePaths[coreSelector[4]]);
				GameObject.Find ("Sprite4").GetComponent<Image> ().color = colors[colorSelector[4]];
			}
		}
	}

	public void deletePlayer(){
		if (maxPlayers > 2) {
			if (currentPlayer == maxPlayers)
				changeCurrentPlayer(1);
			if (maxPlayers == 4)
				player4.SetActive (false);
			else if (maxPlayers == 3)
				player3.SetActive (false);
			maxPlayers--;
		}
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
		int dmg = stats.attack;
		int hp = stats.startingHP;
		int rng = stats.range;
		int move = stats.move;
		GameObject.Find ("CoreInfo").GetComponent<Text> ().text = "  " + hp + "     " + dmg + "\n   " + rng + "      " + move;
	}
	public void changeCurrentPlayer(int cursor){
		currentPlayer = cursor;
		GameObject.Find ("CurrentCore").GetComponent<Image> ().overrideSprite = SpritesLoader.GetInstance ().GetResource (corePaths [coreSelector [cursor]]);
		GameObject.Find ("CurrentSprite").GetComponent<Image> ().color = GameObject.Find ("Sprite"+cursor).GetComponent<Image> ().color;
		//GameObject.Find ("CoreInfo").GetComponent<Text> ().text = coresInfo [coreSelector [currentPlayer]];
		changeInfo();
		GameObject.Find ("PlayerText").GetComponent<Text> ().text = "PLAYER " + cursor;
	}

	public void changeCore(int cursor){
		coreSelector[currentPlayer] += cursor;
		if (coreSelector[currentPlayer] > corePaths.Count - 1) {
			coreSelector[currentPlayer] = 0;
		} else if (coreSelector[currentPlayer] < 0) {
			coreSelector[currentPlayer] = corePaths.Count - 1;
		}
		GameObject.Find ("CurrentCore").GetComponent<Image> ().overrideSprite = SpritesLoader.GetInstance().GetResource(corePaths [coreSelector[currentPlayer]]);
		changeInfo ();
		//GameObject.Find ("CoreInfo").GetComponent<Text> ().text = coresInfo [coreSelector[currentPlayer]];
		GameObject.Find("Core"+currentPlayer).GetComponent<Image> ().overrideSprite = SpritesLoader.GetInstance().GetResource(corePaths [coreSelector[currentPlayer]]);

		if (currentPlayer == 1) {
			GameSelection.player1Core = coreSelector[currentPlayer];
		} else if (currentPlayer == 2) {
			GameSelection.player2Core = coreSelector[currentPlayer];
		} 
	}

	public void changeColor(int cursor){
		colorSelector[currentPlayer] += cursor;
		if (colorSelector[currentPlayer] > colors.Count - 1) {
			colorSelector[currentPlayer] = 0;
		} else if (colorSelector[currentPlayer] < 0) {
			colorSelector[currentPlayer] = colors.Count - 1;
		}	
		if (colorSelector[1] == colorSelector[2]) {//Casos con 3 y 4 jugadores
			colorSelector[currentPlayer] += cursor;
			if (colorSelector[currentPlayer] > colors.Count - 1) {
				colorSelector[currentPlayer] = 0;
			} else if (colorSelector[currentPlayer] < 0) {
				colorSelector[currentPlayer] = colors.Count - 1;
			}
		}
		GameObject.Find ("CurrentSprite").GetComponent<Image> ().color = colors[colorSelector[currentPlayer]];
		GameObject.Find ("Sprite"+currentPlayer).GetComponent<Image> ().color = colors[colorSelector[currentPlayer]];

		if (currentPlayer == 1) {
			GameSelection.player1Color = colors[colorSelector[currentPlayer]];
		} else if (currentPlayer == 2) {
			GameSelection.player2Color = colors[colorSelector[currentPlayer]];
		} 
	}	

	public void changeMap(int cursor){
		mapSelector+=cursor;
		if (mapSelector > mapTypes.Count-1) {
			mapSelector = 0;
		} else if (mapSelector < 0) {
			mapSelector = mapTypes.Count - 1;
		}
		loadMap ();
	}

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
	}

	public void ModeSelection(){
		GameObject s = GameObject.Find("ModeSelection");
		modoVictoria = (int)s.GetComponent<Slider>().value;
		GameSelection.modoVictoria = modoVictoria;
		switch(modoVictoria){
			case 0:
				s.transform.Find("TextMode").GetComponent<Text>().text = "Asesinato";
				break;
			case 1:
				s.transform.Find("TextMode").GetComponent<Text>().text = "Conquista";
				break;
			case 2:
				s.transform.Find("TextMode").GetComponent<Text>().text = "Masa";
				break;
		}
	}
}
 enum MapTypeSelectionTypes{
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
}