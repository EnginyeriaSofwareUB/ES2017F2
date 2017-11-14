using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogInfo : MonoBehaviour {
	
	public bool active;

	public void showhidePanel(){
		Active (!active);
	}

	public void Active(bool b){
		gameObject.SetActive (b);
		active = b;
	}

}
