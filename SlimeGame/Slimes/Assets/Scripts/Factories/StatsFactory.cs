using System;
using UnityEngine;
using SimpleJSON;
using System.Collections;

public class StatsFactory
{
	private static Hashtable statsContainer;

	private static bool initedStats = false;

	public static StatsContainer GetStat(ElementType type){
		if (!initedStats) {
			initStats ();
		}
		switch (type) {
		case ElementType.EARTH:
			return (StatsContainer) statsContainer["Earth"];
		case ElementType.FIRE:
			return (StatsContainer) statsContainer["Fire"];
		case ElementType.WATER:
			return (StatsContainer) statsContainer["Water"];
		case ElementType.NONE:
			return (StatsContainer) statsContainer["None"];
		default:
			return null;
		}
	}

	public static StatsContainer GetStat(SlimeCoreTypes type){
		if (!initedStats) {
			initStats ();
		}
		switch (type) {
		case SlimeCoreTypes.GLUTTONY:
			return (StatsContainer) statsContainer["Gluttony"];
		case SlimeCoreTypes.SLOTH:
			return (StatsContainer) statsContainer["Sloth"];
		case SlimeCoreTypes.WRATH:
			return (StatsContainer) statsContainer["Wrath"];
		default:
			return null;
		}
	}

	public static StatsContainer GetTutorialIAStats(){
		if (!initedStats) {
			initStats ();
		}
		return (StatsContainer) statsContainer["TutorialIA"];
	}

	public static StatsContainer GetTutorialPlayerStats(){
		if (!initedStats) {
			initStats ();
		}
		return (StatsContainer) statsContainer["TutorialPlayer"];
	}

	private static void initStats(){
		statsContainer = new Hashtable ();
		string s = Resources.Load<TextAsset> ("stats").text;
		JSONNode data = JSON.Parse (s);
		//Debug.Log (data);
		foreach (JSONNode item in data.Children) {
			statsContainer.Add (item["name"].Value,new StatsContainer(item));
		}
		initedStats = true;
	}
}
