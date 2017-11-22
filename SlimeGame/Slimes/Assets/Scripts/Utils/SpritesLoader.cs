using UnityEngine;
using System.Collections.Generic;
using System.Collections;

class SpritesLoader{

	private Hashtable items;

	private static SpritesLoader instance;

	private SpritesLoader(){
		items = new Hashtable ();
	}

	public static SpritesLoader GetInstance(){
		if(instance==null){
			instance = new SpritesLoader();
		}
		return instance;
	}

	public Sprite GetResource(string path){
		if (items.ContainsKey (path)) {
			return (Sprite)items [path];
		} else {
			Sprite s = Resources.Load<Sprite> (path);
			if (s != null) {
				items.Add (path, s);
				return s;
			} else {
				return null;
			}
		}
	}
}