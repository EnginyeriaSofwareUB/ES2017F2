using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		List<List<TileTypeEnum>> matrix = getNewMatrix();
		/*Matrix realmatrix = new Matrix(matrix);
		realmatrix.print();
		realmatrix.getNeighbours (-2, 5);
		realmatrix.getNeighbours (0, 3);*/
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public static List<List<TileTypeEnum>> getNewMatrix(){
		List<List<TileTypeEnum>> matrix = new List<List<TileTypeEnum>>();
		//matrix = [[0,1,1,1],[0,1,1], [0,1,1,1],[0,1,1],[0,1,1,1],[0,1,1],[0,1,1,1]]
		int maxRowLen = 7;
		int maxCols = 13;
		for(int i = 0; i<maxCols;i++){
			List<TileTypeEnum> row = new List<TileTypeEnum>();
			for(int j = 0; j<maxRowLen-((i)%2);j++){
				TileTypeEnum type = TileTypeEnum.Water;
				row.Add(type);
			}
			matrix.Add(row);
		}
		return matrix;
	}
}
