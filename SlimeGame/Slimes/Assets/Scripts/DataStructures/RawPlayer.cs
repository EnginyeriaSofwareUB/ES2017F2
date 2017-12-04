
using System;
using System.Collections.Generic;

public class RawPlayer{
	public StatsContainer statsCoreInfo;
    private float actionsPerSlime;
	private List<RawSlime> slimes;
	private int conquered;

    public RawPlayer(StatsContainer stats, float actionsPerSlime, int conquered){
        this.statsCoreInfo = stats;
        this.actionsPerSlime = actionsPerSlime;
        this.conquered = conquered;
    }

    public void SetSlimes(List<RawSlime> slimes){
        this.slimes = slimes;
    }

    public List<RawSlime> GetSlimes(){
        return this.slimes;
    }

    public override string ToString(){
        string toReturn = "PLAYER (Conquered: " + conquered + " tiles)\n";
        foreach(RawSlime sl in slimes){
            toReturn += (sl.ToString() + "\n");
        }
        return toReturn;
    }
}