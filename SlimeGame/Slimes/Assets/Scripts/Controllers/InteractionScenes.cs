using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractionScenes : MonoBehaviour {

	public void loadScene(int s){
		//cridar aquest quan s'hagi d'obrir l'scene de settings
		PlayerPrefs.SetString ("PreviousScene", SceneManager.GetActiveScene ().name);
		SceneManager.LoadScene (s);
	}

	public void loadBackScene(){
		//onclick del back de scene de settings
		string sceneName = PlayerPrefs.GetString ("PreviousScene");
		PlayerPrefs.SetString ("PreviousScene", SceneManager.GetActiveScene ().name);
		SceneManager.LoadScene (sceneName);
	}
}
