using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ChallengeSelection : MonoBehaviour {

    List<Challenge> challenges;
    // Use this for initialization
    void Start () {
        //Primer crear els reptes:
        int maxChallenges = 5;
        
        string textTutorial = (Resources.Load("challenges") as TextAsset).text;
        challenges = new List<Challenge>();
        JSONNode s = JSON.Parse(textTutorial);
        for (int i = 0; i < s.Count; i++)
        {
            JSONNode text = s[i.ToString()];
            challenges.Add(new Challenge(text["name"], text["description"],text["maxTurns"], text["winCondition"], text["map"]));
        }

        if (challenges.Count < maxChallenges)
        {
            //Desactivar els que estan buids
            for(int i = 0; i < maxChallenges - challenges.Count; i++)
            {
                GameObject.Find("TextC" + (maxChallenges - i).ToString()).SetActive(false);
                GameObject.Find("ButtonC" + (maxChallenges - i).ToString()).SetActive(false);
            }
        }
        if(challenges.Count > maxChallenges)
        {
            Debug.Log("Masses reptes, nomes es veuen " + maxChallenges);
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
            GameSelection.playerColors.Add(new Color(1, 1, 1));
            GameSelection.playerColors.Add(new Color(1, 0, 0));
            GameSelection.playerColors.Add(new Color(0, 0, 1));
            GameSelection.playerColors.Add(new Color(0, 1, 0));

            GameSelection.playerCores.Add(SlimeCoreTypes.WRATH);
            GameSelection.playerCores.Add(SlimeCoreTypes.WRATH);
            GameSelection.playerCores.Add(SlimeCoreTypes.SLOTH);
            GameSelection.playerCores.Add(SlimeCoreTypes.GLUTTONY);

            GameSelection.playerIAs.Add(false);
            GameSelection.playerIAs.Add(true);
            GameSelection.playerIAs.Add(true);
            GameSelection.playerIAs.Add(true);

            GameSelection.modoVictoria = challenges[0].GetModo();
            GameSelection.map = new Matrix(9, 0.1f, Random.Range(0, 10000));
            GameSelection.MAX_TURNS = challenges[0].GetMaxTurns();

            //Debug.Log("primer repte");
        }
        else if (EventSystem.current.currentSelectedGameObject.name.Equals("ButtonC2"))
        {
            GameSelection.playerColors.Add(new Color(0, 0, 1));
            GameSelection.playerColors.Add(new Color(1, 0, 0));

            GameSelection.playerCores.Add(SlimeCoreTypes.WRATH);
            GameSelection.playerCores.Add(SlimeCoreTypes.WRATH);

            GameSelection.playerIAs.Add(false);
            GameSelection.playerIAs.Add(true);

            GameSelection.modoVictoria = challenges[1].GetModo();
            GameSelection.map = challenges[1].GetMap();
            GameSelection.MAX_TURNS = challenges[1].GetMaxTurns();
            //Debug.Log("segon repte");
        }
        else if (EventSystem.current.currentSelectedGameObject.name.Equals("ButtonC3"))
        {
            GameSelection.playerColors.Add(new Color(1, 1, 1));
            GameSelection.playerColors.Add(new Color(1, 0, 0));
            GameSelection.playerColors.Add(new Color(0, 0, 1));
            GameSelection.playerColors.Add(new Color(0, 1, 0));
            GameSelection.playerColors.Add(new Color(0, 1, 1));
            GameSelection.playerColors.Add(new Color(1, 1, 0));

            GameSelection.playerCores.Add(SlimeCoreTypes.WRATH);
            GameSelection.playerCores.Add(SlimeCoreTypes.WRATH);
            GameSelection.playerCores.Add(SlimeCoreTypes.GLUTTONY);
            GameSelection.playerCores.Add(SlimeCoreTypes.SLOTH);
            GameSelection.playerCores.Add(SlimeCoreTypes.WRATH);
            GameSelection.playerCores.Add(SlimeCoreTypes.GLUTTONY);


            GameSelection.playerIAs.Add(false);
            GameSelection.playerIAs.Add(true);
            GameSelection.playerIAs.Add(true);
            GameSelection.playerIAs.Add(true);
            GameSelection.playerIAs.Add(true);
            GameSelection.playerIAs.Add(true);


            GameSelection.modoVictoria = challenges[2].GetModo();
            GameSelection.map = challenges[2].GetMap();
            GameSelection.MAX_TURNS = challenges[2].GetMaxTurns();
            //Debug.Log("tercer repte");
        }
        else if (EventSystem.current.currentSelectedGameObject.name.Equals("ButtonC4"))
        {
            GameSelection.playerColors.Add(new Color(0, 0, 1));
            GameSelection.playerColors.Add(new Color(1, 0, 0));
            GameSelection.playerCores.Add(SlimeCoreTypes.GLUTTONY);
            GameSelection.playerCores.Add(SlimeCoreTypes.WRATH);
            GameSelection.playerIAs.Add(false);
            GameSelection.playerIAs.Add(true);
            GameSelection.modoVictoria = challenges[3].GetModo();
            GameSelection.map = challenges[3].GetMap();
            GameSelection.MAX_TURNS = challenges[3].GetMaxTurns();
            //Debug.Log("quart repte");
        }
        else if (EventSystem.current.currentSelectedGameObject.name.Equals("ButtonC5"))
        {
            GameSelection.playerColors.Add(new Color(0, 0, 1));
            GameSelection.playerColors.Add(new Color(1, 0, 0));
            GameSelection.playerCores.Add(SlimeCoreTypes.GLUTTONY);
            GameSelection.playerCores.Add(SlimeCoreTypes.GLUTTONY);
            GameSelection.playerIAs.Add(false);
            GameSelection.playerIAs.Add(true);
            GameSelection.modoVictoria = challenges[4].GetModo();
            GameSelection.map = challenges[4].GetMap();
            GameSelection.MAX_TURNS = challenges[4].GetMaxTurns();
            //Debug.Log("cinque repte");
        }
        SceneManager.LoadScene("Main");
    }
}

