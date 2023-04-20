using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/TargetScene")]
public class TargetScene : ScriptableObject //, ISerializationCallbackReceiver
{

    public string name = "Anonymous Scene";
    public bool needPreload = false;
    public string[] scenesNeedingPreloadNames = new string[] { };

}
