using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDrawer : MonoBehaviour {

	private Vector2 horizontalOffset;
	private Vector2 diagonalOffset;
	private Vector2 verticalOffset;

	// Use this for initialization
	void Start () {
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	public void instantiateMap(System.Collections.IEnumerable map){

		horizontalOffset = new Vector2 (1f, 0f);
		diagonalOffset = new Vector2 (0.5f, 0.5f);
		verticalOffset = new Vector2 (0f, 1f);

		foreach (MapCoordinates tile in map) {
			Vector2 tileWorldPosition = new Vector2 ();
			int x = (int)tile.getPosition().x;
			int y = (int)tile.getPosition().y;
			if (x % 2 == 1) {
				tileWorldPosition += diagonalOffset;	 
			}
			tileWorldPosition += x * verticalOffset;
			tileWorldPosition += y * horizontalOffset;
			GameObject newTile = new GameObject ("("+x+","+y+")");
			newTile.transform.position = new Vector3 (tileWorldPosition.x, tileWorldPosition.y, 0f); 
		}

	}

	public interface MapCoordinates{

		Vector2 getPosition();

	}



	/***
	Class and methods for testing
	***/
	private void testDrawer(){
		List<TestClass> list = new List<TestClass> ();
		for (int i = 0; i < 5; i++) {
			for (int j = 0; j < 5; j++) {
				list.Add (new TestClass (new Vector2((float)i,(float)j)));
			}
		}
	}

	private class TestClass: MapCoordinates{
		private Vector2 position;
		public TestClass(Vector2 pos){
			position = pos;
		}

		public Vector2 getPosition(){
			return position;
		}
	}

}
