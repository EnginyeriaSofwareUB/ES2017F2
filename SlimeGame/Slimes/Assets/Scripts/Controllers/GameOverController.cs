using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour {

	int numberLosers;
	// Use this for initialization
	void Start () {
		Player winner = GameOverInfo.GetWinner();
		GameObject.Find("Background/Background Winner poster/Player").GetComponent<Text>().text+=" #"+winner.GetName().Substring(winner.GetName().Length-1, 1); 
		GameObject go=GameObject.Find("Background/Background Island/WinnerBody");
		go.GetComponent<Image>().color = winner.GetColor();
		go = GameObject.Find("Background/Background Island/WinnerBody/WinnerFace");
		go.GetComponent<Image>().sprite = SpritesLoader.GetInstance ().GetResource (winner.statsCoreInfo.picDirection);

		List<Player> losers = GameOverInfo.GetLosers();
		//com a molt hi ha 5 losers
		numberLosers = losers.Count;
		for (int i = 0; i<numberLosers;i++){
			go = GameObject.Find("Background/Background Island/Loser"+(i+1));
			go.SetActive(true);
			go.GetComponent<Image>().color = losers[i].GetColor();
		}
	}

	public void CloseGameOverScene(){
		//reiniciar valors de gameoverinfo
		GameOverInfo.Init();
		//desactivem les tombes
		for (int i = 1; i<=numberLosers;i++){
			GameObject.Find("Background/Background Island/Loser"+i).SetActive(false);
		}
		SceneManager.LoadScene(0);
	}
}
