using UnityEngine;
using System.Collections.Generic;
using System.Collections;

class SoundsLoader{

    private Hashtable items;

    private static SoundsLoader instance;

    private SoundsLoader(){
        items = new Hashtable();
    }

    public static SoundsLoader GetInstance(){
        if (instance == null) {
            instance = new SoundsLoader();
        }
        return instance;
    }

    public AudioClip GetResource(string path){
        if (items.ContainsKey(path)){
            return (AudioClip)items[path];
        } else {
            AudioClip s = Resources.Load<AudioClip>(path);
            if (s != null){
                items.Add(path, s);
                return s;
            } else {
                return null;
            }
        }
    }
}