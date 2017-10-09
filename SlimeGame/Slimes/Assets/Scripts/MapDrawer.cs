using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDrawer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	private void instantiateMap(System.Collections.IEnumerable map){
		foreach (MapCoordinates tile in map) {
			
		}
	}

	public interface MapCoordinates{
		Vector2 getPosition();

	}

}
