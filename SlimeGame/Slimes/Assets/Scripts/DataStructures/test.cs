using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		List<List<TileTypeEnum>> matrix;
		 
		Matrix realmatrix;
		for(int y = 3; y<15;y+=2){			
			matrix = getNewMatrix(y,5,0);
			realmatrix = new Matrix(matrix);
			realmatrix.print();	
			matrix = getNewMatrix(y,7,1);
			realmatrix = new Matrix(matrix);
			realmatrix.print();			
		}
	
		/*
		realmatrix.getNeighbours (-2, 5);
		realmatrix.getNeighbours (0, 3);*/
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public static List<List<TileTypeEnum>> getNewMatrix(int maxRowLen, int maxCols, int shifted=0){
		List<List<TileTypeEnum>> matrix = new List<List<TileTypeEnum>>();
		//matrix = [[0,1,1,1],[0,1,1], [0,1,1,1],[0,1,1],[0,1,1,1],[0,1,1],[0,1,1,1]]
		int middleCols = (int)(maxCols/2);
		int middleRow = (int)(maxRowLen/2);
		for(int i = 0; i<maxCols;i++){
			List<TileTypeEnum> row = new List<TileTypeEnum>();
			for(int j = 0; j<maxRowLen-((i+shifted)%2);j++){
				TileTypeEnum type = TileTypeEnum.Water;
				if(i == middleCols && j == middleRow){
					type = TileTypeEnum.Sand;
				}
				row.Add(type);
			}
			matrix.Add(row);
		}
		return matrix;
	}
}
