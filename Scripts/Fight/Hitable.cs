using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Hitable : Effectable
{
    public abstract void Hit(Vector3 attackerPos, List<Effect> hit);
}