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
	private List<string> coresInfo;
	private List<MapTypeSelection> mapTypes;
	private List<Color> colors;
	private int slimeSelector1;
	private int slimeSelector2;
	private int colorSelector1;
	private int colorSelector2;
	private int mapSelector;
	private int modoVictoria;

	public Matrix map;

	// Use this for initialization
	void Start () {
		modoVictoria = 0;
		slimeSelector1 = 0;
		slimeSelector2 = 1;
		mapSelector = 0;
		colorSelector1 = 0;
		colorSelector2 = 1;
		mapTypes = new List<MapTypeSelection> ();
		/*foreach(MapTypes type in Enum.GetValues(typeof(MapTypes))){
			mapTypes.Add(new MapTypeSelection(type));
		}*/
		foreach(SeededMap seed in GetAllInterestingSeededMaps()){
			mapTypes.Add(new MapTypeSelection(seed));
		}
		mapTypes.Add(new MapTypeSelection());
		corePaths = new List<string> ();
		corePaths.Add ("Sprites/Wrath");
		corePaths.Add ("Sprites/Sloth");
		corePaths.Add ("Sprites/Gluttony");
        sprite = "Sprites/slime_sprite";
		GameObject.Find ("Sprite1").GetComponent<Image> ().overrideSprite = SpritesLoader.GetInstance().GetResource(sprite);
		GameObject.Find ("Sprite2").GetComponent<Image> ().overrideSprite = SpritesLoader.GetInstance().GetResource(sprite);
        GameObject.Find("Core1").GetComponent<Image>().overrideSprite = SpritesLoader.GetInstance().GetResource(corePaths[slimeSelector1]);
        GameObject.Find("Core2").GetComponent<Image>().overrideSprite = SpritesLoader.GetInstance().GetResource(corePaths[slimeSelector2]);
        colors = new List<Color> ();
		colors.Add (new Color (1, 0, 0));
		colors.Add (new Color (0, 0, 1));
		colors.Add (new Color (0, 1, 0));
		colors.Add (new Color (1, 1, 0));
		colors.Add (new Color (0, 1, 1));
		colors.Add (new Color (1, 0, 1));
		colors.Add (new Color (1, 1, 1));
		coresInfo = new List<string> ();
		coresInfo.Add ("Info de SlimeCore0");
		coresInfo.Add ("Info de SlimeCore1");
		coresInfo.Add ("Info de SlimeCore2");
		GameObject.Find ("Color1").GetComponent<Image> ().color = colors[colorSelector1];
		GameObject.Find ("Sprite1").GetComponent<Image> ().color = colors[colorSelector1];
		GameObject.Find ("Color2").GetComponent<Image> ().color = colors[colorSelector2];
		GameObject.Find ("Sprite2").GetComponent<Image> ().color = colors[colorSelector2];
		GameObject.Find ("Text1").GetComponent<Text> ().text = coresInfo [slimeSelector1];
		GameObject.Find ("Text2").GetComponent<Text> ().text = coresInfo [slimeSelector2];
		loadMap ();

		GameSelection.player1Color = colors [colorSelector1];
		GameSelection.player2Color = colors [colorSelector2];
		GameSelection.player1Core = slimeSelector1;
		GameSelection.player2Core = slimeSelector2;
		GameSelection.modoVictoria = modoVictoria;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void changeCore1(int cursor){
		slimeSelector1 += cursor;
		if (slimeSelector1 > corePaths.Count - 1) {
			slimeSelector1 = 0;
		} else if (slimeSelector1 < 0) {
			slimeSelector1 = corePaths.Count - 1;
		}
		GameObject.Find ("Core1").GetComponent<Image> ().overrideSprite = SpritesLoader.GetInstance().GetResource(corePaths [slimeSelector1]);
		GameObject.Find ("Text1").GetComponent<Text> ().text = coresInfo [slimeSelector1];
		GameSelection.player1Core = slimeSelector1;
	}

	public void changeCore2(int cursor){
		slimeSelector2 += cursor;
		if (slimeSelector2 > corePaths.Count - 1) {
			slimeSelector2 = 0;
		} else if (slimeSelector2 < 0){
			slimeSelector2 = corePaths.Count - 1;
		}
		GameObject.Find ("Core2").GetComponent<Image> ().overrideSprite = SpritesLoader.GetInstance().GetResource(corePaths [slimeSelector2]);
		GameObject.Find ("Text2").GetComponent<Text> ().text = coresInfo [slimeSelector2];
		GameSelection.player2Core = slimeSelector2;
	}

	public void changeColor1(int cursor){
		colorSelector1 += cursor;
		if (colorSelector1 > colors.Count - 1) {
			colorSelector1 = 0;
		} else if (colorSelector1 < 0) {
			colorSelector1 = colors.Count - 1;
		}	
		if (colorSelector1 == colorSelector2) {
			colorSelector1 += cursor;
			if (colorSelector1 > colors.Count - 1) {
				colorSelector1 = 0;
			} else if (colorSelector1 < 0) {
				colorSelector1 = colors.Count - 1;
			}
		}
		GameObject.Find ("Color1").GetComponent<Image> ().color = colors[colorSelector1];
		GameObject.Find ("Sprite1").GetComponent<Image> ().color = colors[colorSelector1];
		GameSelection.player1Color = colors [colorSelector1];
	}	

	public void changeColor2(int cursor){
		colorSelector2 += cursor;
		if (colorSelector2 > colors.Count - 1) {
			colorSelector2 = 0;
		} else if (colorSelector2 < 0) {
			colorSelector2 = colors.Count - 1;
		}	
		if (colorSelector1 == colorSelector2) {
			colorSelector2 += cursor;
			if (colorSelector2 > colors.Count - 1) {
				colorSelector2 = 0;
			} else if (colorSelector2 < 0) {
				colorSelector2 = colors.Count - 1;
			}
		}
		GameObject.Find ("Color2").GetComponent<Image> ().color = colors[colorSelector2];
		GameObject.Find ("Sprite2").GetComponent<Image> ().color = colors[colorSelector2];
		GameSelection.player2Color = colors [colorSelector2];
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
