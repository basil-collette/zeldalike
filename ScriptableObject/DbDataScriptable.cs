using UnityEngine;

[System.Serializable]
public class DbDataScriptable : ScriptableObject
{
    public int Id;
    public string NameLibelle;
    public string NameCode;
    public bool Actif = true;
}