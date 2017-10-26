using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

            // Priority to action colliders
            bool actionColliderFired = false;
            foreach (Collider2D col in colliders)
            {
                actionColliderFired = checkActionCollider(col);
                if (actionColliderFired)
                {
                    controller.HideAnyRange();
                    break; // 1 action collider max per click
                }
            }

            // Only 1 action per click
            if (!actionColliderFired)
            {
                foreach (Collider2D col in colliders)
                {
                    if (col.gameObject.CompareTag("Slime"))
                    {
                        //Debug.Log("Collider = Slime");
                        //Seleccionar slime
                        controller.SetSelectedSlime(col.gameObject);
                        return;
                    }
                    else if (col.gameObject.CompareTag("Tile"))
                    {
                        //Debug.Log("Collider = Tile");
                        Tile tile = col.gameObject.GetComponent<Tile>();
                        uiController.ShowCanvasInfo(tile.ToString());
                    }
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

    public bool checkActionCollider(Collider2D col)
    {
        if (col.gameObject.CompareTag("MovementRange"))
        {
            //Debug.Log("Collider = MovementRange");
            Tile tile = col.gameObject.GetComponent<Tile>();
            controller.MoveSlime(tile.GetTileData());
            return true;
        }
        else if (col.gameObject.CompareTag("AttackRange"))
        {
            //Debug.Log("Collider = AttackRange");
            Slime defender = col.gameObject.GetComponent<Slime>();
            controller.AttackSlime(defender);
            return true;
        }
        else if (col.gameObject.CompareTag("DivisionRange"))
        {
            //Debug.Log("Collider = DivisionRange");
            Tile tile = col.gameObject.GetComponent<Tile>();
            controller.DivideSlime(tile);
            return true;
        }
        else if (col.gameObject.CompareTag("FusionRange"))
        {
            //Debug.Log("Collider = FusionRange");
            Tile tile = col.gameObject.GetComponent<Tile>();
            controller.FusionSlime(tile);
            return true;
        }

        return false;
    }
}
