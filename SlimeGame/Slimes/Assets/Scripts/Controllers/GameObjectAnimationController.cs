using System;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectAnimationController: MonoBehaviour
{
	private List<Vector3> transformList;
	private List<Vector3> rotateList;
	private List<Vector3> scaleList;

	private List<float> transformOffsets;
	private List<float> rotateOffsets;
	private List<float> scaleOffsets;

	private List<float> transformDurations;
	private List<float> rotateDurations;
	private List<float> scaleDurations;

	private Vector3 currentTransformDt;
	private Vector3 currentRotateDt;
	private Vector3 currentScaleDt;

	private Mode transformMode;
	private Mode rotateMode;
	private Mode scaleMode;

	private Status transformStatus;
	private Status rotateStatus;
	private Status scaleStatus;

	private float currentTransformTime;
	private float currentRotateTime;
	private float currentScaleTime;

	private int currentTransform;
	private int currentRotate;
	private int currentScale;

	private int transformCursor;
	private int rotateCursor;
	private int scaleCursor;

	private float currentTransformDuration;
	private float currentRotateDuration;
	private float currentScaleDuration;

	private Vector3 currentTargetPosition;
	private Vector3 currentTargetRotation;
	private Vector3 currentTargetScale;

	private int reverseDurationTransformOffset;
	private int reverseDurationRotationOffset;
	private int reverseDurationScaleOffset;

	private int reverseOffsetTransformOffset;
	private int reverseOffsetRotationOffset;
	private int reverseOffsetScaleOffset;

	private float transformOffsetDuration;
	private float rotateOffsetDuration;
	private float scaleOffsetDuration;

	private List<TrajectoryFunction> transformFunctionList;
	private List<TrajectoryFunction> rotateFunctionList;
	private List<TrajectoryFunction> scaleFunctionList;

	private List<TimeFunction> timeTransformFunctionList;
	private List<TimeFunction> timeRotateFunctionList;
	private List<TimeFunction> timeScaleFunctionList;

	public delegate Vector3 TrajectoryFunction(float time);
	public delegate float TimeFunction(float time);

	void Start(){
		transformList = new List<Vector3>();
		rotateList = new List<Vector3>();
		scaleList = new List<Vector3>();

		transformOffsets = new List<float>();
		rotateOffsets = new List<float>();
		scaleOffsets = new List<float>();

		transformDurations = new List<float>();
		rotateDurations = new List<float>();
		scaleDurations = new List<float>();

		test ();
		//reversetest();
		scaleTest();
	}

	private void reversetest(){

		transformList.Add (new Vector3(3f, 0f, 0f));
		transformList.Add (new Vector3(5f, 3f, 1f));
		transformList.Add (new Vector3(-2f, 3f, 1f));
		transformList.Add (new Vector3 (0f, 0f, 0f));
		transformDurations.Add (5f);
		transformDurations.Add (1f);
		transformDurations.Add (5f);
		transformOffsets.Add (1f);
		transformOffsets.Add (3f);
		transformOffsets.Add (5f);
		transformOffsets.Add (0f);
		transformStatus = Status.PLAYING;
		transformMode = Mode.ONESHOTREVERSE;
		currentTransformTime = 0f;
		transformCursor = -1;
		currentTransform = transformList.Count - 2;
		reverseDurationTransformOffset = 0;
		reverseOffsetTransformOffset = 0;
		currentTransformDuration = transformDurations [currentTransform];
		currentTargetPosition = transformList [currentTransform];
		currentTransformDt = (currentTargetPosition - transform.position)/currentTransformDuration;
	}

	private void scaleTest(){
		AddScaleTransition (new Vector3 (1f, 1f, 1f), 5f, 1f);
		AddScaleTransition (new Vector3 (3f, 1f, 1f), 1f, 3f);
		AddScaleTransition (new Vector3 (3f, 3f, 1f), 5f, 5f);
		//El último valor de duracion no se utiliza
		AddScaleTransition (new Vector3 (1f, 1f, 1f), 0f, 0f);
		scaleStatus = Status.PLAYING;
		currentScale = 0;
		scaleMode = Mode.BOUNCE;
		currentScaleTime = 0f;
		currentScaleDuration = scaleDurations[currentScale];
		scaleOffsetDuration = scaleOffsets [currentScale];
		scaleCursor = 1;
		currentTargetScale = scaleList[currentScale + scaleCursor];
		currentScaleDt = (currentTargetScale - scaleList[0])/currentScaleDuration;
		reverseDurationScaleOffset = -1;
		reverseOffsetScaleOffset = -1;
		currentScale = 1;
	}


	private void test(){
		AddTransformTransition (new Vector3 (0f, 0f, 0f), 5f, 1f);
		AddTransformTransition (new Vector3 (3f, 0f, 0f), 1f, 3f);
		AddTransformTransition (new Vector3 (5f, 3f, 1f), 5f, 5f);
		AddTransformTransition (new Vector3 (-2f, 3f, 1f), 0f, 0f);
		transformStatus = Status.PLAYING;
		currentTransform = 0;
		transformMode = Mode.BOUNCE;
		currentTransformTime = 0f;
		currentTransformDuration = transformDurations[currentTransform];
		transformOffsetDuration = transformOffsets [currentTransform];
		transformCursor = 1;
		currentTargetPosition = transformList [currentTransform + transformCursor];
		currentTransformDt = (currentTargetPosition - transformList[0])/currentTransformDuration;
		reverseDurationTransformOffset = -1;
		reverseOffsetTransformOffset = -1;
		currentTransform = 1;
	}

	void Update(){
		UpdateTransfom ();
		UpdateRotate ();
		UpdateScale ();
	}

	private void UpdateTransfom(){
		if(transformStatus==Status.PLAYING){
			currentTransformTime += Time.deltaTime;
				Vector3 v = currentTransformDt * Time.deltaTime;
				if (v.magnitude < (transform.position - currentTargetPosition).magnitude) {
					if (0 < currentTransformTime - transformOffsetDuration && currentTransformTime - transformOffsetDuration < currentTransformDuration) {

						transform.position += v;
					}
				} else {
					transform.position = currentTargetPosition;
					currentTransform += transformCursor;
					currentTransformTime = 0f;
					switch(transformMode){
					case Mode.ONESHOT:
						if (currentTransform > transformList.Count-1) {
							transformStatus = Status.ENDED;
						} else {
							transformOffsetDuration = transformOffsets [currentTransform - 1];
							currentTransformDuration = transformDurations [currentTransform-1];
							currentTargetPosition = transformList [currentTransform];
							currentTransformDt = (currentTargetPosition - transform.position)/currentTransformDuration;
						}
						break;
					case Mode.ONESHOTREVERSE:
						if (currentTransform < 0) {
							transformStatus = Status.ENDED;
						} else {
							transformOffsetDuration = transformOffsets [currentTransform + 1];
							currentTransformDuration = transformDurations [currentTransform];
							currentTargetPosition = transformList [currentTransform];
							currentTransformDt = (currentTargetPosition - transform.position)/currentTransformDuration;						}
						break;
					case Mode.BOUNCE:
						if (currentTransform > transformList.Count-1) {
							transformCursor = -1;
							currentTransform = transformList.Count - 2;
							reverseDurationTransformOffset = 0;
							reverseOffsetTransformOffset = 1;
							transformOffsetDuration = transformOffsets [currentTransform+1];
							currentTransformDuration = transformDurations [currentTransform];
							currentTargetPosition = transformList [currentTransform];
							currentTransformDt = (currentTargetPosition - transform.position)/currentTransformDuration;
						} else if(currentTransform < 0) {
							transformCursor = 1;
							currentTransform = 0;
							reverseDurationTransformOffset = -1;
							reverseOffsetTransformOffset = -1;
							transformOffsetDuration = transformOffsets [currentTransform];
							currentTransformDuration = transformDurations [currentTransform];
							currentTargetPosition = transformList [currentTransform + transformCursor];
							currentTransformDt = (currentTargetPosition - transform.position)/currentTransformDuration;
						} else {
						transformOffsetDuration = transformOffsets [currentTransform + reverseOffsetTransformOffset];
							currentTransformDuration = transformDurations [currentTransform + reverseDurationTransformOffset];
							currentTargetPosition = transformList [currentTransform];
							currentTransformDt = (currentTargetPosition - transform.position)/currentTransformDuration;
						}
						break;
					case Mode.TRAJECTORYFUNCTION:
						break;
					default:
						break;
				}
			}
		}
	}

	private void UpdateRotate(){
		if(rotateStatus==Status.PLAYING){
			currentRotateTime += Time.deltaTime;
			Vector3 v = currentRotateDt * Time.deltaTime;
			if (v.magnitude < (transform.position - currentTargetPosition).magnitude) {
				if (0 < currentRotateTime - rotateOffsetDuration && currentRotateTime - rotateOffsetDuration < currentRotateDuration) {

					transform.position += v;
				}
			} else {
				transform.position = currentTargetPosition;
				currentRotate += rotateCursor;
				currentRotateTime = 0f;
				switch(rotateMode){
				case Mode.ONESHOT:
					if (currentRotate > rotateList.Count-1) {
						rotateStatus = Status.ENDED;
					} else {
						rotateOffsetDuration = rotateOffsets [currentRotate - 1];
						currentRotateDuration = rotateDurations [currentRotate-1];
						currentTargetRotation = rotateList [currentRotate];
						currentRotateDt = (currentTargetRotation - transform.position)/currentRotateDuration;
					}
					break;
				case Mode.ONESHOTREVERSE:
					if (currentRotate < 0) {
						rotateStatus = Status.ENDED;
					} else {
						rotateOffsetDuration = rotateOffsets [currentTransform + 1];
						currentRotateDuration = rotateDurations [currentRotate];
						currentTargetRotation = rotateList [currentRotate];
						currentRotateDt = (currentTargetRotation - transform.position)/currentRotateDuration;						}
					break;
				case Mode.BOUNCE:
					if (currentRotate > rotateList.Count-1) {
						rotateCursor = -1;
						currentRotate = rotateList.Count - 2;
						reverseDurationTransformOffset = 0;
						reverseOffsetTransformOffset = 1;
						rotateOffsetDuration = rotateOffsets [currentRotate+1];
						currentRotateDuration = rotateDurations [currentRotate];
						currentTargetRotation = rotateList [currentRotate];
						currentRotateDt = (currentTargetPosition - transform.position)/currentRotateDuration;
					} else if(currentRotate < 0) {
						rotateCursor = 1;
						currentRotate = 0;
						reverseDurationTransformOffset = -1;
						reverseOffsetTransformOffset = -1;
						rotateOffsetDuration = rotateOffsets [currentRotate];
						currentRotateDuration = rotateDurations[currentRotate];
						currentTargetRotation = rotateList [currentRotate + transformCursor];
						currentRotateDt = (currentTargetPosition - transform.position)/currentRotateDuration;
					} else {
						rotateOffsetDuration = rotateOffsets [currentRotate + reverseOffsetTransformOffset];
						currentRotateDuration = rotateDurations [currentRotate + reverseDurationTransformOffset];
						currentTargetRotation = rotateList [currentRotate];
						currentRotateDt = (currentTargetPosition - transform.position)/currentTransformDuration;
					}
					break;
				case Mode.TRAJECTORYFUNCTION:
					break;
				default:
					break;
				}
			}
		}
	}

	private void UpdateScale(){
		if(scaleStatus==Status.PLAYING){
			currentScaleTime += Time.deltaTime;
			Vector3 v = currentScaleDt * Time.deltaTime;
			if (v.magnitude < (transform.localScale - currentTargetScale).magnitude) {
				if (0 < currentScaleTime - scaleOffsetDuration && currentScaleTime - scaleOffsetDuration < currentScaleDuration) {
					transform.localScale += v;
				}
			} else {
				transform.localScale = currentTargetScale;
				currentScale += scaleCursor;
				currentScaleTime = 0f;
				switch(scaleMode){
				case Mode.ONESHOT:
					if (currentScale > scaleList.Count-1) {
						scaleStatus = Status.ENDED;
					} else {
						scaleOffsetDuration = scaleOffsets[currentScale- 1];
						currentScaleDuration = scaleDurations [currentScale-1];
						currentTargetScale = scaleList[currentScale];
						currentScaleDt = (currentTargetScale - transform.localScale)/currentScaleDuration;
					}
					break;
				case Mode.ONESHOTREVERSE:
					if (currentScale < 0) {
						scaleStatus = Status.ENDED;
					} else {
						scaleOffsetDuration = scaleOffsets[currentScale+ 1];
						currentScaleDuration = scaleDurations [currentScale];
						currentTargetScale = scaleList [currentScale];
						currentScaleDt = (currentTargetScale - transform.localScale)/currentScaleDuration;						
					}
					break;
				case Mode.BOUNCE:
					if (currentScale > scaleList.Count-1) {
						scaleCursor = -1;
						currentScale = scaleList.Count - 2;
						reverseDurationScaleOffset = 0;
						reverseOffsetScaleOffset = 1;
						scaleOffsetDuration = scaleOffsets [currentScale+1];
						currentScaleDuration = scaleDurations [currentScale];
						currentTargetScale = scaleList [currentScale];
						currentScaleDt = (currentTargetScale - transform.localScale)/currentScaleDuration;
					} else if(currentScale < 0) {
						scaleCursor = 1;
						currentScale = 0;
						reverseDurationScaleOffset = -1;
						reverseOffsetScaleOffset = -1;
						scaleOffsetDuration = scaleOffsets [currentScale];
						currentScaleDuration = scaleDurations[currentScale];
						currentTargetScale = scaleList [currentScale + scaleCursor];
						currentScaleDt = (currentTargetScale - transform.localScale)/currentScaleDuration;
					} else {
						scaleOffsetDuration = scaleOffsets[currentScale + reverseOffsetScaleOffset];
						currentScaleDuration = scaleDurations [currentScale + reverseDurationScaleOffset];
						currentTargetScale = scaleList [currentScale];
						currentScaleDt = (currentTargetScale - transform.localScale)/currentScaleDuration;
					}
					break;
				case Mode.TRAJECTORYFUNCTION:
					break;
				default:
					break;
				}
			}
		}
	}

	public GameObjectAnimationController (){
		
	}

	public enum Mode{
		ONESHOT,
		ONESHOTREVERSE,
		BOUNCE,
		LOOP,
		TRAJECTORYFUNCTION
	}

	public enum Status{
		WAITING,
		PLAYING,
		ENDED
	}

	public void AddTransformTransition(Vector3 point,float duration,float offset){
		transformList.Add (point);
		transformDurations.Add (duration);
		transformOffsets.Add (offset);
	}

	public void AddRotateTransition(Vector3 point,float duration,float offset){
		rotateList.Add (point);
		rotateDurations.Add (duration);
		rotateOffsets.Add (offset);
	}

	public void AddScaleTransition(Vector3 point,float duration,float offset){
		scaleList.Add (point);
		scaleDurations.Add (duration);
		scaleOffsets.Add (offset);
	}

	public void StartAnimation(){
		if (transformList.Count > 0) {
			transformStatus = Status.PLAYING;
		}
		if (rotateList.Count > 0) {
			transformStatus = Status.PLAYING;
		}
		if (scaleList.Count > 0) {
			transformStatus = Status.PLAYING;
		}
	}

	public void SetTransformMode(Mode mode){
		transformMode = mode;
	}

	public void SetRotationMode(Mode mode){
		rotateMode = mode;
	}

	public void SetScaleMode(Mode mode){
		scaleMode = mode;
	}

}
