using System.Collections.Generic;
using UnityEngine;

public abstract class Hitable : Effectable
{
    [SerializeField] protected AudioClip hitSound;

    public abstract void Hit(GameObject attacker, List<Effect> hit);
}