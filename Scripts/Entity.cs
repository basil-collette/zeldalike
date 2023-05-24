using UnityEngine;

public class Entity : Thinker
{
    [HideInInspector] public Vector3 anchorPosition;

    protected void Start()
    {
        this.anchorPosition = transform.position;
    }

    protected void Update()
    {
        
    }

    public virtual bool TargetIsNear(Vector3 targetPosition, float radius)
    {
        return Vector2.Distance(transform.position, targetPosition) <= radius;
    }

}
