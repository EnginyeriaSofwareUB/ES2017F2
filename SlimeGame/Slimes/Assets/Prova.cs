using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prova : MonoBehaviour {

	// Use this for initialization
	void Start () {
		/*int[][] map = MapParser.ReadMap (MapTypes.Small);
		Debug.Log ("Rows: " + map.Length + "First row columns: " + map [0].Length);*/

		List<List<TileTypeEnum>> map = MapParser.ReadMap (MapTypes.Big);
		/*foreach(List<TileTypeEnum> row in map){
			foreach (TileTypeEnum tile in row) {
				Debug.Log (tile.ToString());
			}
		}*/

		Matrix realmatrix = new Matrix(map);
		realmatrix.print();		

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
