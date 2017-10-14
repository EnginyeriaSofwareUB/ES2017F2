using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDrawer {

	private static Vector2 horizontalOffset;
	private static Vector2 diagonalOffset;
	private static Vector2 verticalOffset;

	public static void InitTest () {
		testDrawer ();
	}
		
	public static void instantiateMap(System.Collections.IEnumerable map){

		Sprite sprite = Resources.Load<Sprite>("Test/testTileFlat");
		horizontalOffset = new Vector2 (sprite.rect.width/(float)(sprite.pixelsPerUnit*2f), 0f);
		diagonalOffset =  new Vector2 (sprite.rect.width/(float)(sprite.pixelsPerUnit*4f), 3f*sprite.rect.height/(float)(sprite.pixelsPerUnit*8));
		verticalOffset =  new Vector2 (0f, 3f*sprite.rect.height/(float)(sprite.pixelsPerUnit*4));

		foreach (MapCoordinates tile in map) {
			
			Vector2 tileWorldPosition = drawInternCoordenates(tile.getPosition());
			int x = (int)tile.getPosition().x;
			int y = (int)tile.getPosition().y;
			/*
			if (x % 2 == 1 && x!=0) {
				tileWorldPosition += diagonalOffset;	 
			}
			*/
			
			GameObject newTile = new GameObject ("Tile ("+x+","+y+")");
			newTile.AddComponent<SpriteRenderer> ();
			newTile.GetComponent<SpriteRenderer> ().sprite = sprite;
			newTile.transform.localScale = new Vector3 (0.5f, 0.5f, 1f);
			//rotacion de 60
			
			Vector3 vec = new Vector3 (tileWorldPosition.x, tileWorldPosition.y, 0f); 
			
			newTile.transform.position =vec;
		}

	}
	public static Vector2 drawInternCoordenates(Vector2 axial){		
		Vector2 tileWorldPosition = new Vector2 ();
		int x = (int)axial.x;
		int y = (int)axial.y;
		tileWorldPosition += x * diagonalOffset;	 
		tileWorldPosition += y * horizontalOffset;
		tileWorldPosition.y=-tileWorldPosition.y;
		return tileWorldPosition;
	}
	public interface MapCoordinates{

		Vector2 getPosition();

	}



	/***
	Class and methods for testing
	***/
	private static void testDrawer(){
		List<TestClass> list = new List<TestClass> ();
		for (int i = -5; i < 5; i++) {
			for (int j = -5; j < 5; j++) {
				list.Add (new TestClass (new Vector2((float)i,(float)j)));
			}
		}
		instantiateMap (list);
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
