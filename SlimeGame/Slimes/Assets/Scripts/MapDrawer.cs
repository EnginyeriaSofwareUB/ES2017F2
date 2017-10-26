using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDrawer
{

    private static Vector2 horizontalOffset;
    private static Vector2 diagonalOffset;
    //private static Vector2 verticalOffset;

    public static void InitTest()
    {
        testDrawer();
    }

    public static void instantiateMap(System.Collections.IEnumerable map)
    {

        Sprite sprite = Resources.Load<Sprite>("Test/testTileFlat");
        Hashtable sprites = new Hashtable();
        sprites.Add(TileType.Block, Resources.Load<Sprite>("Test/testTileFlat2"));
        sprites.Add(TileType.Sand, Resources.Load<Sprite>("Test/testTileFlat3"));
        sprites.Add(TileType.Water, Resources.Load<Sprite>("Test/testTileFlat"));
        horizontalOffset = new Vector2(sprite.rect.width / (float)(sprite.pixelsPerUnit * 2f), 0f);
        diagonalOffset = new Vector2(sprite.rect.width / (float)(sprite.pixelsPerUnit * 4f), 3f * sprite.rect.height / (float)(sprite.pixelsPerUnit * 8));
        //		verticalOffset =  new Vector2 (0f, 3f*sprite.rect.height/(float)(sprite.pixelsPerUnit*4));

        foreach (TileData tile in map)
        {

            Vector2 tileWorldPosition = drawInternCoordenates(tile.getPosition());
            int x = (int)tile.getPosition().x;
            int y = (int)tile.getPosition().y;
            /*
			if (x % 2 == 1 && x!=0) {
				tileWorldPosition += diagonalOffset;	 
			}
			*/

            GameObject newTile = new GameObject("Tile (" + x + "," + y + ")");
            newTile.tag = "Tile";                           //Add tag
            newTile.AddComponent<SpriteRenderer>();
            newTile.AddComponent<Tile>();                   //Adding Script
            newTile.GetComponent<SpriteRenderer>().sprite = (Sprite)sprites[tile.getTileType()];
            newTile.AddComponent<PolygonCollider2D>();      //Adding Collider
            newTile.GetComponent<Tile>().SetTileData(tile);
            newTile.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
            //rotacion de 60
            tile.SetRealWorldPosition(tileWorldPosition);
            Vector3 vec = new Vector3(tileWorldPosition.x, tileWorldPosition.y, 0f);
            newTile.transform.position = vec;
        }

    }
    public static Vector2 drawInternCoordenates(Vector2 axial)
    {
        Vector2 tileWorldPosition = new Vector2();
        int x = (int)axial.x;
        int y = (int)axial.y;
        tileWorldPosition += x * diagonalOffset;
        tileWorldPosition += y * horizontalOffset;
        tileWorldPosition.y = -tileWorldPosition.y;
        return tileWorldPosition;
    }
    public interface MapCoordinates
    {

        Vector2 getPosition();
        TileType getTileType();
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

    public static void ShowMovementRange(Slime slime)
    {
        Sprite sprite = Resources.Load<Sprite>("Test/movementRangeFilter");

        foreach (KeyValuePair<TileData, List<TileData>> tilePair in slime.possibleMovements)
        {
            MarkRanged("MovementRange", tilePair.Key, sprite);
        }
    }

    public static void ShowAttackRange(Slime currentSlime, List<Player> players)
    {
        Sprite attackFilter = Resources.Load<Sprite>("Test/attackRangeFilter");

        Vector2 myPos = currentSlime.GetActualTile().getPosition();
        int attackRange = currentSlime.GetAttackRange();
        foreach (Player pl in players)
        {
            if (pl != currentSlime.GetPlayer())
            {
                foreach (GameObject slGO in pl.GetSlimes())
                {
                    Slime slime = slGO.GetComponent<Slime>();
                    Vector2 slPos = slime.GetActualTile().getPosition();
                    if (Matrix.GetDistance(slPos, myPos) <= attackRange)
                    {
                        MapDrawer.MarkRanged("AttackRange", slime.GetActualTile(), attackFilter);
                    }
                }
            }
        }
    }

    public static void ShowDivisionRange(Slime currentSlime, Matrix map)
    {
        Sprite divisionFilter = Resources.Load<Sprite>("Test/attackRangeFilter");

        // Por cada casilla a distancia 1 que no este bloqueada...
        foreach (TileData tile in map.getNeighbours(currentSlime.GetActualTile()))
        {
            MapDrawer.MarkRanged("DivisionRange", tile, divisionFilter);
        }
    }

    public static void ShowFusionRange(Slime currentSlime, Matrix map)
    {
        Sprite fusionFilter = Resources.Load<Sprite>("Test/attackRangeFilter");

        // Por cada casilla a distancia 1 que no este bloqueada...
        foreach (TileData tile in map.getNeighbours(currentSlime.GetActualTile(), true))
        {
            Slime overSlime = tile.GetSlimeOnTop();
            if (overSlime != null && overSlime.GetPlayer() == currentSlime.GetPlayer())
            {
                MapDrawer.MarkRanged("FusionRange", tile, fusionFilter);
            }
        }
    }

    public static void MarkRanged(string rangedTag, TileData tile, Sprite sprite)
    {
        int x = (int)tile.getPosition().x;
        int y = (int)tile.getPosition().y;

        Vector2 tileWorldPosition = drawInternCoordenates(tile.getPosition());

        GameObject newTile = new GameObject("Tile (" + x + "," + y + ")");
        newTile.tag = rangedTag;                           //Add tag
        newTile.AddComponent<SpriteRenderer>();
        newTile.AddComponent<Tile>();                   //Adding Script

        newTile.GetComponent<SpriteRenderer>().sprite = sprite;
        newTile.GetComponent<SpriteRenderer>().sortingOrder = 1;
        Color tmp = newTile.GetComponent<SpriteRenderer>().color;
        tmp.a = 0.5f;
        newTile.GetComponent<SpriteRenderer>().color = tmp;

        newTile.AddComponent<PolygonCollider2D>();      //Adding Collider
        newTile.GetComponent<Tile>().SetTileData(tile);
        newTile.transform.localScale = new Vector3(0.5f, 0.5f, 1f);

        Vector3 vec = new Vector3(tileWorldPosition.x, tileWorldPosition.y, 0f);
        newTile.transform.position = vec;
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
