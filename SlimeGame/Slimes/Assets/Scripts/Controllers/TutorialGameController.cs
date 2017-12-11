using System;
using UnityEngine;
using System.Collections.Generic;

public class TutorialGameController: GameController
{

	ChainTextDialog chainTextDialog;

	void Start(){

		uiController = Camera.main.GetComponent<TutorialUIController>();
		soundController = gameObject.GetComponent<SoundController>();
		camController = Camera.main.GetComponent<CameraController>();

		TileFactory.tileMaterial = tileMaterial;
		matrix = new Matrix (MapParser.ReadMap("Maps/tutorial"));
		MapDrawer.instantiateMap (matrix.getIterable());

		players = new List<Player> ();
		players.Add(new Player("Jugador", 1, StatsFactory.GetStat(SlimeCoreTypes.SLOTH))); // Test with 2 players
		players.Add(new Player("IA Tutorial", 1, StatsFactory.GetStat(SlimeCoreTypes.SLOTH))); // Test with 2 players
		players[0].SetColor(GameSelection.player1Color);
		players[1].SetColor(GameSelection.player2Color);
		SlimeFactory.instantiateSlime(players[0], new Vector2(0f,-2f));
		SlimeFactory.instantiateSlime(players[1], new Vector2(0f,2f));
		status = GameControllerStatus.WAITINGFORACTION;
		uiController.UpdateRound(currentTurn+1);
		uiController.UpdatePlayer(GetCurrentPlayer().GetColor());
		uiController.UpdateActions(playerActions,GetCurrentPlayer().GetActions());
		uiController.ShowBothPanels ();

		InitDialogChains ();

	}

	private void InitDialogChains(){
		
		chainTextDialog = new ChainTextDialog ();
		chainTextDialog.SetButtonImage(SpritesLoader.GetInstance ().GetResource ("Buttons/button_template"));
		chainTextDialog.SetBackgroundImage(SpritesLoader.GetInstance ().GetResource ("Panels/emergent"));
		List<String> texts = new List<String>();
		texts.Add ("Bienvenid@ al tutorial.");
		texts.Add ("Aquí aprenderás las bases de Slimers");
		texts.Add ("¿Estás list@ para empezar?");
		texts.Add ("Este es tu slime.\nHaz click para seleccionarlo.");
		texts.Add ("¿Has visto que ha aparecido alrededor?\nEsas son las casillas por los que tu slime se puede mover en una acción.");
		texts.Add ("Prueba a moverte aquí.");
		texts.Add ("Muy bien.\nAhora vuelve a donde estabas.");
		texts.Add ("Perfecto.");
		texts.Add ("¿Sabias que los slimes pueden hacer muchas mas cosas además de moverse?");
		texts.Add ("Ya verás, si mantienes pulsado el slime verás que se marcan alrededor unas casillas.\nPrueba a arrastrar y soltar encima de esas casillas a ver que pasa.");
		texts.Add ("Wow, se han separado.");
		chainTextDialog.SetTextList(texts);


	}
}