using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTrajectory : MonoBehaviour {
    public float speed = 4;
    private Vector2 direction;
    private Vector2 endPos;

    public bool moving;

	// Use this for initialization
	void Start () {
        moving = true;
	}
	
	public void SetTrajectoryPoints(Vector2 startPos, Vector2 endPos){
        transform.position = startPos;
        direction = (endPos - startPos).normalized;
        this.endPos = endPos;
    }

	// Update is called once per frame
	void Update () {
        if (moving)
        {
            Vector2 position = new Vector2(transform.position.x, transform.position.y);
            Vector2 inc = direction * speed * Time.deltaTime;
            if ((endPos-position).magnitude <= inc.magnitude) {
                moving = false;
                transform.position = endPos;
                Destroy(gameObject);
            } else {
                transform.position = position + inc;
            }

        }
    }
}
