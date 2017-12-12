using System;
using UnityEngine;
using System.Collections.Generic;

public class TutorialGameController: GameController
{

	ChainTextDialog chainTextDialog;
	public InGameMarker marker;
	private TutorialFSMStatus tutorialStatus;
	protected InputController inputController;
	void Start(){

		inputController = Camera.main.GetComponent<TutorialInputController>();
		uiController = Camera.main.GetComponent<TutorialUIController>();
		soundController = gameObject.GetComponent<SoundController>();
		camController = Camera.main.GetComponent<CameraController>();

		TileFactory.tileMaterial = tileMaterial;
		matrix = new Matrix (MapParser.ReadMap("Maps/tutorial"));
		MapDrawer.instantiateMap (matrix.getIterable());

		players = new List<Player> ();
		players.Add(new Player("Jugador", 1, StatsFactory.GetStat(SlimeCoreTypes.SLOTH))); // Test with 2 players
		//players.Add(new Player("IA Tutorial", 1, StatsFactory.GetStat(SlimeCoreTypes.SLOTH))); // Test with 2 players
		players[0].SetColor(new Color(1,1,1));
		//players[1].SetColor(new Color(1,0,1));
		Slime playerSlime = SlimeFactory.instantiateSlime(players[0], new Vector2(0f,-2f));
		//SlimeFactory.instantiateSlime(players[1], new Vector2(0f,2f));
		status = GameControllerStatus.WAITINGFORACTION;
		uiController.UpdateRound(currentTurn+1);
		uiController.UpdatePlayer(GetCurrentPlayer().GetColor());
		uiController.UpdateActions(playerActions,GetCurrentPlayer().GetActions());
		uiController.ShowBothPanels ();

		InitDialogChains ();
		InitMarker ();
		marker.SetParentTransform (playerSlime.transform);
		marker.SetMarkerRelativeSize ();
		marker.SetActive (false);
	}

	private void InitDialogChains(){
		
		chainTextDialog = new ChainTextDialog ();
		chainTextDialog.SetPanelSize(new Vector2(500f, 350f));
		chainTextDialog.SetTextSize(new Vector2(350f, 200f));
		chainTextDialog.SetPanelAnchors (new Vector2 (0.75f, 0.5f), new Vector2 (0.75f, 0.5f));
		chainTextDialog.SetButtonImage(SpritesLoader.GetInstance ().GetResource ("Buttons/button_template"));
		chainTextDialog.SetBackgroundImage(SpritesLoader.GetInstance ().GetResource ("Panels/emergent"));
		List<String> texts = new List<String>();
		texts.Add ("Bienvenid@ al tutorial");
		texts.Add ("Aquí aprenderás las bases de Slimers");
		texts.Add ("¿Estás list@ para empezar?");
		/*
		chainTextDialog.SetTextList (texts);
		chainTextDialog.SetOnClickFunction (
			() => {
				List<String> texts1 = new List<String>();
				texts1.Add ("Este es tu slime.\nHaz click para seleccionarlo");
				chainTextDialog.SetTextList(texts1);
				chainTextDialog.Show();
				chainTextDialog.SetOnClickFunction(
					()=>{
						List<String> texts2 = new List<String>();
						texts2.Add ("¿Has visto que ha aparecido alrededor?\nEsas son las casillas por las que tu slime se puede mover en una acción");
						texts2.Add ("Prueba a moverte aquí");
						chainTextDialog.SetTextList(texts2);
						chainTextDialog.Show();
						chainTextDialog.SetOnClickFunction(
							()=>{
								List<String> texts3 = new List<String>();
								texts3.Add ("Muy bien.\nAhora vuelve a donde estabas");
								texts3.Add ("Perfecto");
								chainTextDialog.SetTextList(texts3);
								chainTextDialog.Show();
								chainTextDialog.SetOnClickFunction(
									()=>{
										List<String> texts4 = new List<String>();
										texts4.Add ("¿Sabias que los slimes pueden hacer muchas mas cosas además de moverse?");
										texts4.Add ("Ya verás, si mantienes pulsado el slime verás que se marcan alrededor unas casillas.\nPrueba a arrastrar y soltar encima de esas casillas a ver que pasa");
										chainTextDialog.SetTextList(texts4);
										chainTextDialog.Show();
										chainTextDialog.SetOnClickFunction(
											()=>{
												chainTextDialog.Hide();
											});
									});
							});
					});
		});
		*/
		/*
		texts.Add ("Este es tu slime.\nHaz click para seleccionarlo");
		//--
		texts.Add ("¿Has visto que ha aparecido alrededor?\nEsas son las casillas por las que tu slime se puede mover en una acción");
		texts.Add ("Prueba a moverte aquí");
		//--
		texts.Add ("Muy bien.\nAhora vuelve a donde estabas");
		texts.Add ("Perfecto");
		//--
		texts.Add ("¿Sabias que los slimes pueden hacer muchas mas cosas además de moverse?");
		texts.Add ("Ya verás, si mantienes pulsado el slime verás que se marcan alrededor unas casillas.\nPrueba a arrastrar y soltar encima de esas casillas a ver que pasa");
		//--
		texts.Add ("Wow, se han separado");
		texts.Add ("Como has conseguido separarte puedes ovservar que la cantidad de acciones que puedes hacer en un turno ha aumentado");
		texts.Add ("Mueve el nuevo slime aquí y júntalo con el anterior");
		//--
		texts.Add ("¡Cuidado! Un slime enemigo ha aparecido, prueba a atacarle");
		texts.Add ("Mmmmm. Parece que tu ataque no le ha hecho mucho daño");
		//--
		texts.Add ("¿Por que no conviertes a tu slime en un slime de fuego, a ver si así haces mas daño?");
		texts.Add ("Para ello, primero mueve tu slime a esta casilla de fuego");
		texts.Add ("Ahora selecciona tu slime y vuelve a hacer click en el");
		//--
		texts.Add ("Así se hace, ahora tu slime tiene el <b>recubrimiento<b> de fuego");
		texts.Add ("Ahora vuelve a atacar al slime enemigo");
		//--
		texts.Add ("Has conseguido acabar con él");
		texts.Add ("Ahora ya conoces las bases de <b>Slimers<b>");
		texts.Add ("Explora los diferentes retos para descubrir sus verdaderos límites, si es que los tienen");
		*/
		chainTextDialog.SetTextList(texts);
		chainTextDialog.SetOnClickFunction (
			()=>{
				marker.SetActive (true);
				inputController.SetActiveInput(true);
				List<String> t = new List<String>();
				t.Add ("Este es tu slime.\nHaz click para seleccionarlo");
				chainTextDialog.SetTextList (t);
				chainTextDialog.SetOnClickFunction (() => {
				});
				chainTextDialog.Show ();
			}
		);
		inputController.SetActiveMove (false);
		inputController.SetActiveAttack (false);
		inputController.SetActiveEat (false);
		inputController.SetActiveJoin (false);
		inputController.SetActiveSplit (false);
		inputController.SetActiveConquer (false);
	}

