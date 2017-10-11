using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	private GameObject selectedItem;

	// Use this for initialization
	void Start () {
		MapDrawer.InitTest ();
		instantiateSlime ();
        selectedItem = new GameObject("Empty"); //Init selected item as Empty
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
	}

    public GameObject GetSelectedItem()
    {
        return selectedItem;
    }
    public void SetSelectedItem(GameObject gameObject)
    {
        if (selectedItem.name.Equals("Empty"))
            Destroy(selectedItem);
        selectedItem = gameObject;
    }
    public void DeselectItem()
    {
        SetSelectedItem(new GameObject("Empty"));
    }
}
