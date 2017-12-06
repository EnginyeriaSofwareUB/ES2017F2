using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour {

    public bool pauseActive;
    public bool settingsActive;

    public GameObject pausePanel;
    public GameObject settingsPanel;
	private float effects;
	private float music;
	private GameObject musicBar;
	private GameObject effectsBar;

	//private string jsonData;

    // Use this for initialization
    void Start () {
		pausePanel = GameObject.Find("PausePanel");
        pauseActive = false;
        settingsPanel = GameObject.Find("SettingsPanel");
        settingsActive = false;
        showHideSettingsPanel();
        showHidePausePanel();
		/*jsonData = JsonUtility.ToJson(this);
		PlayerPrefs.SetString("SettingsVolume", jsonData);
		loadSettings ("SettingsVolume");*/
		//effects = 1;
		//music = 1;
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void showHidePausePanel(){
        if (!pauseActive){
            Time.timeScale = 1;
        } else {
            Time.timeScale = 0;
        }
        pausePanel.SetActive(pauseActive);
        pauseActive = !pauseActive;
    }

    public void showHideSettingsPanel() {
		musicBar = GameObject.Find ("Music");
		effectsBar = GameObject.Find ("Effects");
		if (!settingsActive){
			//loadSettings (jsonData);
			effectsBar.GetComponent<Slider> ().value = effects;
			musicBar.GetComponent<Slider> ().value = music;
		}
        SoundController sc = GameObject.Find("Main Camera").GetComponent<SoundController>();
        settingsPanel.SetActive(settingsActive);
        settingsActive = !settingsActive;
		if (!settingsActive){
			//saveSettings();
			sc.updateMusicVolume(music);
			sc.updateEffectsVolume(effects);
		}
    }

	/*
	//private void saveSettings() {
		string jsonData = JsonUtility.ToJson (this);
		PlayerPrefs.SetString ("SettingsVolume", jsonData);
		PlayerPrefs.Save ();
	}*/
	/*
	private void loadSettings(string jsonData) {
		SaveSettings loadedData = JsonUtility.FromJson<SaveSettings> (jsonData);
		effects = loadedData.effects;
		music = loadedData.music;
	}*/
		
	public void onValueChange(){
		effects = effectsBar.GetComponent<Slider> ().value;
		music = musicBar.GetComponent<Slider> ().value;
		//saveSettings ();
	}

}
