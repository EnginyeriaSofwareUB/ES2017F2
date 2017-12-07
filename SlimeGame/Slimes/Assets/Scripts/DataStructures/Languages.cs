using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Languages {

	private static string language;

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
		{"Game over","Final del juego"}
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
		{"Game over","Final del joc"}
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
		{"Game over","Game over"}
	};

	public static void DefineLanguage(string l){
		language = l;
	}
	public static string GetString(string word){
		if (language.Equals("Spanish") || language.Equals("spanish")){
			return GetStringSpanish(word);
		} else if (language.Equals("Catalan") || language.Equals("catalan")){
			return GetStringCatalan(word);
		}else {
			//english predeterminat
			return GetStringEnglish(word);
		}
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
