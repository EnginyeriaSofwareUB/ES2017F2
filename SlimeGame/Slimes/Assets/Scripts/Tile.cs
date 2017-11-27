using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
	
	private TileData data;
	private SpriteAnimation animation;
	public SpriteRenderer tileUILayer;
	public SpriteRenderer tileElementLayer;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		animation.update ();
	}

	public Vector2 getPosition(){
		return data.getPosition();
	}

    //TODO modify when we have more attributes
    public override string ToString()
    {
		return data.ToString();
        //TODO Em peta perque data me diu que es NULL
        //return data.ToString();
    }

	public TileData GetTileData(){
		return data;
	}
	public void SetTileData(TileData data){
		this.data=data;
	}

	public void startUILayer(Vector3 pos, Vector3 size){
		GameObject gotileUILayer = new GameObject ("TileUILayer");
		gotileUILayer.GetComponent<Transform> ().SetParent (this.transform);
		tileUILayer = gotileUILayer.AddComponent<SpriteRenderer> ();
		tileUILayer.gameObject.transform.position = pos;
		tileUILayer.gameObject.transform.localScale = new Vector2(1f,1f);;
		tileUILayer.sortingLayerName = "TileUI";
		tileUILayer.color = new Color (1f, 1f, 1f, 0.5f);
	}

	public void startElementLayer(Vector3 pos, Vector3 size){
		GameObject gotileElementLayer = new GameObject ("TileElementLayer");
		gotileElementLayer.GetComponent<Transform> ().SetParent (this.transform);
		tileElementLayer = gotileElementLayer.AddComponent<SpriteRenderer> ();
		tileElementLayer.gameObject.transform.position = pos;
		tileElementLayer.gameObject.transform.localScale = new Vector2(1f,1f);
		tileElementLayer.sortingLayerName = "TileElement";
		tileElementLayer.color = new Color (1f, 1f, 1f, 0.5f);
		//tileElementLayer.material = GameObject.Find ("Main Camera").GetComponent<GameController> ().fire;
		animation = new SpriteAnimation (tileElementLayer);
		animation.LoadSprites ("Tiles/Fire/full",6);
		animation.playAnimation ();
	}

	public void SetSlimeOnTop(Slime obj){
		data.SetSlimeOnTop (obj);
	}

	public Slime GetSlimeOnTop(){
		return data.GetSlimeOnTop ();
	}

}
