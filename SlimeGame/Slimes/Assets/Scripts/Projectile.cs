using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    SpriteRenderer rend;

	// Use this for initialization
	void Start () {
        rend.sprite = Resources.Load<Sprite>("Sprites/Proj");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
