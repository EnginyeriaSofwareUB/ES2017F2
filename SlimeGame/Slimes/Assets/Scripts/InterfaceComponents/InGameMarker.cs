using System;
using UnityEngine;

public class InGameMarker
{

	protected GameObject marker;
	private SpriteAnimation spanim;

	public delegate void OnClickOkDialog();

	public InGameMarker (){

		marker = new GameObject ("Marker");
		marker.AddComponent<Transform> ();
		marker.AddComponent<SpriteRenderer> ();
		marker.GetComponent<SpriteRenderer>().sortingLayerName = "UILayer";
		
	}

	public void SetParentTransform(Transform parentTransform){
		marker.transform.SetParent (parentTransform);
	}

	public void SetSprite(Sprite sp){
		marker.GetComponent<SpriteRenderer> ().sprite = sp;
	}

	public void Update(){
		if (spanim != null) {
			spanim.update ();
		}
	}
}
