
using System;
using System.Collections.Generic;

public class RawPlayer{
	public StatsContainer statsCoreInfo;
    private float actionsPerSlime;

    private int turnActions;
	private List<RawSlime> slimes;
	private List<TileData> conqueredTiles;

    public RawPlayer(StatsContainer stats, float actionsPerSlime){
        this.statsCoreInfo = stats;
        this.actionsPerSlime = actionsPerSlime;
        this.conqueredTiles = new List<TileData>();
    }

    public void SetSlimes(List<RawSlime> slimes){
        this.slimes = slimes;
    }

    public List<RawSlime> GetSlimes(){
        return this.slimes;
    }

    public void AddSlime(RawSlime slime){
        this.slimes.Add(slime);
    }

    public void RemoveSlime(RawSlime sl){
        sl.GetTileData().SetSlimeOnTop((RawSlime)null);
		this.slimes.Remove(sl);
	}

    public void UpdateActions(){
        this.turnActions = (int)actionsPerSlime * slimes.Count;
    }

    public int GetActions(){
        return turnActions;
    }

    public void Conquer(TileData tile){
        tile.Conquer(this);
        conqueredTiles.Add(tile);
    }

    public List<TileData> GetConqueredTiles(){
        return conqueredTiles;
    }

    public override string ToString(){
        string toReturn = "PLAYER (Conquered: " + conqueredTiles.Count + " tiles)\n";
        foreach(RawSlime sl in slimes){
            toReturn += (sl.ToString() + "\n");
        }
        return toReturn;
    }

    public RawPlayer GetCopy(){
        RawPlayer rawPlayer = new RawPlayer(statsCoreInfo, actionsPerSlime);
		List<RawSlime> rawSlimes = new List<RawSlime>();
		foreach(RawSlime sl in slimes){
			RawSlime rawSl = sl.GetCopy();
			rawSl.SetPlayer(rawPlayer);
			rawSlimes.Add(rawSl);
		}
		rawPlayer.SetSlimes(rawSlimes);
		return rawPlayer;
    }
}