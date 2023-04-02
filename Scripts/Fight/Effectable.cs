using UnityEngine;

[System.Serializable]
public abstract class Effectable : MonoBehaviour
{
    public abstract void Effect(Vector3 attackerPos, Effect effect);

}