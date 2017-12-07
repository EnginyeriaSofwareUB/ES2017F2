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

	private int currentTranform;
	private int currentRotate;
	private int currentScale;

	private int tranformCursor;
	private int rotateCursor;
	private int scaleCursor;

	private List<TrajectoryFunction> transformFunctionList;
	private List<TrajectoryFunction> rotateFunctionList;
	private List<TrajectoryFunction> scaleFunctionList;

	private List<TimeFunction> timeTransformFunctionList;
	private List<TimeFunction> timeRotateFunctionList;
	private List<TimeFunction> timeScaleFunctionList;

	public delegate Vector3 TrajectoryFunction(float time);
	public delegate float TimeFunction(float time);

	void Start(){
		
	}

	void Update(){
		UpdateTransfom ();
		UpdateRotate ();
		UpdateScale ();
	}

	private void UpdateTransfom(){
		if(transformStatus==Status.PLAYING){
			switch(transformMode){
			case Mode.ONESHOT:
				break;
			case Mode.ONESHOTREVERSE:
				break;
			case Mode.BOUNCE:
				break;
			case Mode.TRAJECTORYFUNCTION:
				break;
			default:
				break;
			}
		}
	}

	private void UpdateRotate(){
		if(rotateStatus==Status.PLAYING){
			currentRotateTime += Time.deltaTime;
			switch(rotateMode){
			case Mode.ONESHOT:
				if (currentRotateTime < rotateDurations [currentRotate]) {
				
				} else {
				
				}
				break;
			case Mode.ONESHOTREVERSE:
				break;
			case Mode.BOUNCE:
				break;
			case Mode.TRAJECTORYFUNCTION:
				break;
			default:
				break;
			}
		}
	}

	private void UpdateScale(){
		if(scaleStatus==Status.PLAYING){
			switch(scaleMode){
			case Mode.ONESHOT:
				break;
			case Mode.ONESHOTREVERSE:
				break;
			case Mode.BOUNCE:
				break;
			case Mode.TRAJECTORYFUNCTION:
				break;
			default:
				break;
			}
		}
	}

	public GameObjectAnimationController (){
		
	}

	public enum Mode{
		ONESHOT,
		ONESHOTREVERSE,
		BOUNCE,
		TRAJECTORYFUNCTION
	}

	public enum Status{
		WAITING,
		PLAYING,
		ENDED
	}
}
