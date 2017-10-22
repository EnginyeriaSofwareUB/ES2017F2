﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Matrix {
	
	private Dictionary<int, Dictionary<int,TileData>> map;
	private int shifted;
	public Matrix(List<List<TileType>> matrix){
		//input 		
		map = new Dictionary<int, Dictionary<int,TileData>> ();
		int middleRow = (int)((matrix.Count) / 2);
		int firstX = -middleRow;
		int middleCell = (int)(matrix [middleRow].Count / 2); 
		int firstY = middleRow-middleCell-(int)((matrix.Count)/4);
		if((matrix.Count)%4 ==1)firstY += 1;
		shifted = 0;
		if (matrix[0].Count<matrix[1].Count){
			shifted = 1;
		}
		for (int i = 0; i < matrix.Count; i++) {
			int hexX = firstX+i;
			map [hexX] = new Dictionary<int,TileData> (); 
			List<TileType> row = matrix[i];
			if (i % 2 == shifted) {
				firstY -= 1;
			}
			for (int j = 0; j < row.Count; j++) {
				
				int hexY = firstY+j;
				if (row[j] != TileType.Null) {
					TileData tile = new TileData(row[j], new Vector2(hexX,hexY));
					map [hexX] [hexY] = tile;
					
				}
			}
		}
	}
	public Matrix(int diagonal){
		map = new Dictionary<int, Dictionary<int,TileData>> ();
		int maxim = (int)(diagonal/2);
		int minim = -maxim;
		for(int x = minim;x<=maxim;x++){
			map [x] = new Dictionary<int,TileData> (); 
			for(int y = minim;y<=maxim;y++){
				if(y*x<=0 || Math.Abs(x)+Math.Abs(y)<=maxim){
					TileData tile = new TileData(TileType.Null,new Vector2(x,y));
					map[x][y]=tile;
					
				}
				
			}
		}
		/*Dictionary<TileType,float> dict = new Dictionary<TileType,float>();
		dict[TileType.Block]=0.3f;
		dict[TileType.Sand]=0.7f;*/
		DistributeTiles();
	}
	
	public List<TileData> GetTotalTiles(){
		List<TileData> i = new List<TileData>();
		foreach(int x in map.Keys){
			i.AddRange(map[x].Values);
		}
		return i;
	}
	/*
	public List<TileData> cube_reachable(TileData startTile){
		List<TileData> tiles = new List<TileData>();
		tiles.Add(startTile)
		return tiles;
	}
	 */
	
	public List<TileData> getIterable(){
		List<TileData> l = new List<TileData>();
		foreach(int x in map.Keys){
			foreach(int y in map[x].Keys){
				l.Add(map[x][y]);
			}
		}
		
		return l;
	}
	public List<TileData> getNeighbours(TileData original, Boolean blockingIncluded=false){
		List<TileData> neighbours = new List<TileData> ();
		List<Vector2> directions = new List<Vector2> {
			new Vector2 (0, -1),new Vector2 (1, -1), new Vector2 (1, 0),new Vector2 (0, 1),new Vector2 (-1,1),new Vector2 (-1, 0)
		};
		int x = (int)original.getPosition().x;
		int y = (int)original.getPosition().y;
		foreach(Vector2 vec in directions){
			
			TileData tile = getTile (x + (int)vec.x, y + (int)vec.y);
			if (tile != null && (blockingIncluded || !tile.isBlocking())) {
				neighbours.Add (tile);
				//Debug.Log("("+(x+(int)vec.x)+','+(y+(int)vec.y)+")");
			}
		}
		return neighbours;

	}
	public Boolean isThereTile(int x,int y){
		Boolean b;
		try{
			TileData tile = map [x][y];
			b=true;
		}catch(Exception){
			b = false;
		}
		return b;
	}
	public TileData getTile(int x, int y){
		TileData tile = null;
		if (isThereTile (x, y)) {
			tile = map [x][y];
		}
		return tile;
	}
	/*
	override public string ToString(){        
		int count = 0;
		Debug.Log("Matrix: ");
		String final = "";
		foreach(int y in map.Keys){
			String s = "";
			if(count%2!=shifted) s+="     ";
			foreach(int x in map[y].Keys){
				s+="("+x+","+y+","+map[y][x].type+")";
			}            
			//Debug.Log(s);
			final+=s+"\n";
			count++;
		}
		return final;
	}
	 */
	public static Vector3 axial_to_cube(Vector2 vec){
		int xC = (int)vec.x;
		int zC = (int)vec.y;
		int yC = -xC-zC;
		return new Vector3(xC,yC,zC);
	}
	public static Vector2 cube_to_axial(Vector3 vec){
		return new Vector2(vec.x,vec.z);
	}
	public static int GetDistance(Vector2 v1, Vector2 v2){
		return cube_distance(axial_to_cube(v1),axial_to_cube(v2));
	}
	public static int cube_distance(Vector3 v1, Vector3 v2){
		return (int)Math.Max(Math.Max(Math.Abs(v1.x-v2.x),Math.Abs(v1.y-v2.y)),Math.Abs(v1.z-v2.z));
	}
	/*public List<TileData> coordinateRange(int x, int y, int range){
		List<TileData> tiles = new List<TileData>();
		for(int i=-range;i<=range;i++){
			for(int j=Math.Max(-range,-i-range);j<=Math.Min(range, -i+range);j++){
				TileData tile = getTile(x+i, y+j);
				if(tile!=null)tiles.Add(tile);
			}
		}
		return tiles;
	}*/
	
	/*
	returns a list in which list[k] is dictionary with all the possible moves of length k until k=range
	 */
	public Dictionary<TileData,List<TileData>> possibleCoordinatesAndPath(int x, int y, int range){
		Dictionary<TileData,List<TileData>> dic = new Dictionary<TileData,List<TileData>>();
		foreach(Dictionary<TileData,List<TileData>> rangeDic in coordinateRangeAndPath(x,y,range)){
			foreach(TileData key in rangeDic.Keys){
				dic[key]=rangeDic[key];
			}
		}
		return dic;
	}
	public List<Dictionary<TileData,List<TileData>>> coordinateRangeAndPath(int x, int y, int range){		
		List<Dictionary<TileData,List<TileData>>> listdic = new List<Dictionary<TileData,List<TileData>>>();
		//Queue<QueueItem> queue = new Queue<QueueItem>();
		TileData startTile = getTile(x,y);
		List<TileData> visited = new List<TileData>();
		if(startTile!=null){
			visited.Add(startTile);
			Dictionary<TileData,List<TileData>> first = new Dictionary<TileData,List<TileData>>();
			first[startTile]=new List<TileData>();
			listdic.Add(first);			
			for(int i=1;i<=range;i++){
				Dictionary<TileData,List<TileData>> imoves = new Dictionary<TileData,List<TileData>>();
				foreach(TileData tile in listdic[i-1].Keys){
					List<TileData> path = listdic[i-1][tile];
					foreach(TileData neighbour in getNeighbours(tile)){
						if(!visited.Contains(neighbour)){
							visited.Add(neighbour);
							imoves[neighbour] = new List<TileData>(path);
							imoves[neighbour].Add(neighbour);
						}
					}
				}
				listdic.Add(imoves);
			}
			listdic.Remove(first);
			return listdic;
		}else return null;		
	}
	/*
	float
	 */
	public void DistributeTiles(Dictionary<TileType,float> probabilitiesDic=null){
		//list must sum up to 1
		List<TileData> alltiles = GetTotalTiles();
		int totalTiles =alltiles.Count;
		if(probabilitiesDic==null){
			probabilitiesDic = new Dictionary<TileType,float>();
		}
		if(probabilitiesDic.Keys.Count==0){
			foreach(TileType type in Enum.GetValues(typeof(TileType))){
				if(type!=TileType.Null){
					probabilitiesDic[type]= 1f/(Enum.GetValues(typeof(TileType)).Length-1);
				}
			}
		}
		
		int maxNumIslands = probabilitiesDic.Keys.Count*(totalTiles/(2*probabilitiesDic.Keys.Count));		
		//int numIslands = 0;
		List<TileData> centers = new List<TileData>();
		Dictionary<TileType,int> typeNumCenters= new Dictionary<TileType,int>();
		foreach(TileType type in probabilitiesDic.Keys){
			typeNumCenters[type]=(int)Math.Round(probabilitiesDic[type]*maxNumIslands);
			
		}		
		System.Random rnd = new System.Random();	
		foreach(TileType type in typeNumCenters.Keys){
			for(int i =0;i<typeNumCenters[type];i++){
				
				TileData tile = alltiles[rnd.Next(alltiles.Count)];
				alltiles.Remove(tile);
				centers.Add(tile);
				tile.SetTileType(type);
			}
		}			
		
		while(alltiles.Count>0){
			
			TileData tile = alltiles[rnd.Next(alltiles.Count)];	
			int minDistance = 9999;		
			TileData nearestCenter = null;
			foreach(TileData center in centers){
				int dist = GetDistance(center.getPosition(),tile.getPosition());
				if(minDistance>dist){
					nearestCenter=center;
					minDistance=dist;
				}
			}
			//centers.Add(tile);
			tile.SetTileType(nearestCenter.getTileType());
			alltiles.Remove(tile);

		} 
	}
}
