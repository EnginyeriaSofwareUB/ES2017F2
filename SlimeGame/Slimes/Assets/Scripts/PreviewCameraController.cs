using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewCameraController : MonoBehaviour {

	// Use this for initialization
	Vector2 size;
	bool restart = false;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(restart){
			Camera cam = this.GetComponent<Camera> ();
			cam.orthographicSize=3;
			restart=false;
		}
		if(this.size!=null){
			Camera cam = this.GetComponent<Camera> ();
			float horzExtent = 0.8f* cam.orthographicSize * cam.aspect;
			float vertExtent = 0.8f* cam.orthographicSize;
			while(vertExtent<size.y || horzExtent<size.x){
				cam.orthographicSize++;
				horzExtent = 0.8f*cam.orthographicSize * cam.aspect;
				vertExtent = 0.8f* cam.orthographicSize;
			}
			
		}
	}
	public void SetSize(Vector2 size){
		this.size = size;
		restart = true;
	}
}
