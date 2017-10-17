using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class InputController : MonoBehaviour {

    GameController controller;
    UIController uiController;

	void Start () {
        controller = Camera.main.GetComponent<GameController>();
        uiController = Camera.main.GetComponent<UIController>();
    }
	
	void Update () {
        //Boto esquerra del mouse
        if (Input.GetMouseButtonDown(0))
        {
            //Obtinc els colliders que hi ha a la posicio del mouse
            Collider2D[] colliders = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            if (colliders.Length == 1)
            {
                //Show info (depenent de si es una casella o un slime)
                if (colliders[0].gameObject.CompareTag("Slime")){
                    Slime slime = (Slime)colliders[0].GetComponent(typeof(Slime));
                    uiController.ShowCanvasInfo(slime.ToString());
                }
                else
                {
                    Tile tile = (Tile)colliders[0].GetComponent(typeof(Tile));
                    uiController.ShowCanvasInfo(tile.ToString());
                }
            }
            else
            {
                foreach (Collider2D col in colliders)
                {
                    if (col.gameObject.CompareTag("Slime"))
                    {
                        //Seleccionar slime
                        controller.SetSelectedItem(col.gameObject);
                        //show info
                        Slime slime = (Slime) col.gameObject.GetComponent(typeof(Slime));
                        uiController.ShowCanvasInfo(slime.ToString());
                    }
                    else if (col.gameObject.CompareTag("Tile"))
                    {
                        //Debug.Log(col.gameObject.name);
                    }
                }
            }
            //Boto dret del mouse
        } else if (Input.GetMouseButtonDown(1))
        {
            //Deseleccionar
            controller.DeselectItem();
            uiController.DisableCanvas();
        }
	}
}
