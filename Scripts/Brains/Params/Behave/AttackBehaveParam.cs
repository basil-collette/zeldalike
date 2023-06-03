using UnityEngine;

public class AttackBehaveParam : BehaveParam
{
    public Collider2D attackCollider;
    public Transform targetTransform;
    public Vector3 targetPos;
    public float attackDuration;
    public float cooldown;

    public AttackBehaveParam()
    {

    }

}