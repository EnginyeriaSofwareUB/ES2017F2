using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapParser : MonoBehaviour {

	// Use this for initialization
	void Start () {
		ReadMap ();
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log("Update of MapParser...", gameObject);
	}

	void ReadMap() {
		int[][] map;

		string path = "Assets/Resources/map.txt";

		System.IO.StreamReader reader = new System.IO.StreamReader (path);
		string line = reader.ReadLine ();
		while (line != null) {
			string[] elements = line.Split (' ');
			if (elements.Length > 0) {
				// Convertim string a enter i l'afegim al array.
			}
			line = reader.ReadLine ();
		}
		reader.Close();
	}
}
