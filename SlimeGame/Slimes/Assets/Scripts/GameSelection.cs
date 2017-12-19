using System;
using UnityEngine;
using System.Collections.Generic;

public class GameSelection
{
	public static List<Color> playerColors = new List<Color>();
	//public static List<int> playerCores = new List<int> ();
	public static List<SlimeCoreTypes> playerCores = new List<SlimeCoreTypes> ();
	public static List<bool> playerIAs = new List<bool> ();

	//public static Color player1Color=new Color(1,1,1);
	//public static Color player2Color=new Color(1,1,0);

	//public static int player1Core=0;
	//public static int player2Core=1;

	//public static SlimeCoreTypes player1Stats = SlimeCoreTypes.GLUTTONY;
	//public static SlimeCoreTypes player2Stats = SlimeCoreTypes.WRATH;

	public static Matrix map=null;

	public static int modoVictoria;

    public static int MAX_TURNS = 0;
}
