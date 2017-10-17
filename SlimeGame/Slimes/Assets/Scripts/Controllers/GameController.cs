using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	private GameObject selectedItem;
	private Matrix matrix;
	private GameObject panelTip,textTip;
	// Use this for initialization
	void Start () {
		//MapDrawer.InitTest ();
		matrix = new Matrix(MapParser.ReadMap(MapTypes.Small));
		MapDrawer.instantiateMap(matrix.getIterable());
		instantiateSlime ();
        selectedItem = new GameObject("Empty"); //Init selected item as Empty
		panelTip = GameObject.Find("PanelTip"); //ja tenim el panell, per si el necessitem activar, i desactivar amb : panelTip.GetComponent<DialogInfo> ().Active (boolean);
		textTip = GameObject.Find ("TextTip"); //ja tenim el textBox, per canviar el text : textTip.GetComponent<Text> ().text = "Text nou";
		panelTip.GetComponent<DialogInfo> ().Active (false);
		textTip.GetComponent<Text> ().text = "Aquí es mostraran els diferents trucs que pot fer el jugador";

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
