using Assets.Scripts.Enums;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Items/Weapon")]
public class WeaponScriptable : ItemScriptable
{
    public WeaponTypeEnum WeaponType;
    public float AttackDelay = 1;
    public float Speed = 1; //1 is the animator normal speed
}
