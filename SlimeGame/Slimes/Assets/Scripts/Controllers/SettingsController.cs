using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsController : MonoBehaviour {
	public float effects, music;
	private bool firstTime;
	private int previousScene;
	private string jsonData;
	private bool modifyingBar;
	// Use this for initialization
	void Start () {
		
		jsonData = PlayerPrefs.GetString ("SettingsVolume");
		PlayerPrefs.SetString ("SettingsVolume", jsonData);
		loadSet ();
		onValueChange (true);
	}

	public void onValueChange(bool modifyBar){
		if (modifyBar) {
			modifyingBar = true;
			GameObject.Find ("Music").GetComponent<Slider> ().value = music;
			GameObject.Find ("Effects").GetComponent<Slider> ().value = effects;
			modifyingBar = false;
		} else {
			if (!modifyingBar) {
				effects = GameObject.Find ("Effects").GetComponent<Slider> ().value;
				music = GameObject.Find ("Music").GetComponent<Slider> ().value;
				saveSet ();
			}
		}
	}

	private void loadSet(){
		if (jsonData != null && !jsonData.Equals ("")) {
			Vector2 data = JsonUtility.FromJson<Vector2> (jsonData);
			music = data.x;
			effects = data.y;
		} else {
			music = 1.0f;
			effects = 1.0f;
			saveSet ();
		}
	}

	private void saveSet(){
		Vector2 data = new Vector2 (music, effects);
		jsonData = JsonUtility.ToJson (data);
		PlayerPrefs.SetString ("SettingsVolume", jsonData);
		PlayerPrefs.Save ();
	}
}