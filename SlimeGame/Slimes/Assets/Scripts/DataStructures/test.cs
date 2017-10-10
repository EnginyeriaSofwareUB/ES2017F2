using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		List<List<TileType>> matrix;
		 
		Matrix realmatrix;
		//for(
			int y = 9; //y<15;y+=2){			
			matrix = getNewMatrix(y,5,0);
			realmatrix = new Matrix(matrix);
		    realmatrix.print();	
			foreach(TileData tile in realmatrix.coordinateRange(3,2,2)) Debug.Log(tile.ToString());
			
			/*matrix = getNewMatrix(y,7,1);
			realmatrix = new Matrix(matrix);
			realmatrix.print();			
			realmatrix.coordinateRange(0,0,3);*/
		//}
	
		/*
		realmatrix.getNeighbours (-2, 5);
		realmatrix.getNeighbours (0, 3);*/
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public static List<List<TileType>> getNewMatrix(int maxRowLen, int maxCols, int shifted=0){
		List<List<TileType>> matrix = new List<List<TileType>>();
		//matrix = [[0,1,1,1],[0,1,1], [0,1,1,1],[0,1,1],[0,1,1,1],[0,1,1],[0,1,1,1]]
		int middleCols = (int)(maxCols/2);
		int middleRow = (int)(maxRowLen/2);
		for(int i = 0; i<maxCols;i++){
			List<TileType> row = new List<TileType>();
			for(int j = 0; j<maxRowLen-((i+shifted)%2);j++){
				TileType type = TileType.Water;
				if(i == middleCols && j == middleRow){
					type = TileType.Sand;
				}
				row.Add(type);
			}
			matrix.Add(row);
		}
		return matrix;
	}
}
