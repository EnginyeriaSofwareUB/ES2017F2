using UnityEngine;
using System;

public class SlimeFactory{

	public static Slime instantiateSlime(Player pl,int x,int y){
		GameObject slime = new GameObject("Slime " + (pl.GetNumSlimes() + 1).ToString() + " - " + pl.GetName());
		slime.AddComponent<SpriteRenderer>();
		slime.tag = "Slime";
		slime.AddComponent<Slime>();
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
		//CONFIGURACIO BARRA VIDA PER SLIME
		//afegim canvas al gameObject per despres posar les imatges de la barra de vida
		GameObject newCanvas = new GameObject("Canvas");
		newCanvas.layer = 8;
		Canvas c = newCanvas.AddComponent<Canvas>();
		c.renderMode = RenderMode.WorldSpace;
		//es el fill del slime 
		newCanvas.transform.SetParent (slime.transform);
		RectTransform rect = newCanvas.GetComponent<RectTransform> ();
		//posicion del canvas, dins hi haura la barra de vida
		rect.localPosition = new Vector3 (0f,1f,0f);
		rect.sizeDelta = new Vector2 (1.5f,0.25f);

		return slime.GetComponent<Slime> ();
	}

	public static Slime instantiateSlime(Player pl,Vector2 v){
		return instantiateSlime (pl, (int)v.x, (int)v.y);
	}
	
}

