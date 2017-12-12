using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public static int xLimit;
    public static int yLimit;
    public int howfar;
	float newZoom;
	float beforeZoom;
	float MaxZoom;
	float MinZoom;
	private Vector3? center;
	private Vector3? beforeMovePosition;
	private Vector3 originalCenter;

	private GameController controller;
	// Use this for initialization
	float speed;
	float speedZoom;
	private bool calibrateZoom = false;
	void Start () {	
		speed=10;
		speedZoom=10;	
		howfar = 1;
		newZoom=-1;
		beforeZoom=-1;
		MaxZoom=-1;
		MinZoom=3;
		center=null;
		controller = this.GetComponent<GameController>();
		originalCenter = new Vector3(this.transform.position.x,this.transform.position.y,this.transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		if(calibrateZoom && MaxZoom<0){
			Camera cam = this.GetComponent<Camera> ();
			if((GetVerticalExtent(cam)<yLimit || GetHorizontalExtent(cam)<xLimit)){
				cam.orthographicSize+=Time.deltaTime*10;
			}else{
				calibrateZoom=false;
				MaxZoom=cam.orthographicSize;
			}
		}
		else if(newZoom>0){
			Camera cam = this.GetComponent<Camera> ();
			if(Mathf.Abs(newZoom-cam.orthographicSize)<=0.2){
				newZoom=-1;
				beforeZoom=-1;
			}else{
				int zoomin = -1;
				if(newZoom-beforeZoom>0)zoomin = 1;
				float ortosizenew = cam.orthographicSize+zoomin*Time.deltaTime*GetSpeedZoom();
				if(ortosizenew<=MaxZoom &&ortosizenew>=MinZoom){
					cam.orthographicSize=ortosizenew;
				}else{
					newZoom=-1;
					beforeZoom=-1;
				}
			}			
		}		
		if(center.HasValue){
			Vector3 mogutDeMoment = this.beforeMovePosition.Value-this.transform.position;
			Vector3 totalPerMoure = this.beforeMovePosition.Value-this.center.Value;
			float dist =Vector3.Magnitude(totalPerMoure)-Vector3.Magnitude(mogutDeMoment);
			if(dist<0.1f){
				center = null;	
				beforeMovePosition=null;
			}else{
				Vector3 realMove = GetVectorSpeed()*Time.deltaTime;
				this.transform.position-=realMove;
			}
		}
	}
	float GetSpeedZoom(){
		float speed = speedZoom;
		if(this.beforeMovePosition.HasValue && this.center.HasValue){
			float x1 = Vector3.Magnitude(this.beforeMovePosition.Value-this.center.Value);
			float x2 = Mathf.Abs(newZoom-beforeZoom);
			speed = (Vector3.Magnitude(GetVectorSpeed())*x2)/x1;			
		}
		return speed;
	}
	Vector3 GetVectorSpeed(){
		Camera cam = this.GetComponent<Camera>();
		return (this.beforeMovePosition.Value-this.center.Value).normalized*speed;
	}
	float GetHorizontalExtent(Camera cam){
		return 0.7f* cam.orthographicSize * cam.aspect;
	}
	float GetVerticalExtent(Camera cam){
		return 0.7f* cam.orthographicSize;
	}
	public static void InitMapSize(Vector2 mapSize){
		xLimit = (int) mapSize.x;
        yLimit = (int) mapSize.y;
	}
	public void ZoomIn(){
		//zoom=5;
		Camera cam = this.GetComponent<Camera>();
		ChangeZoom(cam.orthographicSize-1);
	}
	public void ZoomOut(){
		Camera cam = this.GetComponent<Camera>();
		ChangeZoom(cam.orthographicSize+1);
		
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
		return howfar/**orto*/;
	}
	
	
	public void ChangeZoom(float newZoom){
		if(newZoom<=MaxZoom&& newZoom>=MinZoom){
			this.newZoom=newZoom;
			this.beforeZoom=this.GetComponent<Camera>().orthographicSize;
		}		
	}
	public void InitMaxZoom(){
		calibrateZoom=true;

	}
	public void GlobalCamera(){
		CenterCamera();
		Camera cam = this.GetComponent<Camera>();
		ChangeZoom(MaxZoom);
	}
	public void CenterCamera(Vector2 newCenter){
		this.center = new Vector3(newCenter.x,newCenter.y,originalCenter.z);
		this.beforeMovePosition = this.transform.position;
		
	}
	public void CenterCamera(){
		CenterCamera(originalCenter);//new Vector3(0,0,-1);
	}

	public void AllTilesInCamera(Tile center, List<Tile> tiles){
		Vector2 centerPos = center.transform.position;
		Vector2 size = new Vector2();
		foreach(Tile tile in tiles){
			Vector3 tileWorldPosition = tile.transform.position-center.transform.position;
			if (Mathf.Abs(tileWorldPosition.x) > size.x)size.x = Mathf.Abs(tileWorldPosition.x);
            if (Mathf.Abs(tileWorldPosition.y) > size.y)size.y =Mathf.Abs(tileWorldPosition.y);
			
		}		
		CenterCamera(new Vector2(centerPos.x,centerPos.y));
		ChangeZoom(Mathf.Max(MaxZoom*size.y/(yLimit),MaxZoom*size.x/(xLimit)));
	}
	
}
