﻿using UnityEngine;

public abstract class Bot : AliveEntity
{
    public string targetTag = "Player";
    public Transform target;
    public float attackRadius = 1f;
    public float health = 1;

    protected bool targetting;

    protected new void Start()
    {
        base.Start();

        Health healthComp = GetComponent<Health>();
        if (healthComp != null)
        {
            healthComp.health = ScriptableObject.CreateInstance<FloatValue>();
            healthComp.health.initialValue = this.health;
            healthComp.health.RuntimeValue = this.health;
            /*
            healthComp.health = new FloatValue()
            {
                initialValue = this.health,
                RuntimeValue = this.health
            };
            */
        }

        this.targetting = false;

        //this.target = FindObjectOfType<Player>().transform;
        this.target = GameObject.FindGameObjectWithTag(targetTag).transform;

        this.nearRadius = this.attackRadius;
    }

    protected new void Update()
    {
        base.Update();
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public bool TargetIsOnAttackRadius()
    {
        return base.TargetIsNear(target.position, attackRadius);
    }

}
