using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {

    public bool pauseActive;

    public GameObject pausePanel;

    // Use this for initialization
    void Start () {
		pausePanel = GameObject.Find("PausePanel");
        pauseActive = false;
        showHidePausePanel();
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void showHidePausePanel()
    {
        if (!pauseActive)
        {
            Time.timeScale = 1;
        } else
        {
            Time.timeScale = 0;
        }
        pausePanel.SetActive(pauseActive);
        pauseActive = !pauseActive;
    }
}
