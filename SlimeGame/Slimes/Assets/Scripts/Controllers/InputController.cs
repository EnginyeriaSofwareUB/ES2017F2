using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class InputController : MonoBehaviour
{

    GameController controller;
    UIController uiController;

    void Start()
    {
        controller = Camera.main.GetComponent<GameController>();
        uiController = Camera.main.GetComponent<UIController>();
    }

    void Update()
    {
        //Boto esquerra del mouse
        if (Input.GetMouseButtonDown(0))
        {
            //Obtinc els colliders que hi ha a la posicio del mouse
            Collider2D[] colliders = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            foreach (Collider2D col in colliders)
            {
                if (col.gameObject.CompareTag("Slime"))
                {
                    //Seleccionar slime
                    controller.SetSelectedSlime(col.gameObject);
                    //show info
                    Slime slime = (Slime)col.gameObject.GetComponent(typeof(Slime));
                    uiController.ShowCanvasInfo(slime.ToString());
					return;
                }
                else if (col.gameObject.CompareTag("Tile"))
                {
                    //Debug.Log(col.gameObject.name);
                    Tile tile = col.gameObject.GetComponent<Tile>();
                    //Vector2 position = tile.getPosition();
                    controller.userHitOnTile(tile.data);
                    uiController.ShowCanvasInfo(tile.ToString());
                }
            }

            //Boto dret del mouse
        }
        else if (Input.GetMouseButtonDown(1))
        {
            //Deseleccionar
            controller.DeselectItem();
            uiController.DisableCanvas();
        }
    }
}
