using UnityEngine;

[System.Serializable]
public class SlimeCore
{
    private SlimeCoreTypes type;

    [SerializeField]
    private string name;

    [SerializeField]
    private int movementRange;

    [SerializeField]
    private int attackRange;

    [SerializeField]
    private int attack;

    [SerializeField]
    private int mass;


    public static SlimeCore Create(SlimeCoreTypes type)
    {
        string path = SlimeCoreTypesCtrl.GetPath(type);
        TextAsset reader = Resources.Load (path) as TextAsset;
        SlimeCore result = JsonUtility.FromJson<SlimeCore>(reader.text);
        result.SetCoreType(type);
        return result;
    }

    public override string ToString()
    {
        return "SLIME --> Name: " + name + "MovementRange: " + movementRange + ", AttackRange: " + attackRange;
    }

    public int GetMovementRange(){
        return this.movementRange;
    }

    public int GetAttackRange(){
        return this.attackRange;
    }

    public void SetCoreType(SlimeCoreTypes type){
        this.type = type;
    }

    public SlimeCoreTypes GetCoreType(){
        return type;
    }
}