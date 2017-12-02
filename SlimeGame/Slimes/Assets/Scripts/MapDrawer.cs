using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDrawer {


    public static Vector2 size;
	//private static Vector2 verticalOffset;

	private static Tile[,] tiles;
	private static int MAXMAPSIZE = 50; 

	public static void InitTest () {
		testDrawer ();
	}
		
	public static Vector2 instantiateMap(System.Collections.IEnumerable map, int offsetx = 0, int offsety = 0){

		//verticalOffset =  new Vector2 (0f, 3f*sprite.rect.height/(float)(sprite.pixelsPerUnit*4));
		tiles = new Tile[MAXMAPSIZE,MAXMAPSIZE];
        size = new Vector2();

		foreach (TileData tile in map) {			
			int x = (int)tile.getPosition().x;
			int y = (int)tile.getPosition().y;

			SetTileAt(TileFactory.instantiateTile(x,y,offsetx,offsety,tile),x,y);
		}
        return size;

	}

	public interface MapCoordinates{

		Vector2 getPosition();
		TileType getTileType();
	}

	public static Tile GetTileAt(int x,int y){
		int auxX = x, auxY = y;
		if (x < 0) {
			auxX = MAXMAPSIZE + x; 
		}
		if (y < 0) {
			auxY = MAXMAPSIZE + y;
		}
		return tiles [auxX, auxY];
	}

	public static void SetTileAt(Tile t,int x,int y){
		int auxX = x, auxY = y;
		if (x < 0) {
			auxX = MAXMAPSIZE + x; 
		}
		if (y < 0) {
			auxY = MAXMAPSIZE + y;
		}
		tiles [auxX, auxY] = t;
	}

	/***
	Class and methods for testing
	***/
    private static void testDrawer()
    {
        List<TestClass> list = new List<TestClass>();
        for (int i = -5; i < 5; i++)
        {
            for (int j = -5; j < 5; j++)
            {
                list.Add(new TestClass(new Vector2((float)i, (float)j)));
            }
        }
        instantiateMap(list);
    }

    public static Vector2 MapSize()
    {
        return size;
    }

    private class TestClass : MapCoordinates
    {
        private Vector2 position;
        public TestClass(Vector2 pos)
        {
            position = pos;
        }

        public Vector2 getPosition()
        {
            return position;
        }

        public TileType getTileType()
        {
            return TileType.Sand;
        }

    }

}
