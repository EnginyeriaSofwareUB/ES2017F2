using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	private GameObject selectedItem;
	private Matrix matrix;
	// Use this for initialization
	void Start () {
		//MapDrawer.InitTest ();
		matrix = new Matrix(MapParser.ReadMap(MapTypes.Small));
		MapDrawer.instantiateMap(matrix.getIterable());
		instantiateSlime ();
		moveSlimeTest();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	private void instantiateSlime(){
		GameObject slime = new GameObject ("Slime");
		slime.AddComponent<SpriteRenderer> ();
		slime.tag = "Slime";
		slime.AddComponent<Slime> ();
		slime.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Test/slime");
		slime.GetComponent<SpriteRenderer> ().sortingOrder = 1;
		slime.AddComponent<BoxCollider2D> ();
		slime.AddComponent<SlimeMovement>();
		
	}
	private void moveSlimeTest(){
		GameObject slime = GameObject.FindGameObjectWithTag("Slime");
		slime.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
		List<Dictionary<TileData,List<TileData>>> listdic = matrix.coordinateRangeAndPath(0,0,4);
		List<Vector2> listvec = new List<Vector2>();
		IEnumerator<TileData> enumerator = listdic[4].Keys.GetEnumerator();
		enumerator.MoveNext();
		enumerator.MoveNext();
		foreach(TileData tile in listdic[4][enumerator.Current]){
			 listvec.Add(MapDrawer.drawInternCoordenates(tile.getPosition()));
		}

		slime.GetComponent<SlimeMovement>().SetBufferAndPlay(listvec);
	}
}
