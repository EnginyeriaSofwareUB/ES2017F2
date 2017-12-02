using UnityEngine;
using System;

public class TileFactory{

	private static Vector2 horizontalOffset;
	private static Vector2 diagonalOffset;
	private static Sprite sprite;

	private static bool inited=false;

	public static Tile instantiateTile(int x,int y,int offsetx,int offsety,TileData tile){
		if (!inited) {
			initValues ();
			inited = true;
		}

		Vector2 tileWorldPosition = drawInternCoordenates(tile.getPosition());
		GameObject newTile = new GameObject("Tile (" + x + "," + y + ")");
		newTile.tag = "Tile";                           //Add tag
		newTile.AddComponent<SpriteRenderer>();
		newTile.AddComponent<Tile>();                   //Adding Script
		//newTile.GetComponent<SpriteRenderer> ().sprite = (Sprite) sprites[tile.getTileType()];
		newTile.GetComponent<SpriteRenderer> ().sprite = sprite;
		newTile.GetComponent<SpriteRenderer> ().sortingLayerName = "TileContent";
		//newTile.GetComponent<SpriteRenderer> ().material = GameObject.Find ("Main Camera").GetComponent<GameController> ().tileMaterial;
		newTile.AddComponent<PolygonCollider2D>();      //Adding Collider
		newTile.GetComponent<Tile>().SetTileData(tile);
		Vector3 localScale = new Vector3 (0.5f, 0.5f, 1f);
		newTile.transform.localScale = localScale;
		//rotacion de 60
		tile.SetTile(newTile.GetComponent<Tile>());
		tileWorldPosition.x+=offsetx;
		tileWorldPosition.y+=offsety;
		Vector3 vec = new Vector3 (tileWorldPosition.x, tileWorldPosition.y, 0f);
		if (Mathf.Abs(tileWorldPosition.x) > MapDrawer.size.x) MapDrawer.size.x = Mathf.Abs(tileWorldPosition.x);
		if (Mathf.Abs(tileWorldPosition.y) > MapDrawer.size.y) MapDrawer.size.y = Mathf.Abs(tileWorldPosition.y);

		newTile.transform.position =vec;
		newTile.GetComponent<Tile>().startUILayer (vec,localScale);
		newTile.GetComponent<Tile> ().startConquerLayer (vec,localScale);
		newTile.GetComponent<Tile>().startElementLayer (vec,localScale);
		return newTile.GetComponent<Tile> ();

	}

	private static void initValues(){
		
		sprite = SpritesLoader.GetInstance().GetResource("Test/testTileFlat");
		//Hashtable sprites = new Hashtable ();
		//sprites.Add(TileType.Block, Resources.Load<Sprite>("Test/testTileFlat2"));
		//sprites.Add(TileType.Sand, SpritesLoader.GetInstance().GetResource("Test/testTileFlat3"));
		//sprites.Add(TileType.Water, SpritesLoader.GetInstance().GetResource("Test/testTileFlat"));
		horizontalOffset = new Vector2 (sprite.rect.width/(float)(sprite.pixelsPerUnit*2f), 0f);
		diagonalOffset =  new Vector2 (sprite.rect.width/(float)(sprite.pixelsPerUnit*4f), 3f*sprite.rect.height/(float)(sprite.pixelsPerUnit*8));
	
	}

	private static Vector2 drawInternCoordenates(Vector2 axial){
		
		Vector2 tileWorldPosition = new Vector2 ();
		int x = (int)axial.x;
		int y = (int)axial.y;
		tileWorldPosition += x * diagonalOffset;	 
		tileWorldPosition += y * horizontalOffset;
		tileWorldPosition.y=-tileWorldPosition.y;
		return tileWorldPosition;

	}

}