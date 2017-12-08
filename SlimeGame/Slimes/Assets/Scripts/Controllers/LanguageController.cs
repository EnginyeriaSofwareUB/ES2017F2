using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Text[] txs = FindObjectsOfType<Text>(); //agafem tots els que tenen text
		foreach (Text t in txs){
			//li posem de text el que tenen assignat segons l'idioma seleccionats
			t.text = Languages.GetString(t.name,t.text);
		}
	}
}
