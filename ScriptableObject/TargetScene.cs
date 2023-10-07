using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/TargetScene")]
public class TargetScene : ScriptableObject
{
    public string nameLibelle;
    public string nameCode;
    public bool needPreload = false;
    public string[] scenesNeedingPreloadNames = new string[] { };
    public string musicName;
    public CameraParameters cameraParameters;
}
