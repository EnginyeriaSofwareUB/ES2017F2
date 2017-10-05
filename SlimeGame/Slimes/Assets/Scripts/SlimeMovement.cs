using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMovement : MonoBehaviour {

	private Vector2 startPos, endPos;
	private List<Vector2> bufferPositions = null;
	private int indexBuffer;

	public float secondsXmovement = 1; //1 segon
	private float startTime;

	void Start(){
		List<Vector2> list = new List<Vector2> ();
		list.Add(new Vector2(4,4));
		list.Add(new Vector2(4,-4));
		list.Add(new Vector2(-4,-4));
		list.Add(new Vector2(-4,4));

		SetBufferAndPlay (list);
	}
	
	// Update is called once per frame

	void Update(){
		if (bufferPositions != null && bufferPositions.Count>0) {
			float i = (Time.time - startTime) / secondsXmovement;
			transform.position = Vector2.Lerp (startPos, endPos, i);
			if (i >= 1) {
				startTime = Time.time;
				indexBuffer++;
				if (indexBuffer >= bufferPositions.Count) {
					bufferPositions = null;
				} else {
					startPos = endPos;
					endPos = bufferPositions [indexBuffer];
				}
			}
		}
	}

	void SetBufferAndPlay(List<Vector2> buffer){
		if (buffer != null && buffer.Count > 0) {
			bufferPositions = buffer;
			startPos = transform.position;
			startTime = Time.time;
			indexBuffer = 0;
			endPos = bufferPositions [indexBuffer];
		}
	}
}
