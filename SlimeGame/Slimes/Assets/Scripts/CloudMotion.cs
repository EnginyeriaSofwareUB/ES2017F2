using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMotion : MonoBehaviour {
	float currentTime;
	public float maxTime;
	public Vector3 startPos;
	public Vector3 endPos;
	RectTransform rectTransform;
	float normalizedValue;

	// Use this for initialization
	void Start () {
		currentTime = 0;
		//maxTime = 6;
		//startPos = new Vector3 (-1000, 200, 0);
		//endPos = new Vector3 (1000, 200, 0);
		rectTransform = gameObject.GetComponent<RectTransform> ();
	}
	
	// Update is called once per frame
	void Update () {
		currentTime += Time.deltaTime;
		normalizedValue = currentTime / maxTime;
		rectTransform.anchoredPosition = Vector3.Lerp (startPos, endPos, normalizedValue);
		if (currentTime > maxTime)
			Start ();
	}
}
