using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Languages {

	private enum Lang {SPANISH,CATALAN,ENGLISH};
	private static Lang language = Lang.ENGLISH; //per defecte IDIOMA->ENGLISH

	private static Dictionary<string,string> spanish = new Dictionary<string,string>(){
		{"Play","Jugar"},
		{"Tutorial","Tutorial"},
		{"Settings","Ajustes"},
		{"Back","Atrás"},
		{"Effects","Efectos"},
		{"Music","Música"},
		{"Player","Jugador"},
		{"Round","Ronda"},
		{"Actions","Acciones:"},
		{"Text","Texto"},
		{"Tip","Consejo"},
		{"Pause","Pausa"},
		{"Exit","Salir"},
		{"Resume","Continuar"},
		{"Continue","Continuar"},
		{"Game over","FINAL DEL JUEGO"},
		{"Map","Mapa"},
		{"Death","Muerte"},
		{"Mass","Masa"},
		{"Conquest","Conquista"},
		{"Language","Idioma"},
		{"Challenge","Retos"},
		{"ChallengeSelection","Selección del reto"},
		{"GameModeText","Modo de Juego"},
		{"Slimicide","slimicidio"},
		{"MassAttack","Mass-Attack"},
		{"Player1","Jugador #1"},
		{"Player2","Jugador #2"},
		{"Player3","Jugador #3"},
		{"Player4","Jugador #4"},
		{"PlayerText","Jugador #1"}
	};

	private static Dictionary<string,string> catalan = new Dictionary<string,string>(){
		{"Play","Jugar"},
		{"Tutorial","Tutorial"},
		{"Settings","Configuració"},
		{"Back","Endarrere"},
		{"Effects","Efectes"},
		{"Music","Música"},
		{"Player","Jugador"},
		{"Round","Ronda"},
		{"Actions","Accions:"},
		{"Text","Text"},
		{"Tip","Consell"},
		{"Pause","Pausa"},
		{"Exit","Sortir"},
		{"Resume","Continuar"},
		{"Continue","Continuar"},
		{"Game over","FINAL DEL JOC"},
		{"Map","Mapa"},
		{"Death","Mort"},
		{"Mass","Massa"},
		{"Conquest","Conquesta"},
		{"Language","Idioma"},
		{"Challenge","Reptes"},
		{"ChallengeSelection","Selecció del repte"},
		{"GameModeText","Mode de Joc"},
		{"Slimicide","slimicidi"},
		{"MassAttack","Mass-Attack"},
		{"Player1","Jugador #1"},
		{"Player2","Jugador #2"},
		{"Player3","Jugador #3"},
		{"Player4","Jugador #4"},
		{"PlayerText","Jugador #1"}
	};

	private static Dictionary<string,string> english = new Dictionary<string,string>(){
		{"Play","Play"},
		{"Tutorial","Tutorial"},
		{"Settings","Settings"},
		{"Back","Back"},
		{"Effects","Effects"},
		{"Music","Music"},
		{"Player","Player"},
		{"Round","Round"},
		{"Actions","Actions:"},
		{"Text","Text"},
		{"Tip","Tip"},
		{"Pause","Pause"},
		{"Exit","Exit"},
		{"Resume","Resume"},
		{"Continue","Continue"},
		{"Game over","GAME OVER"},
		{"Map","Map"},
		{"Death","Death"},
		{"Mass","Mass"},
		{"Conquest","Conquest"},
		{"Language","Language"},
		{"Challenge","Challenges"},
		{"ChallengeSelection","Challenge Selection"},
		{"GameModeText","Game Mode"},
		{"Slimicide","Slimicide"},
		{"MassAttack","Mass-Attack"},
		{"Player1","Player #1"},
		{"Player2","Player #2"},
		{"Player3","Player #3"},
		{"Player4","Player #4"},
		{"PlayerText","Player #1"}
	};

	public static void DefineLanguage(string l){
		if (l.Equals("Spanish") || l.Equals("spanish")){
			language = Lang.SPANISH;
		}else if (l.Equals("Catalan") || l.Equals("catalan")){
			language = Lang.CATALAN;
		} else{
			language = Lang.ENGLISH;
		}
	}

	public static string GetLanguage(){
		switch (language){
			case Lang.SPANISH:
				return "Spanish";
			case Lang.CATALAN:
				return "Catalan";
			case Lang.ENGLISH:
				return "English";
			default:
				return "English";
		}
	}

	public static string GetString(string word,string actual){
		string ret;
		switch(language){
			case Lang.SPANISH:
				ret = GetStringSpanish(word,actual);
				break;
			case Lang.CATALAN:
				ret = GetStringCatalan(word,actual);
				break;
			case Lang.ENGLISH:
				ret = GetStringEnglish(word,actual);
				break;
			default:
				ret = actual;
				break;
		}
		return ret;
	}

	private static string GetStringSpanish(string word,string actual){
		if (spanish.ContainsKey(word)) return spanish[word];
		else return actual;
	}

	private static string GetStringCatalan(string word,string actual){
		if (catalan.ContainsKey(word)) return catalan[word];
		else return actual;
	}

	private static string GetStringEnglish(string word,string actual){
		if (english.ContainsKey(word)) return english[word];
		else return actual;
	}
}
