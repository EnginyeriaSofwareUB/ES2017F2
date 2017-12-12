using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ChallengeSelection : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //Primer crear els reptes:
        List<Challenge> challenges;
        string textTutorial = (Resources.Load("challenges") as TextAsset).text;
        challenges = new List<Challenge>();
        JSONNode s = JSON.Parse(textTutorial);
        for (int i = 0; i < s.Count; i++)
        {
            JSONNode text = s[i.ToString()];
            challenges.Add(new Challenge(text["name"], text["description"]));
        }

        if (challenges.Count < 4){
            //Desactivar els que estan buids
            for(int i = 0; i < 4-challenges.Count; i++)
            {
                GameObject.Find("TextC" + (4 - i).ToString()).SetActive(false);
                GameObject.Find("ButtonC" + (4 - i).ToString()).SetActive(false);
            }
        }
        if(challenges.Count > 5)
        {
            Debug.Log("Masses reptes, nomes es veuen 5");
        }
        //Activem els texts i els botons com ens sembla
        for(int i =0; i < challenges.Count; i++)
        {
            GameObject.Find("TextC" + (i + 1).ToString()).GetComponent<Text>().text = challenges[i].ToString();
            GameObject.Find("ButtonC" + (i + 1).ToString()).GetComponent<Button>().onClick.AddListener(TaskOnClick);
        }
    }

    private void TaskOnClick()
    {
        //Aqui canvies lo que es necessiti segons quin repte seleccionis
        if (EventSystem.current.currentSelectedGameObject.name.Equals("ButtonC1"))
        {
            //Debug.Log("primer repte");
        }
        else if (EventSystem.current.currentSelectedGameObject.name.Equals("ButtonC2"))
        {
            //Debug.Log("segon repte");
        }
        else if (EventSystem.current.currentSelectedGameObject.name.Equals("ButtonC3"))
        {
            //Debug.Log("tercer repte");
        }
        else if (EventSystem.current.currentSelectedGameObject.name.Equals("ButtonC4"))
        {
            //Debug.Log("quart repte");
        }
        else if (EventSystem.current.currentSelectedGameObject.name.Equals("ButtonC5"))
        {
            //Debug.Log("cinque repte");
        }
        //SceneManager.LoadScene("Main");
    }
}

