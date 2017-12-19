using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractionScenes : MonoBehaviour {

	SoundController soundController;

	public void Start(){
		soundController = gameObject.GetComponent<SoundController>();
        AudioClip clip = SoundsLoader.GetInstance().GetResource("Sounds/HappySong");
        soundController.PlayLoop(clip);
	}

	public void loadScene(int s){
		//cridar aquest quan s'hagi d'obrir l'scene de settings
		PlayerPrefs.SetString ("PreviousScene", SceneManager.GetActiveScene ().name);
		SceneManager.LoadScene (s);
	}

	public void loadBackScene(){
		//onclick del back de scene de settings
		string sceneName = PlayerPrefs.GetString ("PreviousScene");
		Debug.Log(sceneName);
		if (sceneName != null && !sceneName.Equals("") && !sceneName.Equals("Settings")){
			PlayerPrefs.SetString ("PreviousScene", SceneManager.GetActiveScene ().name);
			SceneManager.LoadScene (sceneName);
		} else {
			PlayerPrefs.SetString ("PreviousScene", SceneManager.GetActiveScene ().name);
			SceneManager.LoadScene ("MainMenu");
		}
	}
}
