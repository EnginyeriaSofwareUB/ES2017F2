using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameOverInfo {

    static Player winner;
    static List<Player> losers;

    public static void Init ()
    {
        winner=null;
        losers = new List<Player>();
    }

    public static void SetLoser(Player player)
    {
        losers.Add(player);
    }
    public static void SetWinner (Player player)
    {
        winner=player;
    }

    public static Player GetWinner (){
        return winner;
    }

    public static List<Player> GetLosers(){
        return losers;
    }
}
