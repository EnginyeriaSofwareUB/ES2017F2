using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectorController : MonoBehaviour {
    private string sprite;
	private List<string> corePaths;
	private List<string> coresInfo;
	private List<MapTypes> mapTypes;
	private List<Color> colors;
	private int slimeSelector1;
	private int slimeSelector2;
	private int colorSelector1;
	private int colorSelector2;
	private int mapSelector;


	// Use this for initialization
	void Start () {
		slimeSelector1 = 0;
		slimeSelector2 = 1;
		mapSelector = 2;
		colorSelector1 = 0;
		colorSelector2 = 1;
		mapTypes = new List<MapTypes> ();
		mapTypes.Add (MapTypes.Small);
		mapTypes.Add (MapTypes.Medium);
		mapTypes.Add (MapTypes.Big);
		corePaths = new List<string> ();
		corePaths.Add ("Sprites/Wrath");
		corePaths.Add ("Sprites/Sloth");
		corePaths.Add ("Sprites/Gluttony");
        sprite = "Sprites/slime_sprite";
		GameObject.Find ("Sprite1").GetComponent<Image> ().overrideSprite = Resources.Load<Sprite> (sprite);
		GameObject.Find ("Sprite2").GetComponent<Image> ().overrideSprite = Resources.Load<Sprite> (sprite);
        GameObject.Find("Core1").GetComponent<Image>().overrideSprite = Resources.Load<Sprite>(corePaths[slimeSelector1]);
        GameObject.Find("Core2").GetComponent<Image>().overrideSprite = Resources.Load<Sprite>(corePaths[slimeSelector2]);
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
		GameObject.Find ("Core1").GetComponent<Image> ().overrideSprite = Resources.Load<Sprite> (corePaths [slimeSelector1]);
		GameObject.Find ("Text1").GetComponent<Text> ().text = coresInfo [slimeSelector1];
	}

	public void changeCore2(int cursor){
		slimeSelector2 += cursor;
		if (slimeSelector2 > corePaths.Count - 1) {
			slimeSelector2 = 0;
		} else if (slimeSelector2 < 0){
			slimeSelector2 = corePaths.Count - 1;
		}
		GameObject.Find ("Core2").GetComponent<Image> ().overrideSprite = Resources.Load<Sprite> (corePaths [slimeSelector2]);
		GameObject.Find ("Text2").GetComponent<Text> ().text = coresInfo [slimeSelector2];
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
		MapDrawer.instantiateMap(new Matrix(MapParser.ReadMap(mapTypes[mapSelector])).getIterable(),-1000,0);
	}

}
