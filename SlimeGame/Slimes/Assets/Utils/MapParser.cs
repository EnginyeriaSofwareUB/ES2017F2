﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapParser {

	/*public static int[][] ReadMap(MapTypes mapType) {
		string path = MapTypesCtrl.GetPath(mapType);

		System.IO.StreamReader reader = new System.IO.StreamReader (path);
		string content = reader.ReadToEnd ();
		reader.Close ();

		string[] lines = content.Split ("\n" [0]);
		int rows = lines.Length;

		int[][] map = new int[rows][];

		for(int i = 0; i < rows; i++){
			string[] elements = lines [i].Split(' ');
			map [i] = new int[elements.Length];
			for(int j = 0; j < elements.Length; j++){
				map [i] [j] = int.Parse(elements [j]);
			}
		}

		return map;
	}*/

	public static List<List<TileTypeEnum>> ReadMap(MapTypes mapType) {
		string path = MapTypesCtrl.GetPath(mapType);

		System.IO.StreamReader reader = new System.IO.StreamReader (path);
		string content = reader.ReadToEnd ();
		reader.Close ();

		string[] lines = content.Split ("\n" [0]);
		int rows = lines.Length;

		List<List<TileTypeEnum>> map = new List<List<TileTypeEnum>>();

		for(int i = 0; i < rows; i++){
			string[] elements = lines [i].Split(' ');
			List<TileTypeEnum> row = new List<TileTypeEnum>();
			for(int j = 0; j < elements.Length; j++){
				row.Add((TileTypeEnum)int.Parse(elements [j]));
			}
			map.Add (row);
		}

		return map;
	}
}