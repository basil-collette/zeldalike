using UnityEngine;

[System.Serializable]
public class CameraParameters
{
    public float Smoothing = 0.05f;
    public bool IsFixed = false;

    public Vector2 MinPos = new Vector2(-5, -5); //Left Bottom
    public Vector2 MaxPos = new Vector2(5, 5); //Right Top
}
