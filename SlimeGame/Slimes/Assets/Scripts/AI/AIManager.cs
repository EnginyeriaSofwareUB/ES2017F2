using UnityEngine;

public class AIManager{

    public static AIInterface GetAIByVictoryCondition(GameController g, ModosVictoria modo){
        switch(modo){
            case ModosVictoria.ASESINATO:
                //Debug.Log("Asesinato AI");
                return new AIMurder(g);
                
            case ModosVictoria.CONQUISTA:
                //Debug.Log("Conquista AI");
                return new AIConquer(g);

            case ModosVictoria.MASA:
                //Debug.Log("Masa AI");
                return new AIMasa(g);
        
        }
        return new AIRandom(g);
    }
}