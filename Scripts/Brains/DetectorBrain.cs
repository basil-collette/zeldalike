using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DetectorBrain : Brain
{
    public List<Sense> senses;

    public float detectionRange = 4;
    public bool needDirectSee = true;

    protected Animator animator;
    //protected Vector3 lastPositionKnown;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override Vector3? Think(ThinkParam param)
    {
        Transform target = ((TargetThinkParam)param).target;
        Vector3 targetPos = target.position;

        if (Vector2.Distance(transform.position, targetPos) > detectionRange)
        {
            GetComponent<Animator>().SetBool("targeting", false);
            return Vector3.zero;
        }

        GetComponent<Animator>().SetBool("targeting", true);

        Vector3 direction = DirectionHelper.GetDirection(transform.position, targetPos);

        if (!needDirectSee)
        {
            animator.SetFloat("moveX", direction.x);
            animator.SetFloat("moveY", direction.y);
            return targetPos;
        }

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, (targetPos - transform.position), detectionRange);
        if (ReferenceEquals(hitInfo.transform.gameObject, target.gameObject))
        {
            animator.SetFloat("moveX", direction.x);
            animator.SetFloat("moveY", direction.y);
            return targetPos;
        }

        return Vector3.zero;
    }

    public override short? Behave(BehaveParam param)
    {
        return null;
    }

}

[Serializable]
public class Sense
{
    public SenseEnum type;
    public float distance;
    public AngleDegree[] schema;
}

[Serializable]
public class AngleDegree
{
    public float degree;
    public float angle;
}