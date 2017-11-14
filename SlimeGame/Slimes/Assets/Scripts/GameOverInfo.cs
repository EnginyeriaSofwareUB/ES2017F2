using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverInfo {

    static List<Player> winners;
    static List<Player> losers;

    public static void Init ()
    {
        winners = new List<Player>();
        losers = new List<Player>();
    }

    public static void SetLoser(Player player)
    {
        losers.Add(player);
    }
    public static void SetWinner (Player player)
    {
        winners.Add(player);
    }
}
