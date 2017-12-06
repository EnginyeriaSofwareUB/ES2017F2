using UnityEngine;
using UnityEngine.UI;
using System;

public class TipDialog
{
	protected GameObject canvas;

	protected GameObject panel;
	protected GameObject text;
	protected GameObject button;

	public delegate void OnClickOkDialog();

	protected OnClickOkDialog f;

	public TipDialog (){
		
		canvas = GameObject.Find ("Canvas");
		if (canvas == null) {
			Canvas c = new Canvas ();
			canvas = c.gameObject;
		}
			
		f = () => {Debug.Log("Clicked");};

		panel = new GameObject ("DialogPanel");
		panel.AddComponent<RectTransform> ();
		panel.AddComponent<CanvasRenderer> ();
		panel.AddComponent<Image> ();
		panel.layer = 5;
		panel.transform.SetParent (canvas.transform);

		button = new GameObject ("DialogButton");
		button.AddComponent<RectTransform> ();
		button.AddComponent<CanvasRenderer> ();
		button.AddComponent<Image> ();
		button.AddComponent<Button> ();
		button.GetComponent<Button> ().onClick.AddListener (
			() => {
				Hide();
				f();
			});
		button.layer = 5;
		button.transform.SetParent (panel.transform);
		ResetButtonValues ();



		text = new GameObject ("DialogText");
		text.AddComponent<RectTransform> ();
		text.AddComponent<CanvasRenderer> ();
		text.AddComponent<Text> ();
		text.layer = 5;
		text.transform.SetParent (panel.transform);
		ResetTextValues ();

		placePanel ();
		placeText ();
		placeButton ();

	}

	private void placePanel(){
		RectTransform t = (RectTransform) panel.transform;
		t.sizeDelta = new Vector2 (200f, 120f);
		t.localPosition = new Vector3 (0f,0f,0f);
	}

	private void placeText(){
		RectTransform t = (RectTransform) text.transform;
		t.sizeDelta = new Vector2 (140f, 50f);
		t.anchorMin = new Vector2 (0.5f, 0.65f);
		t.anchorMax = new Vector2 (0.5f, 0.65f);
		t.pivot = new Vector2 (0.5f, 0.5f);
		//t.localPosition = new Vector3 (0f,0f,0f);
	}

	private void placeButton(){
		RectTransform t = (RectTransform) button.transform;
		t.sizeDelta = new Vector2 (60f, 20f);
		t.anchorMin = new Vector2 (0.5f, 0.25f);
		t.anchorMax = new Vector2 (0.5f, 0.25f);
		t.pivot = new Vector2 (0.5f, 0.5f);
		//t.localPosition = new Vector3 (0f,0f,0f);
	}

	public void SetBackgroundImage(Sprite t){
		panel.GetComponent<Image>().sprite = t;
	}

	public void SetButtonImage(Sprite t){
		button.GetComponent<Image>().sprite = t;
	}

	public void SetInfoTextFont(Font f){
		text.GetComponent<Text> ().font = f;
	}

	public void SetInfoTextText(string s){
		text.GetComponent<Text> ().text = s;
	}

	public void ResetTextValues(){
		SetInfoTextFont(Resources.GetBuiltinResource<Font>("Arial.ttf"));
		SetInfoTextText ("Default text");
		text.GetComponent<Text> ().raycastTarget = false;

	}

	public void ResetButtonValues(){
		
	}

	public void SetOnClickFunction(OnClickOkDialog f){
		this.f = f;
	}

	public void Hide(){
		panel.SetActive (false);
		text.SetActive (false);
		button.SetActive (false);
	}

	public void Show(){
		panel.SetActive (true);
		text.SetActive (true);
		button.SetActive (true);
	}
}
