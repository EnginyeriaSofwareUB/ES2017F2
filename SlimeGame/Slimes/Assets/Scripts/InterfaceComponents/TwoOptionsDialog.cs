using UnityEngine;
using UnityEngine.UI;
using System;

public class TwoOptionsDialog
{
	private GameObject canvas;

	private GameObject panel;
	private GameObject text;
	private GameObject acept;
	private GameObject decline;

	public delegate void OnClickDialog();

	OnClickDialog aceptFunction;
	OnClickDialog declineFunction;

	public TwoOptionsDialog (){

		canvas = GameObject.Find ("Canvas");
		if (canvas == null) {
			Canvas c = new Canvas ();
			canvas = c.gameObject;
		}

		aceptFunction = () => {Debug.Log("Acepted");};
		declineFunction = () => {Debug.Log("Declined");};

		panel = new GameObject ("DialogPanel");
		panel.AddComponent<RectTransform> ();
		panel.AddComponent<CanvasRenderer> ();
		panel.AddComponent<Image> ();
		panel.layer = 5;
		panel.transform.SetParent (canvas.transform);

		acept = new GameObject ("DialogButtonAccept");
		acept.AddComponent<RectTransform> ();
		acept.AddComponent<CanvasRenderer> ();
		acept.AddComponent<Image> ();
		acept.AddComponent<Button> ();
		acept.GetComponent<Button> ().onClick.AddListener (
			() => {
				aceptFunction();
				Hide();
			});
		acept.layer = 5;
		acept.transform.SetParent (panel.transform);

		decline = new GameObject ("DialogButtonDecline");
		decline.AddComponent<RectTransform> ();
		decline.AddComponent<CanvasRenderer> ();
		decline.AddComponent<Image> ();
		decline.AddComponent<Button> ();
		decline.GetComponent<Button> ().onClick.AddListener (
			() => {
				declineFunction();
				Hide();
			});
		decline.layer = 5;
		decline.transform.SetParent (panel.transform);

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
		placeAceptButton ();
		placeDeclineButton ();

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

	private void placeAceptButton(){
		RectTransform t = (RectTransform) acept.transform;
		t.sizeDelta = new Vector2 (50f, 20f);
		t.anchorMin = new Vector2 (0.25f, 0.25f);
		t.anchorMax = new Vector2 (0.25f, 0.25f);
		t.pivot = new Vector2 (0.5f, 0.5f);
		//t.localPosition = new Vector3 (0f,0f,0f);
	}

	private void placeDeclineButton(){
		RectTransform t = (RectTransform) decline.transform;
		t.sizeDelta = new Vector2 (50f, 20f);
		t.anchorMin = new Vector2 (0.75f, 0.25f);
		t.anchorMax = new Vector2 (0.75f, 0.25f);
		t.pivot = new Vector2 (0.5f, 0.5f);
		//t.localPosition = new Vector3 (0f,0f,0f);
	}

	public void SetBackgroundImage(Sprite t){
		panel.GetComponent<Image>().sprite = t;
	}

	public void SetButtonsImage(Sprite t){
		SetAceptButtonImage (t);
		SetDeclineButtonImage (t);
	}

	public void SetAceptButtonImage(Sprite t){
		acept.GetComponent<Image>().sprite = t;
	}

	public void SetAceptButtonColor(Color c){
		acept.GetComponent<Image> ().color = c;
	}

	public void SetDeclineButtonImage(Sprite t){
		decline.GetComponent<Image>().sprite = t;
	}

	public void SetDeclineButtonColor(Color c){
		decline.GetComponent<Image> ().color = c;
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

	public void SetOnClickAceptFunction(OnClickDialog f){
		this.aceptFunction = f;
	}

	public void SetOnClickDeclineFunction(OnClickDialog f){
		this.declineFunction = f;
	}

	public void Hide(){
		panel.SetActive (false);
		text.SetActive (false);
		acept.SetActive (false);
		decline.SetActive (false);
	}

	public void Show(){
		panel.SetActive (true);
		text.SetActive (true);
		acept.SetActive (true);
		decline.SetActive (true);
	}
}
