using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Languages {

	private enum Lang {SPANISH,CATALAN,ENGLISH};
	private static Lang language = Lang.SPANISH;

	private static Dictionary<string,string> spanish = new Dictionary<string,string>(){
		{"Play","Jugar"},
		{"Tutorial","Tutorial"},
		{"Settings","Ajustes"},
		{"Back","Atras"},
		{"Effects","Efectos"},
		{"Music","Musica"},
		{"Player","Jugador"},
		{"Round","Ronda"},
		{"Actions","Acciones"},
		{"Text","Texto"},
		{"Tip","Consejo"},
		{"Pause","Pausa"},
		{"Exit","Salir"},
		{"Resume","Continuar"},
		{"Continue","Continuar"},
		{"Game over","FINAL DEL JUEGO"}
	};

	private static Dictionary<string,string> catalan = new Dictionary<string,string>(){
		{"Play","Jugar"},
		{"Tutorial","Tutorial"},
		{"Settings","Configuracio"},
		{"Back","Endarrere"},
		{"Effects","Efectos"},
		{"Music","Musica"},
		{"Player","Jugador"},
		{"Round","Ronda"},
		{"Actions","Accions"},
		{"Text","Text"},
		{"Tip","Consell"},
		{"Pause","Pausa"},
		{"Exit","Sortir"},
		{"Resume","Continuar"},
		{"Continue","Continuar"},
		{"Game over","FINAL DEL JOC"}
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
		{"Actions","Actions"},
		{"Text","Text"},
		{"Tip","Tip"},
		{"Pause","Pause"},
		{"Exit","Exit"},
		{"Resume","Resume"},
		{"Continue","Continue"},
		{"Game over","GAME OVER"}
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
	public static string GetString(string word){
		string ret;
		switch(language){
			case Lang.SPANISH:
				ret = GetStringSpanish(word);
				break;
			case Lang.CATALAN:
				ret = GetStringCatalan(word);
				break;
			case Lang.ENGLISH:
				ret = GetStringEnglish(word);
				break;
			default:
				ret = word;
				break;
		}
		return ret;
	}

	private static string GetStringSpanish(string word){
		if (spanish.ContainsKey(word)) return spanish[word];
		else return word;
	}

	private static string GetStringCatalan(string word){
		if (catalan.ContainsKey(word)) return catalan[word];
		else return word;
	}

	private static string GetStringEnglish(string word){
		if (english.ContainsKey(word)) return english[word];
		else return word;
	}
}