	private void InitMarker(){

		marker = new InGameMarker ();
		marker.SetSprite (SpritesLoader.GetInstance().GetResource("Panels/selection_arrow"));

	}

	public TutorialFSMStatus GetTutorialStatus(){
		return tutorialStatus;
	}

	public void tutorialFSMCheck(){
		List<String> texts = new List<String>();
		switch (tutorialStatus) {
		case TutorialFSMStatus.SELECTSLIME:
			chainTextDialog.Hide ();
			texts.Add ("¿Has visto que ha aparecido alrededor?\nEsas son las casillas por las que tu slime se puede mover en una acción");
			texts.Add ("Prueba a moverte aquí");
			chainTextDialog.SetTextList (texts);
			chainTextDialog.Show ();
			chainTextDialog.SetOnClickFunction (() => {
				inputController.SetActiveInput (true);
				marker.SetActive(true);
				marker.SetParentTransform (MapDrawer.GetTileAt (-1, -3).transform);
			});
			inputController.SetActiveInput (false);
			inputController.SetActiveMove (true);
			tutorialStatus = TutorialFSMStatus.MOVEFIRSTSLIME;
			break;
		case TutorialFSMStatus.MOVEFIRSTSLIME:
			Debug.Log ("Sida");
			break;
		case TutorialFSMStatus.RETURNSLIME:
			break;
		case TutorialFSMStatus.SPLITSLIME:
			break;
		case TutorialFSMStatus.MOVESECONDARYSLIME:
			break;
		case TutorialFSMStatus.JOINSLIME:
			break;
		case TutorialFSMStatus.ATTACKSLIME:
			break;
		case TutorialFSMStatus.MOVETOCONQUER:
			break;
		case TutorialFSMStatus.CONQUERTILE:
			break;
		case TutorialFSMStatus.ATTACKWITHFIRE:
			break;
		}
	}

	public enum TutorialFSMStatus{
		SELECTSLIME,
		MOVEFIRSTSLIME,
		RETURNSLIME,
		SPLITSLIME,
		MOVESECONDARYSLIME,
		JOINSLIME,
		ATTACKSLIME,
		MOVETOCONQUER,
		CONQUERTILE,
		ATTACKWITHFIRE,
		TUTORIALENDED
	}

	public override List<Tile> GetJoinTile(Slime slime){
		return new List<Tile>();
	}

	public override List<Tile> GetSplitRangeTiles(Slime slime){
		return new List<Tile>();
	}

	public override List<Tile> GetSlimesInAttackRange(Slime slime){
		return new List<Tile>();
	}

	public override List<Tile> GetPossibleMovements(Slime slime){
		List<Tile> tiles = new List<Tile>();
		switch(tutorialStatus){
		case TutorialFSMStatus.MOVEFIRSTSLIME:
			tiles.Add (MapDrawer.GetTileAt (-1, -3));
			break;
		case TutorialFSMStatus.RETURNSLIME:
			tiles.Add (MapDrawer.GetTileAt (0, -2));
			break;
		case TutorialFSMStatus.MOVESECONDARYSLIME:
			break;
		case TutorialFSMStatus.MOVETOCONQUER:
			break;
		}
		return tiles;
	}

	protected override Player IsGameEndedAndWinner ()
	{
		if (tutorialStatus == TutorialFSMStatus.TUTORIALENDED) {
			return players [0];
		} else {
			return null;
		}
	}

}