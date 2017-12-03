using UnityEngine;
using System;

public class SlimeFactory{

	public static Slime instantiateSlime(Player pl,int x,int y){
		GameObject slime = new GameObject("Slime " + (pl.GetNumSlimes() + 1).ToString() + " - " + pl.GetName());
		slime.AddComponent<SpriteRenderer>();
		slime.tag = "Slime";
		GameObject face = new GameObject ("SlimeFace");
		face.AddComponent<SpriteRenderer> ();
		face.transform.SetParent (slime.transform);
		slime.AddComponent<Slime>().face = face;
		slime.GetComponent<SpriteRenderer>().sprite = SpritesLoader.GetInstance().GetResource(pl.statsCoreInfo.picDirection+0);
		slime.GetComponent<SpriteRenderer>().sortingLayerName = "TileElement";
		slime.AddComponent<BoxCollider2D>();
		slime.AddComponent<SlimeMovement>();

		pl.AddSlime(slime.GetComponent<Slime>());

		slime.GetComponent<Slime> ().changeScaleSlime ();
		Tile tile = MapDrawer.GetTileAt(x, y);
		Vector2 tileWorldPosition = tile.GetTileData().GetRealWorldPosition();//MapDrawer.drawInternCoordenates(new Vector2(x0, y0));
		slime.transform.position = new Vector3(tileWorldPosition.x, tileWorldPosition.y, 0f);
		slime.GetComponent<Slime>().SetActualTile(tile);
		slime.GetComponent<Slime>().setPlayer(pl);


		return slime.GetComponent<Slime> ();
	}

	public static Slime instantiateSlime(Player pl,Vector2 v){
		return instantiateSlime (pl, (int)v.x, (int)v.y);
	}
	
}

