using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsController : MonoBehaviour {
	public float effects_volume_1, music_volume_1;
	// Use this for initialization
	void Start () {
		string jsonData = PlayerPrefs.GetString ("SettingsVolume");
		if (jsonData != null) {
			loadSettings (jsonData);
		} else {
			effects_volume_1 = 1;
			music_volume_1 = 1;
			saveSettings ();
		}
		GameObject.Find ("VolumeEffects").GetComponent<Slider> ().value = effects_volume_1;
		GameObject.Find ("VolumeMusic").GetComponent<Slider> ().value = music_volume_1;
	}

	public void applySettings(){
		effects_volume_1 = GameObject.Find ("VolumeEffects").GetComponent<Slider> ().value;
		music_volume_1 = GameObject.Find ("VolumeMusic").GetComponent<Slider> ().value;
		saveSettings ();
		returnScene ();
	}

	public void cancelSettings(){
		returnScene ();
	}

	private void returnScene(){
		SceneManager.LoadScene(1);
	}

	private void saveSettings(){
		string jsonData = JsonUtility.ToJson (this);
		PlayerPrefs.SetString ("SettingsVolume", jsonData);
		PlayerPrefs.Save ();
	}

	private void loadSettings(string jsonData){
		SaveSettings loadedData = JsonUtility.FromJson<SaveSettings> (jsonData);
		effects_volume_1 = loadedData.effects_volume_1;
		music_volume_1 = loadedData.music_volume_1;
	}
}
	
public class SaveSettings{
	public float effects_volume_1 = 1;
	public float music_volume_1 = 1;
}
