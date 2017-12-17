using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Challenge{

    string name;
    string description;
    int maxTurns;
    int modo;
    Matrix map;


    public Challenge(string name, string description, int maxTurns, int modo, int map)
    {
        this.name = name;
        this.description = description;
        this.maxTurns = maxTurns;
        this.modo = modo;
        this.map = new Matrix(MapParser.ReadMap((MapTypes) map));
    }

    public override string ToString()
    {
        return name + ": \n" + description;
    }
    public int GetModo()
    {
        return modo;
    }
    public int GetMaxTurns()
    {
        return maxTurns;
    }
    public Matrix GetMap()
    {
        return map;
    }
}
