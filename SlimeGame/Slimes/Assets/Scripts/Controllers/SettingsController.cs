using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsController : MonoBehaviour {
	public float effects, music;
	private bool firstTime;
	private int previousScene;
	// Use this for initialization
	void Start () {
		
		firstTime = true;
		//GameObject.Find ("Effects").GetComponent<Toggle> ().onValueChanged.RemoveAllListeners ();
		//GameObject.Find ("Music").GetComponent<Toggle> ().onValueChanged.RemoveAllListeners ();
		string jsonData = PlayerPrefs.GetString ("SettingsVolume");
		if (jsonData != null) {
			loadSettings (jsonData);
		} else {
			effects = 1;
			music = 1;
			saveSettings ();
		}

		GameObject.Find ("Effects").GetComponent<Slider> ().value = effects;
		GameObject.Find ("Music").GetComponent<Slider> ().value = music;
		firstTime = false;

	}

	public void onValueChange(){
		if (!firstTime) {
			GameObject[] list = GameObject.FindGameObjectsWithTag ("Setting");
			foreach (GameObject go in list) {
				if (go.name.Equals ("Effects")) {
					effects = go.GetComponent<Slider> ().value;
				} else if (go.name.Equals ("Music")) {
					music = go.GetComponent<Slider> ().value;
				}
			}
			saveSettings ();
		}
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
	public float effects = 1;
	public float music = 1;
}
