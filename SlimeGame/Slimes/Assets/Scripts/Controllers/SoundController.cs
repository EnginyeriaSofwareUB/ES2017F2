using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController {
	public AudioSource efxSource;
	public AudioSource musicSource;
	public static SoundController instance = null; 

	public SoundController(){
		efxSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
		musicSource = GameObject.Find("EventSystem").GetComponent<AudioSource>();
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
}
