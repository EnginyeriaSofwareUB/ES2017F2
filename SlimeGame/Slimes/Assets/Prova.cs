using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prova : MonoBehaviour {

	// Use this for initialization
	void Start () {
		int[][] map = MapParser.ReadMap (MapTypes.Small);
		Debug.Log ("Rows: " + map.Length + "First row columns: " + map [0].Length);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
