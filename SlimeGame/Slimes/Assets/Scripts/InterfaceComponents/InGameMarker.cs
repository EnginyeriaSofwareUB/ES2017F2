using System;
using UnityEngine;

public class InGameMarker
{

	protected GameObject marker;
	private GameObjectAnimationController controller;
	private GameObject markerContainer;
	public delegate void OnClickOkDialog();

	public InGameMarker (){
		markerContainer = new GameObject ("MarkerContainer");
		marker = new GameObject ("Marker");
		marker.transform.SetParent (markerContainer.transform);
		marker.AddComponent<SpriteRenderer> ();
		marker.GetComponent<SpriteRenderer>().sortingLayerName = "Marker";
		controller = marker.AddComponent<GameObjectAnimationController> ();
		controller.initLists ();
		controller.AddTransformTransition (new Vector3(0f, 0f, 0f), 0f, 0f);
		controller.AddTransformTransition (new Vector3(0f, 0f, 0f), 0.5f, 0f);
		controller.AddTransformTransition (new Vector3 (0f, 1.25f, 0f), 0.0f, 0.0f);
		controller.StartAnimation ();

	}

	public void SetParentTransform(Transform parentTransform){
		markerContainer.transform.SetParent (parentTransform);
		markerContainer.transform.localPosition = new Vector3 (0f, 5f, 0f);
	}

	public void SetMarkerRelativeSize(){
		marker.transform.Rotate (0f,0f,77.5f);
		markerContainer.transform.localPosition = new Vector3 (0f, 5f, 0f);
		markerContainer.transform.localScale = new Vector3 (1f, 1f, 1f);
	}

	public void SetSprite(Sprite sp){
		marker.GetComponent<SpriteRenderer> ().sprite = sp;
	}

	public void SetActive(bool active){
		markerContainer.SetActive (active);
	}

}
