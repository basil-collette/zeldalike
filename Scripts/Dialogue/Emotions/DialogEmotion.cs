using UnityEngine;

[System.Serializable]
public class DialogEmotion
{
    public DialogEmotionType Type;
    [Range(-3, 3)] public float MinPitch = 1;
    [Range(-3, 3)] public float MaxPitch = 1;
    [Range(0, 2)] public float Speed = 0.05f;
}
