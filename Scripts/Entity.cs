using UnityEngine;

public class Entity : Thinker
{
    public Transform anchorPosition;

    protected void Start()
    {
        this.anchorPosition = transform;
    }

    protected void Update()
    {
        
    }

    public virtual bool TargetIsNear(Vector3 targetPosition, float radius)
    {
        return Vector2.Distance(transform.position, targetPosition) <= radius;
    }

}
