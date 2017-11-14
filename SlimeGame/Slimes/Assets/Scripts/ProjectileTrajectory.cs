using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTrajectory : MonoBehaviour {

	public GameObject trajectoryPointPrefeb;
    //private Vector2 startPos, endPos;

    public float secondsXmovement = 1; //1 segon, pero es pot determinar des de unity
    private float startTime;

    private int numTrajectoryPoints = 20; //A modificar a conveniencia
	private List<Vector2> trajectoryPoints = null;
    private int trajectoryCounter = 0;

    public bool moving;

	// Use this for initialization
	void Start () {
		trajectoryPoints = new List<Vector2>();
        moving = false;
	}
	
	public void SetTrajectoryPoints(Vector2 startPos, Vector2 endPos){
		for (int i = 0; i < numTrajectoryPoints; i++) {
			Vector2 pos = new Vector2 ((startPos.x - endPos.x) * i / numTrajectoryPoints, (startPos.y - endPos.y) * i / numTrajectoryPoints);
			trajectoryPoints[i] = pos;
		}
        startTime = Time.time;
        moving = true;
        gameObject.AddComponent<Projectile>();
        gameObject.GetComponent<Projectile>().transform.position = trajectoryPoints[trajectoryCounter];
        trajectoryCounter++;
    }

	// Update is called once per frame
	void Update () {
        if (trajectoryPoints != null && trajectoryPoints.Count > 0)
        {
            float i = (Time.time - startTime) / secondsXmovement;
            gameObject.GetComponent<Projectile>().transform.position = trajectoryPoints[trajectoryCounter];
            trajectoryCounter++;
            if (trajectoryCounter == numTrajectoryPoints)
            {
                DestroyObject(gameObject.GetComponent<Projectile>());
                trajectoryPoints = null;
                moving = false; //acaba el moviment

            }
        }
    }
}
