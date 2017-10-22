using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsController : MonoBehaviour {
	public bool effects, music;
	private bool firstTime;
	private Image image;
	private List<string> spritePaths;
	private int slimeSelector1;
	private int slimeSelector2;
	// Use this for initialization
	void Start () {
		/*
		firstTime = true;
		GameObject.Find ("Effects").GetComponent<Toggle> ().onValueChanged.RemoveAllListeners ();
		GameObject.Find ("Music").GetComponent<Toggle> ().onValueChanged.RemoveAllListeners ();
		string jsonData = PlayerPrefs.GetString ("SettingsVolume");
		if (jsonData != null) {
			loadSettings (jsonData);
		} else {
			effects = true;
			music = true;
			saveSettings ();
		}

		GameObject.Find ("Effects").GetComponent<Toggle> ().isOn = effects;
		GameObject.Find ("Music").GetComponent<Toggle> ().isOn = music;
		firstTime = false;
*/
		slimeSelector1 = 0;
		slimeSelector2 = 1;
		spritePaths = new List<string> ();
		spritePaths.Add ("Test/slime");
		spritePaths.Add ("Test/slime2");
		spritePaths.Add ("Test/slime3");
		GameObject.Find ("Sprite1").GetComponent<Image> ().overrideSprite = Resources.Load<Sprite> (spritePaths [slimeSelector1]);
		GameObject.Find ("Sprite2").GetComponent<Image> ().overrideSprite = Resources.Load<Sprite> (spritePaths [slimeSelector2]);
	}

	public void onClickToggle(){
		if (!firstTime) {
			GameObject[] list = GameObject.FindGameObjectsWithTag ("Setting");
			foreach (GameObject go in list) {
				if (go.name.Equals ("Effects")) {
					effects = go.GetComponent<Toggle> ().isOn;
				} else if (go.name.Equals ("Music")) {
					music = go.GetComponent<Toggle> ().isOn;
				}
			}
			saveSettings ();
		}
	}

	public void nextSprite1(){
		if (slimeSelector1 == spritePaths.Count - 1) {
			slimeSelector1 = 0;
		} else {
			slimeSelector1++;
		}
		if (slimeSelector1 == slimeSelector2) {
			slimeSelector1++;
		}
		GameObject.Find ("Sprite1").GetComponent<Image> ().overrideSprite = Resources.Load<Sprite> (spritePaths[slimeSelector1]);
	}	

	public void nextSprite2(){
		if (slimeSelector2 == spritePaths.Count - 1) {
			slimeSelector2 = 0;
		} else {
			slimeSelector2++;
		}
		if (slimeSelector1 == slimeSelector2) {
			slimeSelector2++;
		}
		GameObject.Find ("Sprite2").GetComponent<Image> ().overrideSprite = Resources.Load<Sprite> (spritePaths[slimeSelector2]);
	}	

	public void prevSprite1(){
		if (slimeSelector1 == 0) {
			slimeSelector1 = spritePaths.Count - 1;
		} else {
			slimeSelector1--;
		}
		if (slimeSelector1 == slimeSelector2) {
			slimeSelector1--;
		}
		GameObject.Find ("Sprite1").GetComponent<Image> ().overrideSprite = Resources.Load<Sprite> (spritePaths[slimeSelector1]);
	}	

	public void prevSprite2(){
		if (slimeSelector2 == 0) {
			slimeSelector2 = spritePaths.Count - 1;
		} else {
			slimeSelector2--;
		}
		if (slimeSelector1 == slimeSelector2) {
			slimeSelector2--;
		}
		GameObject.Find ("Sprite2").GetComponent<Image> ().overrideSprite = Resources.Load<Sprite> (spritePaths[slimeSelector2]);
	}	

	private void saveSettings(){
		string jsonData = JsonUtility.ToJson (this);
		Debug.Log ("Saving "+jsonData);
		PlayerPrefs.SetString ("SettingsVolume", jsonData);
		PlayerPrefs.Save ();
	}

	private void loadSettings(string jsonData){
		SaveSettings loadedData = JsonUtility.FromJson<SaveSettings> (jsonData);
		effects = loadedData.effects;
		music = loadedData.music;
		Debug.Log ("Loading: "+jsonData);
	}
}
	
public class SaveSettings{
	public bool effects = true;
	public bool music = true;
}
