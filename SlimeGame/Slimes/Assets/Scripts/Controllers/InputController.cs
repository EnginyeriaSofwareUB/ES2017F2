using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    GameController controller;
	// Use this for initialization
	void Start () {
        controller = Camera.main.GetComponent<GameController>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D[] colliders = Physics2D.OverlapPointAll(Input.mousePosition);    //TODO per algun motiu colliders esta buid

            foreach(Collider2D col in colliders)
            {
                Debug.Log("Bucle");
                if (col.gameObject.CompareTag("Slime"))
                {
                    //Seleccionar slime
                    controller.SetSelectedItem(gameObject);
                    Debug.Log("SLIME!");
                }else if(col.gameObject.CompareTag("Tile"))
                {
                    //Seleccionar casella (?)
                    controller.SetSelectedItem(gameObject);
                    Debug.Log("TILE!");
                }
            }
        } else if (Input.GetMouseButtonDown(1))
        {
            //Deseleccionar
            controller.DeselectItem();
        }
	}
}
