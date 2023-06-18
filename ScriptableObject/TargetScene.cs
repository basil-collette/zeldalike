using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/TargetScene")]
public class TargetScene : ScriptableObject
{
    public string libelle;
    public bool needPreload = false;
    public string[] scenesNeedingPreloadNames = new string[] { };
    public string musicName;
    public CameraParameters cameraParameters;
}
