using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : Thinker
{
    public Transform anchorPosition;

    protected Rigidbody2D rigidbody;

    protected void Start()
    {
        this.anchorPosition = transform;

        this.rigidbody = GetComponent<Rigidbody2D>();
    }

    protected void Update()
    {
        
    }

    public virtual bool TargetIsNear(Vector3 targetPosition, float radius)
    {
        return Vector2.Distance(transform.position, targetPosition) <= radius;
    }

}
