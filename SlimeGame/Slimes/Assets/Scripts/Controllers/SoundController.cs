using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour{
	public AudioSource efxSource;
	public AudioSource musicSource;
	public float effects;
	public float music;
	//public static SoundController instance = null;
    //string jsonData;

    public void Start()
    {
        efxSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        musicSource = GameObject.Find("EventSystem").GetComponent<AudioSource>();


    }

	public void updateMusicVolume(float music){
        musicSource = GameObject.Find("EventSystem").GetComponent<AudioSource>();
        /*jsonData = PlayerPrefs.GetString("SettingsVolume");
        SaveSettings loadedData = JsonUtility.FromJson<SaveSettings>(jsonData);
        if (loadedData != null){
            musicSource.volume = loadedData.music;
        }*/
		musicSource.volume = music;
    }


	public void updateEffectsVolume(float effects){
        efxSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        /*jsonData = PlayerPrefs.GetString("SettingsVolume");
        SaveSettings loadedData = JsonUtility.FromJson<SaveSettings>(jsonData);
        if (loadedData != null)
        {
            efxSource.volume = loadedData.effects;
        }*/
		efxSource.volume = effects;
    }

	/*public static SoundController GetInstance(){
		if (instance == null)
			instance = new SoundController ();
		return instance;
	}*/

	public void PlaySingle(AudioClip clip){
        efxSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        efxSource.clip = clip;
        //updateEffectsVolume();
        efxSource.Play ();
	}

	public void PlayLoop(AudioClip clip){
        musicSource = GameObject.Find("EventSystem").GetComponent<AudioSource>();
        musicSource.clip = clip;
		musicSource.loop = true;
        //updateMusicVolume();
        musicSource.Play ();
	}
	/*
    public class SaveSettings
    {
        public float effects = 1;
        public float music = 1;
    }*/

}
