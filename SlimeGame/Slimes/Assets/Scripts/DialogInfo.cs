using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogInfo : MonoBehaviour {
	
	public GameObject Panel;
	private int counter;

	public void showhidePanel(){
		counter++;
		counter = counter % 2;
		if (counter % 2 == 1) {
			Panel.gameObject.SetActive (false);
		} else {
			Panel.gameObject.SetActive (true);
		}

	}

	public void Active(bool b){
		Panel.gameObject.SetActive (true);
	}

}
