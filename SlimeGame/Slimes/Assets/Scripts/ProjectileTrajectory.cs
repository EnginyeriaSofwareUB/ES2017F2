using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTrajectory : MonoBehaviour {
    public float speed = 8;
    private Vector2 direction;
    private Vector2 endPos;

    public bool moving;
    private float damage;
	private Slime source;
    private Slime target;

	// Use this for initialization
	void Start () {
        moving = true;
        FloatingTextController.Initialize ();
	}
	
	public void SetTrajectorySlimes(Slime shooter, Slime toAttack){
		this.source = shooter;
		this.target = toAttack;
        damage = shooter.getDamage;
        Vector2 startPosition = shooter.GetActualTile().GetTileData().GetRealWorldPosition();
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
		if (target!=null){
			GameController gameController = GameObject.Find ("Main Camera").GetComponent<GameController> ();
			gameController.ApplyDamage(source,target);
			if (!target.isAlive ()) {
				target.GetTileData ().SetSlimeOnTop ((Slime)null);
				target.GetPlayer ().GetSlimes ().Remove (target);
				Destroy(target.gameObject);
				gameController.RemoveSlime(target);

            }
			gameController.OnProjectileAnimationEnded();
        }

    }
}
