using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public string _name;
    public float _cooldownTime;
    public float _activeTime;

    public abstract void Activate(GameObject parent);
    public abstract void AfterActivate(GameObject parent);

}