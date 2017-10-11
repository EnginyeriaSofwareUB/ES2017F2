using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadOnClick : MonoBehaviour {

	//Cargar una escena a partir de su numero de escena
	public void LoadScene(int sceneNumber){
		SceneManager.LoadScene(sceneNumber);
	}	
}
