using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour {
	public Animator animator;
	private Text damageText;
	private Vector3 position;

	void OnEnable(){
		AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo (0);
		Destroy (gameObject, clipInfo [0].clip.length);
		damageText = animator.GetComponent<Text> ();
	}

	void Update(){
		this.transform.position = Camera.main.WorldToScreenPoint (position);;
	}

	public void SetText(string text){
		damageText.text = text;
	}

	public void SetPosition(Vector3 p){
		position = p;
	}
}
