using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Matrix : ScriptableObject {
	
	private Dictionary<int, Dictionary<int,TileData>> map;
	private int shifted;
	public Matrix(List<List<TileType>> matrix){
		map = new Dictionary<int, Dictionary<int,TileData>> ();
		int middleRow = (int)((matrix.Count) / 2);
		int firstY = -middleRow;
		int middleCell = (int)(matrix [middleRow].Count / 2); 
		int firstX = middleRow-middleCell-(int)((matrix.Count)/4);
		if((matrix.Count)%4 ==1)firstX += 1;
		shifted = 0;
		if (matrix[0].Count<matrix[1].Count){
			shifted = 1;
		}
		for (int y = 0; y < matrix.Count; y++) {
			int hexY = firstY+y;
			map [hexY] = new Dictionary<int,TileData> (); 
			List<TileType> row = matrix[y];
			if (y % 2 == shifted) {
				firstX -= 1;
			}
			for (int x = 0; x < row.Count; x++) {
				
				int hexX = firstX+x;
				if (row[x] != TileType.Null) {
					TileData tile = new TileData(row[x], new Vector2(hexX,hexY));
					map [hexY] [hexX] = tile;
				}

			}
		}
		Debug.Log("La del mig és "+map[0][0].type+" la matriu es: "+matrix.Count+"x"+Math.Max(matrix[0].Count,matrix[1].Count));

	}
	public List<TileData> getNeighbours(int x, int y){
		List<TileData> neighbours = new List<TileData> ();
		List<Vector2> directions = new List<Vector2> {
			new Vector2 (0, -1),new Vector2 (1, -1), new Vector2 (1, 0),new Vector2 (0, 1),new Vector2 (-1,1),new Vector2 (-1, 0)
		};
		foreach(Vector2 vec in directions){
			TileData tile = getTile (x + (int)vec.x, y + (int)vec.y);

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
			TileData tile = map [y][x];
			b=true;
		}catch(Exception ex){
			b = false;
		}
		return b;
	}
	public TileData getTile(int x, int y){
		TileData tile = null;
		if (isThereTile (x, y)) {
			tile = map [y][x];
		}
		return tile;
	}
	public void print(){        
		int count = 0;
		Debug.Log("Matrix: ");
		foreach(int y in map.Keys){
			String s = "";
			if(count%2!=shifted) s+="     ";
			foreach(int x in map[y].Keys){
				s+="("+x+","+y+","+map[y][x].type+")";
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
	public List<TileData> coordinateRange(int x, int y, int range){
		List<TileData> tiles = new List<TileData>();
		for(int i=-range;i<=range;i++){
			for(int j=Math.Max(-range,-i-range);j<=Math.Min(range, -i+range);j++){
				TileData tile = getTile(x+i, y+j);
				if(tile!=null)tiles.Add(tile);
			}
		}
		return tiles;
	}
}
