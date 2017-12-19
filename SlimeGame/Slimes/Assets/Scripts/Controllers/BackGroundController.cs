using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundController : MonoBehaviour {

	public Material seaMaterial;

	public List<GameObject> sea1;
	public List<GameObject> sea2;

	private int layers = 3;

	public List<Sprite> spriteList;

	// Use this for initialization
	void Start () {
		sea1 = new List<GameObject> ();
		sea2 = new List<GameObject> ();

		spriteList = new List<Sprite> ();
		spriteList.Add (SpritesLoader.GetInstance().GetResource("Background/sea_cyan"));
		spriteList.Add (SpritesLoader.GetInstance().GetResource("Background/sea_deep_blue"));
		spriteList.Add (SpritesLoader.GetInstance().GetResource("Background/sea_light_blue"));

		Vector3 pos = new Vector3 (0f, 5f, 0f);

		for (int i = 0; i < layers; i++) {
			GameObject s1 = new GameObject ("S1 "+i);
			GameObject s2 = new GameObject ("S2 "+i);

			s1.transform.position = pos-i*(new Vector3(0f,1f,0f));
			s2.transform.position = pos-(new Vector3(0f,5f,0f))+i*(new Vector3(0f,1f,0f));

			s1.transform.localScale = new Vector3 (2f*(1+Random.Range(0,5)/5.0f), 1f,1f);
			s2.transform.localScale = new Vector3 (2f*(1+Random.Range(0,5)/5.0f), 1f,1f);

			s1.AddComponent<SpriteRenderer> ().sprite = spriteList[Random.Range(0,spriteList.Count)];
			s2.AddComponent<SpriteRenderer> ().sprite = spriteList[Random.Range(0,spriteList.Count)];

			s1.GetComponent<SpriteRenderer> ().material = seaMaterial;
			s2.GetComponent<SpriteRenderer> ().material = seaMaterial;

			s1.GetComponent<SpriteRenderer> ().sortingOrder = 1000 - (int)(s1.transform.position.y*10);
			s2.GetComponent<SpriteRenderer> ().sortingOrder = 1000 - (int)(s2.transform.position.y*10);

			sea1.Add (s1);
			sea2.Add (s2);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
