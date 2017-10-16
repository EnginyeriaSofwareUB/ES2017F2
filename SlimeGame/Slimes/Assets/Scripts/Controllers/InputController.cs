using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class InputController : MonoBehaviour {

    GameController controller;

	void Start () {
        controller = Camera.main.GetComponent<GameController>();
    }
	
	void Update () {
        //Boto esquerra del mouse
        if (Input.GetMouseButtonDown(0))
        {
            //Obtinc els colliders que hi ha a la posicio del mouse
            Collider2D[] colliders = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            if (colliders.Length == 1)
            {
                //TODO show info
                EditorUtility.DisplayDialog("Selected object", colliders[0].gameObject.ToString(),"OK");
            }
            else
            {
                foreach (Collider2D col in colliders)
                {
                    if (col.gameObject.CompareTag("Slime"))
                    {
                        //Seleccionar slime
                        Debug.Log(col.gameObject.name);
                        controller.SetSelectedItem(col.gameObject);
                        //TODO show 
                        EditorUtility.DisplayDialog("Selected object", col.gameObject.ToString(), "OK");
                    }
                    else if (col.gameObject.CompareTag("Tile"))
                    {
                        Debug.Log(col.gameObject.name);
                    }
                }
            }
            //Boto dret del mouse
        } else if (Input.GetMouseButtonDown(1))
        {
            //Deseleccionar
            controller.DeselectItem();
        }
	}
}
