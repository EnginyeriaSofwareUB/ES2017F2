using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsController : MonoBehaviour {
	public bool effects, music;
	// Use this for initialization
	void Start () {
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
		//GameObject.Find ("Effects").GetComponent<Toggle> ().onValueChanged.AddListener(onClickToggle);
	}

	public void onClickToggle(){
		GameObject[] list = GameObject.FindGameObjectsWithTag("Setting");
		foreach(GameObject go in list){
			if (go.name.Equals ("Effects")) {
				effects = go.GetComponent<Toggle> ().isOn;
			} else if (go.name.Equals ("Music")) {
				music = go.GetComponent<Toggle> ().isOn;
			}
		}
		saveSettings ();
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
