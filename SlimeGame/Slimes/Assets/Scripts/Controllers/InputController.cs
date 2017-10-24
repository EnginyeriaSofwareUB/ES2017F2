using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{

    GameController controller;
    UIController uiController;
    public int xLimit;
    public int yLimit;
    public int minZoom;
    public int maxZoom;

    void Start()
    {
        xLimit = 6;
        yLimit = 6;
        minZoom = 3;
        maxZoom = 13;
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
                    controller.userHitOnTile(tile.GetTileData());
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
        else if (Input.GetAxis("Mouse ScrollWheel") > 0 && this.GetComponent<Camera>().orthographicSize > minZoom)
        {
            this.GetComponent<Camera>().orthographicSize --;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && this.GetComponent<Camera>().orthographicSize < maxZoom)
        {
            this.GetComponent<Camera>().orthographicSize++;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(this.transform.position.y < yLimit)
            {
                this.transform.position += new Vector3(0, 1, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (this.transform.position.y > -yLimit)
            {
                this.transform.position -= new Vector3(0, 1, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (this.transform.position.x > -xLimit)
            {
                this.transform.position -= new Vector3(1, 0, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (this.transform.position.x < xLimit)
            {
                this.transform.position += new Vector3(1, 0, 0);
            }
        }
    }
}
