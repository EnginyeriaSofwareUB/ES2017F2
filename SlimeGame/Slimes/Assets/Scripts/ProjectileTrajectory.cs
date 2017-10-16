using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTrajectory : MonoBehaviour {

	public GameObject trajectoryPointPrefeb;
	//private Vector2 startPos, endPos;

	private int numTrajectoryPoints = 20; //A modificar a conveniencia
	private List<Vector2> trajectoryPoints;

	// Use this for initialization
	void Start () {
		trajectoryPoints = new List<Vector2>();
	}
	
	void SetTrajectoryPoints(Vector2 startPos, Vector2 endPos){
		for (int i = 0; i < numTrajectoryPoints; i++) {
			Vector2 pos = new Vector2 ((startPos.x - endPos.x) * i / numTrajectoryPoints, (startPos.y - endPos.y) * i / numTrajectoryPoints);
			trajectoryPoints[i] = pos;
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
