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

	private bool changeTurnMove = false;
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
				if(changeTurnMove)changeTurnMove=false;
			}else{
				int zoomin = -1;
				if(newZoom-beforeZoom>0)zoomin = 1;
				float ortosizenew = cam.orthographicSize+zoomin*Time.deltaTime*GetSpeedZoom();
				if(ortosizenew<=MaxZoom &&ortosizenew>=MinZoom){
					cam.orthographicSize=ortosizenew;
				}else{
					if(changeTurnMove)changeTurnMove=false;
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
				if(changeTurnMove)changeTurnMove=false;
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

	public void Move(Vector2 newPosition){
		
		if (this.transform.position.x < xLimit) {
		}
	}

	public float GetModulo(){
		float orto = 1f/this.GetComponent<Camera> ().orthographicSize;
		return howfar/**orto*/;
	}
	
	
	public void ChangeZoom(float newZoom){
		if(newZoom>=MaxZoom) newZoom=MaxZoom;
		if(newZoom<=MinZoom) newZoom=MinZoom;
		this.newZoom=newZoom;
		this.beforeZoom=this.GetComponent<Camera>().orthographicSize;
		
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

	public void AllTilesInCamera(List<Tile> tiles){
		changeTurnMove=true;
		newZoom=-1;
		center=null;
		Vector2 first = tiles[0].transform.position;
		Rect rect = new Rect(first.x, first.y,0,0);
		foreach(Tile tile in tiles){
			Vector3 tileWorldPosition = tile.transform.position;
			Vector2 point = new Vector2(tileWorldPosition.x,tileWorldPosition.y);
			if(!rect.Contains(point)){
				if (point.x > rect.xMax)rect.xMax=point.x;
				if (point.y > rect.yMax) rect.yMax=point.y;
				if (point.x < rect.xMin)rect.xMin=point.x;
				if (point.y < rect.yMin) rect.yMin=point.y;
				
			};
		}
		//Vector2 centerPos = (max+min)/2.0f;
		Vector2 centerPos = rect.center; 
		CenterCamera(new Vector2(centerPos.x,centerPos.y));
		ChangeZoom(Mathf.Max(MaxZoom*(rect.height)/(2*yLimit),MaxZoom*Mathf.Abs(rect.width)/(2*xLimit)));
	}
	public bool IsCameraMoving(){
		return changeTurnMove; 
	}
}
