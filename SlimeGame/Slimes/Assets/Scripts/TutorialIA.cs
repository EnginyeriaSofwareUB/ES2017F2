using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialIA : AIInterface {


    public SlimeAction GetAction(GameController gameController) {
        gameController.SetSelectedSlime(gameController.GetCurrentPlayer().GetSlimes()[0]);
        return new SlimeAction(ActionType.CONQUER, gameController.GetCurrentPlayer().GetSlimes()[0].actualTile);
    }
}
