using System;
using UnityEngine;
using System.Collections.Generic;

public class TutorialGameController: GameController
{
	void Start(){

		uiController = Camera.main.GetComponent<UIController>();
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

	}
}