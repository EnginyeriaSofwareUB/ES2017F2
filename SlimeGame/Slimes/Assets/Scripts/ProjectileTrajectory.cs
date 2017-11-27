﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTrajectory : MonoBehaviour {
    public float speed = 4;
    private Vector2 direction;
    private Vector2 endPos;

    public bool moving;
    private float damage;
    private Slime toAttack;

	// Use this for initialization
	void Start () {
        moving = true;
        FloatingTextController.Initialize ();
	}
	
	public void SetTrajectorySlimes(Slime shooter, Slime toAttack){
        this.toAttack = toAttack;
        damage = shooter.getDamage();
        Vector2 startPosition = shooter.GetComponent<Slime>().GetActualTile().GetTileData().GetRealWorldPosition();
        transform.position = startPosition;
        this.endPos = toAttack.GetActualTile().GetTileData().GetRealWorldPosition();
        direction = (this.endPos - startPosition).normalized;
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
    
    void OnDestroy(){
        //Es crida quan es produeix l'event que Destroy del gameobject
        FloatingTextController.CreateFloatingText ((-damage).ToString(),toAttack.transform);
		toAttack.changeMass (-damage);
        Debug.Log(toAttack.GetMass());
        Debug.Log(!toAttack.isAlive ());
		if (!toAttack.isAlive ()) {
			toAttack.GetTileData ().SetSlimeOnTop (null);
			toAttack.GetPlayer ().GetSlimes ().Remove (toAttack);
            Destroy(toAttack.gameObject);
            GameObject.Find ("Main Camera").GetComponent<GameController> ().RemoveSlime(toAttack);
		}
        GameObject.Find ("Main Camera").GetComponent<GameController> ().OnProjectileAnimationEnded();
    }
}
