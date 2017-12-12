using UnityEngine;
using System;

public class SlimeFactory{
    private static int ID = 0;

	public static Slime instantiateSlime(Player pl,int x,int y){
		GameObject slimeGameObjectContainer = new GameObject("Slime " + (pl.GetNumSlimes() + 1).ToString() + " - " + pl.GetName());
		GameObject slime = new GameObject("Core");
		slime.transform.SetParent (slimeGameObjectContainer.transform);
		slime.AddComponent<SpriteRenderer>();
		slime.tag = "Slime";
		GameObject face = new GameObject ("SlimeFace");
		face.AddComponent<SpriteRenderer> ();
		face.transform.SetParent (slime.transform);
		face.GetComponent<SpriteRenderer>().sortingLayerName = "TileElement";
		face.GetComponent<SpriteRenderer> ().sprite = SpritesLoader.GetInstance ().GetResource (pl.statsCoreInfo.picDirection);
		slime.AddComponent<Slime>().face = face;
		slime.GetComponent<SpriteRenderer>().sprite = SpritesLoader.GetInstance().GetResource(StatsFactory.GetStat(ElementType.NONE).picDirection+0);
		slime.GetComponent<SpriteRenderer>().sortingLayerName = "TileElement";
		slime.AddComponent<BoxCollider2D>();
		slime.AddComponent<SlimeMovement>().parent = slimeGameObjectContainer;

		pl.AddSlime(slime.GetComponent<Slime>());

		slime.GetComponent<Slime> ().changeScaleSlime ();
		Tile tile = MapDrawer.GetTileAt(x, y);
		Vector2 tileWorldPosition = tile.GetTileData().GetRealWorldPosition();//MapDrawer.drawInternCoordenates(new Vector2(x0, y0));
		slimeGameObjectContainer.transform.position = new Vector3(tileWorldPosition.x, tileWorldPosition.y, 0f);
		slime.GetComponent<Slime>().SetActualTile(tile);
		slime.GetComponent<Slime>().setPlayer(pl);

		slime.transform.localPosition = new Vector3 (0f, 0.22f, 0f);
		
		slime.GetComponent<Slime>().SetId(ID);
		ID++;

		return slime.GetComponent<Slime> ();
	}

	public static Slime instantiateSlime(Player pl,Vector2 v){
		return instantiateSlime (pl, (int)v.x, (int)v.y);
	}
	
}

