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
	private string jsonData2;
	private bool modifyingBar;
	public bool ultimateSlime;
	// Use this for initialization
	void Start () {
		
		jsonData = PlayerPrefs.GetString ("SettingsVolume");
		PlayerPrefs.SetString ("SettingsVolume", jsonData);
		loadSet ();
		GameObject.Find ("Background Not 1").GetComponent<RectTransform> ().localScale = new Vector3 (music, music, 0);
		GameObject.Find ("Background Not 2").GetComponent<RectTransform> ().localScale = new Vector3 (music, music, 0);
		GameObject.Find ("Background Not 3").GetComponent<RectTransform> ().localScale = new Vector3 (music, music, 0);
		GameObject.Find ("Background Exc 1").GetComponent<RectTransform> ().localScale = new Vector3 (effects,effects,0);
		GameObject.Find ("Background Exc 2").GetComponent<RectTransform> ().localScale = new Vector3 (effects,effects,0);
		GameObject.Find ("Background Exc 3").GetComponent<RectTransform> ().localScale = new Vector3 (effects,effects,0);
		onValueChange (true);
		//GameObject.Find(Languages.GetLanguage()).GetComponent<Button>().Select();
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
				resizeImages ();
				saveSet ();
			}
		}
	}
	public void toggleUltimate(){
		ultimateSlime = !ultimateSlime;
		saveSet ();
	}

	private void resizeImages(){
		GameObject.Find ("Background Not 1").GetComponent<RectTransform> ().localScale = new Vector3 (music, music, 0);
		GameObject.Find ("Background Not 2").GetComponent<RectTransform> ().localScale = new Vector3 (music, music, 0);
		GameObject.Find ("Background Not 3").GetComponent<RectTransform> ().localScale = new Vector3 (music, music, 0);
		GameObject.Find ("Background Exc 1").GetComponent<RectTransform> ().localScale = new Vector3 (effects,effects,0);
		GameObject.Find ("Background Exc 2").GetComponent<RectTransform> ().localScale = new Vector3 (effects,effects,0);
		GameObject.Find ("Background Exc 3").GetComponent<RectTransform> ().localScale = new Vector3 (effects,effects,0);
	}

	private void loadSet(){
		if (jsonData != null && !jsonData.Equals ("")) {
			Vector2 data = JsonUtility.FromJson<Vector2> (jsonData);
			int intData = PlayerPrefs.GetInt ("ultimate");
			music = data.x;
			effects = data.y;
			ultimateSlime = intData==1;
		} else {
			music = 1.0f;
			effects = 1.0f;
			ultimateSlime = false;
			saveSet ();
		}
	}
		
	private void saveSet(){
		int intData;
		if (ultimateSlime) {
			intData = 1;
		} else {
			intData = 0;
		}
		Vector2 data = new Vector2 (music, effects);
		jsonData = JsonUtility.ToJson (data);

		PlayerPrefs.SetString ("SettingsVolume", jsonData);
		PlayerPrefs.SetInt ("ultimate", intData);
		PlayerPrefs.Save ();
	}

	public void DefineLanguage(string language){
		Languages.DefineLanguage(language);
		Text[] txs = FindObjectsOfType<Text>(); //agafem tots els que tenen text
		foreach (Text t in txs){
			//li posem de text el que tenen assignat segons l'idioma seleccionats
			t.text = Languages.GetString(t.name,t.text);
		}
	}
}