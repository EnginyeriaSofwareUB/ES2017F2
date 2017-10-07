using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Matrix : ScriptableObject {
	// senar x senar
	//x+y+z=0
	/*
	 * 
	 * 
	0 0 1 1 1 1 0 0          
	 0 1 1 1 1 1 0           
	0 1 1 1 1 1 1 0          
	 1 1 1 1 1 1 1      =    
	0 1 1 1 1 1 1 0          
	 0 1 1 1 1 1 0           
	0 0 1 1 1 1 0 0          
	     (0,-3) (1,-3) (2,-3) (3,-3)
      (--,--) (0,-2) (1,-2) (2,-2) (--,--)
    (--,--) (-1,-1) (0,-1) (2,-3) (3,-3) (0,-3) (--,--)
	
   */
	/*
	0 0 1 1 1 1 1 1 0 0 [0,0,1,1,1,1,1,1,0,0]
	 0 1 1 1 1 1 1 1 0  [0,1,1,1,1,1,1,1,0] 
	0 1 1 1 1 1 1 1 1 0 [0,1,1,1,1,1,1,1,1,0]
	 1 1 1 1 1 1 1 1 1  [1,1,1,1,1,1,1,1,1]
	0 1 1 1 1 1 1 1 1 0  [0,1,1,1,1,1,1,1,1,0]
	 0 1 1 1 1 1 1 1 0   [0,1,1,1,1,1,1,1,0],
	0 0 1 1 1 1 1 1 0 0

	*/
	//public enum TileType {Null, Sand, Water}
	//public createNewNode(int x, int y)

	private Dictionary<int, Dictionary<int,Tile>> map;
	private int shifted;
	public Matrix(List<List<TileTypeEnum>> matrix){
		map = new Dictionary<int, Dictionary<int,Tile>> ();
		int middleRow = (int)((matrix.Count) / 2);
		int firstY = -middleRow;
		int middleCell = (int)(matrix [middleRow].Count / 2); 
		int firstX = middleRow-middleCell-1;
		shifted = 0;
		if (matrix[0].Count<matrix[1].Count){
			shifted = 1;
			firstX = 0;
		}
		for (int y = 0; y < matrix.Count; y++) {
			int hexY = firstY+y;
			map [hexY] = new Dictionary<int,Tile> (); 
			List<TileTypeEnum> row = matrix[y];
			if (y % 2 == shifted) {
				firstX -= 1;
			}
			for (int x = 0; x < row.Count; x++) {
				
				int hexX = firstX+x;
				if (row[x] != TileTypeEnum.Null) {
					Tile tile = new Tile(row[x]);
					map [hexY] [hexX] = tile;
				}

			}
		}

	}
	public List<Tile> getNeighbours(int x, int y){
		List<Tile> neighbours = new List<Tile> ();
		List<Vector2> directions = new List<Vector2> {
			new Vector2 (0, -1),new Vector2 (1, -1), new Vector2 (1, 0),new Vector2 (0, 1),new Vector2 (-1,1),new Vector2 (-1, 0)
		};
		foreach(Vector2 vec in directions){
			Tile tile = getTile (x + (int)vec.x, y + (int)vec.y);

			if (tile != null) {
				neighbours.Add (tile);
				Debug.Log("("+(x+(int)vec.x)+','+(y+(int)vec.y)+")");
			}
		}
		return neighbours;

	}
	public Boolean isThereTile(int x,int y){
		Boolean b;
		try{
			Tile tile = map [y][x];
			b=true;
		}catch(Exception ex){
			b = false;
		}
		return b;
	}
	public Tile getTile(int x, int y){
		Tile tile = null;
		if (isThereTile (x, y)) {
			tile = map [y][x];
		}
		return tile;
	}
	public void print(){        
		int count = 0;
		foreach(int y in map.Keys){
			String s = "";
			if(count%2!=shifted) s+="     ";
			foreach(int x in map[y].Keys){
				s+="("+x+","+y+")";
			}            
			Debug.Log(s);
			count++;
		}
	}
	public static Vector3 axial_to_cube(Vector2 vec){
		int xC = (int)vec.x;
		int zC = (int)vec.y;
		int yC = -xC-zC;
		return new Vector3(xC,yC,zC);
	}
	public static Vector2 cube_to_axial(Vector3 vec){
		return new Vector2(vec.x,vec.z);
	}
	public static int distance(Vector2 v1, Vector2 v2){
		return cube_distance(axial_to_cube(v1),axial_to_cube(v2));
	}
	public static int cube_distance(Vector3 v1, Vector3 v2){
		return (int)Math.Max(Math.Max(Math.Abs(v1.x-v2.x),Math.Abs(v1.y-v2.y)),Math.Abs(v1.z-v2.z));
	}
}
