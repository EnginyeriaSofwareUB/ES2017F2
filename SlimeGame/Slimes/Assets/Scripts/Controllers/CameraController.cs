using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public static int xLimit;
    public static int yLimit;
    public int minZoom;
    public int maxZoom;
    public int speed;
	int zoomOut=0;
	int zoomIn=0;
	private bool GlobalCameraReset=false;

	private Vector3? center;
	//private Vector3? beforeMoveCenter;
	private Vector3 originalCenter;
	// Use this for initialization
	void Start () {		
		speed = 20;
        minZoom = 3;
        maxZoom = 13;
		center=null;
		originalCenter = new Vector3(this.transform.position.x,this.transform.position.y,this.transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		if(zoomOut>0){
			Camera cam = this.GetComponent<Camera> ();
			cam.orthographicSize+=Time.deltaTime*10;
			CheckAllConditions(cam);
			zoomOut--;
		}
		if(zoomIn>0){
			Camera cam = this.GetComponent<Camera> ();
			cam.orthographicSize-=Time.deltaTime*10;
			CheckAllConditions(cam);
			zoomIn--;
		}
		if(center.HasValue){
			Vector3 wantToMove =center.Value- this.transform.position;
			Vector3 realMove = wantToMove*Time.deltaTime;
			wantToMove-=realMove;
			this.transform.position+=realMove;
			Rect rect = new Rect(0,0,1f,1f);
			if(rect.Contains(wantToMove)){
				center = null;	
			}
			
		}
	}
	public void CheckAllConditions(Camera cam){
		if(GlobalCameraReset){
			if(!(GetVerticalExtent(cam)<yLimit || GetHorizontalExtent(cam)<xLimit)){
				GlobalCameraReset=false;
				zoomOut=0;
			}
		}
	}
	float GetHorizontalExtent(Camera cam){
		return 0.9f* cam.orthographicSize * cam.aspect;
	}
	float GetVerticalExtent(Camera cam){
		return 0.9f* cam.orthographicSize;
	}
	public static void InitMapSize(Vector2 mapSize){
		xLimit = (int) mapSize.x;
        yLimit = (int) mapSize.y;
	}
	public void ZoomIn(){
		zoomIn=5;
	}
	public void ZoomOut(){
		Camera cam = this.GetComponent<Camera>();
		if((GetVerticalExtent(cam)<yLimit || GetHorizontalExtent(cam)<xLimit)){
			zoomOut=5;
		} 
		
	}
	public void MoveUp(){
		float orto = 1f/this.GetComponent<Camera> ().orthographicSize;
		if (this.transform.position.y < yLimit) {
			CenterCamera(this.transform.position + (new Vector3 (0, 1, 0))*GetModulo());
		}
	}
	public void MoveDown(){
		float orto = 1f/this.GetComponent<Camera> ().orthographicSize;
		if (this.transform.position.y > -yLimit) {
			CenterCamera(this.transform.position - (new Vector3 (0, 1, 0))*GetModulo());
		}
	}
	public void MoveLeft(){
		float orto = 1f/this.GetComponent<Camera> ().orthographicSize;
		if (this.transform.position.x > -xLimit) {
			CenterCamera(this.transform.position-(new Vector3 (1, 0, 0))*GetModulo());
		}
	}
	public void MoveRight(){
		
		if (this.transform.position.x < xLimit) {
			CenterCamera(this.transform.position + (new Vector3 (1, 0, 0))*GetModulo());
		}
	}
	public float GetModulo(){
		float orto = 1f/this.GetComponent<Camera> ().orthographicSize;
		return speed*orto;
	}
	public Rect GetRectWithAllPoints(List<Vector2> vectors, float aspect){
		if(vectors.Count<=0)return new Rect(0,0,0,0);
		float invaspect = 1.0f/aspect;
		Rect rect = new Rect(vectors[0].x,vectors[0].y,aspect,1);
		foreach(Vector2 vect in vectors){
			if(!rect.Contains(vect)){
				float diff;			
				if((diff = vect.x-rect.xMax)>0f){
					rect.xMax = vect.x;
					rect.yMax += diff*invaspect;
				}
				if((diff = rect.xMin-vect.x)>0f){
					rect.xMin = vect.x;
					rect.yMin -= diff*invaspect;
				}
				if((diff = vect.y-rect.yMax)>0f){
					rect.yMax = vect.y;
					rect.xMax += diff*invaspect;
				}
				if((diff = rect.yMin-vect.y)>0f){
					rect.yMin = vect.y;
					rect.xMin -= diff*invaspect;
				}
			}
			
		}		
		return rect;
	}
	public void ChangeCamera(List<Slime> slimes){
		List<Vector2> vects = new List<Vector2>();
		foreach(Slime slime in slimes){
			vects.Add(slime.actualTile.getPosition());
		}
		Rect rect = GetRectWithAllPoints(vects,this.GetComponent<Camera> ().aspect);
		//GUI.Label(rect,"465");
		//this.transform.position+=(new Vector3(rect.center.x,rect.center.y,0)* Time.deltaTime*speed);
	}
	public void GlobalCamera(){
		CenterCamera();
		GlobalCameraReset=true;
		zoomOut=999;
	}
	public void CenterCamera(Vector2 newCenter){
		this.center = new Vector3(newCenter.x,newCenter.y,originalCenter.z);
	}
	public void CenterCamera(){
		this.center = originalCenter;//new Vector3(0,0,-1);
	}
}
