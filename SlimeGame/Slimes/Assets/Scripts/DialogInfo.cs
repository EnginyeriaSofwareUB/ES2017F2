using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogInfo : MonoBehaviour {
	
	public bool active;

	public void showhidePanel(){
		gameObject.SetActive (!gameObject.activeSelf);
		if (GameObject.Find ("Main Camera").GetComponent<GameController> ().GetSelectedSlime()!=null) {
			GameObject.Find ("Main Camera").GetComponent<GameController> ().DoAction (new SlimeAction (
				ActionType.EAT,
				GameObject.Find ("Main Camera").GetComponent<GameController> ().GetSelectedSlime()));
			Camera.main.GetComponent<UIController>().hideCurrentUITiles();
			GameObject.Find ("Main Camera").GetComponent<GameController> ().SetSelectedSlime (null);
		}

	}
		
}
