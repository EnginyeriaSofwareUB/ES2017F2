using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Challenge{

    string name;
    string description;

    public Challenge(string name, string description)
    {
        this.name = name;
        this.description = description;
    }

    public string ToString()
    {
        return name + ": \n" + description;
    }
}
