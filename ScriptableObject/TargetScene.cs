﻿using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/TargetScene")]
public class TargetScene : ScriptableObject //, ISerializationCallbackReceiver
{

    public string libelle;
    public bool needPreload = false;
    public string[] scenesNeedingPreloadNames = new string[] { };

}