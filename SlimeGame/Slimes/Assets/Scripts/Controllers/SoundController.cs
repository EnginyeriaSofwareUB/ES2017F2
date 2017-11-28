using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController {
	public AudioSource efxSource;
	public AudioSource musicSource;
	public static SoundController instance = null;
    string jsonData = PlayerPrefs.GetString("SettingsVolume");

    public SoundController(){
		efxSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
		musicSource = GameObject.Find("EventSystem").GetComponent<AudioSource>();
        SaveSettings loadedData = JsonUtility.FromJson<SaveSettings>(jsonData);
		if (loadedData != null) {
			efxSource.volume = loadedData.effects;
			musicSource.volume = loadedData.music;
		}
	}

	public static SoundController GetInstance(){
		if (instance == null)
			instance = new SoundController ();
		return instance;
	}

	public void PlaySingle(AudioClip clip){
		efxSource.clip = clip;
		efxSource.Play ();
	}

	public void PlayLoop(AudioClip clip){
		musicSource.clip = clip;
		musicSource.loop = true;
		musicSource.Play ();
	}

    public class SaveSettings
    {
        public float effects = 1;
        public float music = 1;
    }

}
