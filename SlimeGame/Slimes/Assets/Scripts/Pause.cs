using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour {
    public GameObject pausePanel;
    public GameObject settingsPanel;
	private float effects;
	private float music;
	private GameObject musicBar;
	private GameObject effectsBar;
	private SoundController sc;
	private string jsonData;
	private bool modifyingBar;

    // Use this for initialization
    void Start () {
		pausePanel = GameObject.Find("PausePanel");
        settingsPanel = GameObject.Find("SettingsPanel");
		settingsPanel.SetActive (false);
		pausePanel.SetActive (false);
		sc = GameObject.Find("Main Camera").GetComponent<SoundController>();
		jsonData = PlayerPrefs.GetString ("SettingsVolume");
		loadSet ();
    }
	
	// Update is called once per frame
	void Update () {
	}
		
    public void showHidePausePanel(){
		pausePanel.SetActive(!pausePanel.activeSelf);
		Text[] txs = FindObjectsOfType<Text>(); //agafem tots els que tenen text
		foreach (Text t in txs){
			//li posem de text el que tenen assignat segons l'idioma seleccionats
			Debug.Log(t.name);
			t.text = Languages.GetString(t.name,t.text);
		}
		if (!pausePanel.activeSelf){
            Time.timeScale = 1;
        } else {
            Time.timeScale = 0;
        }
    }

    public void showHideSettingsPanel() {
		settingsPanel.SetActive(!settingsPanel.activeSelf);
		Text[] txs = FindObjectsOfType<Text>(); //agafem tots els que tenen text
		foreach (Text t in txs){
			//li posem de text el que tenen assignat segons l'idioma seleccionats
			Debug.Log(t.name);
			t.text = Languages.GetString(t.name,t.text);
		}
		if (settingsPanel.activeSelf) {
			onValueChange (true);
		}
    }

	private void loadSet(){
		if (jsonData != null && !jsonData.Equals ("")) {
			Vector2 data = JsonUtility.FromJson<Vector2> (jsonData);
			music = data.x;
			effects = data.y;
			sc.updateMusicVolume (music);
			sc.updateEffectsVolume (effects);
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
		sc.updateMusicVolume (music);
		sc.updateEffectsVolume(effects);
	}
		
	public void onValueChange(bool modifyBar){
		if (modifyBar) {
			modifyingBar = true;
			GameObject.Find ("MusicSlider").GetComponent<Slider> ().value = music;
			GameObject.Find ("EffectsSlider").GetComponent<Slider> ().value = effects;
			modifyingBar = false;
		} else {
			if (!modifyingBar) {
				effects = GameObject.Find ("EffectsSlider").GetComponent<Slider> ().value;
				music = GameObject.Find ("MusicSlider").GetComponent<Slider> ().value;
				saveSet ();
			}
		}
	}

	public void Exit (){
		Time.timeScale = 1;
		GameSelection.playerColors.Clear ();
		GameSelection.playerCores.Clear ();
		GameSelection.playerIAs.Clear ();
		GameObject.Find ("ExitButton").GetComponent<LoadOnClick> ().LoadScene (0);
	}
}