using UnityEngine;

[System.Serializable]
public class CollideHit : Hit
{

    private new void Start()
    {
        base.Start();
    }

    protected virtual void OnCollisionEnter2D(Collision2D collider)
    {
        Hitable hitableCollider = collider.gameObject.GetComponent<Hitable>();

        if (hitableCollider != null && hitableCollider.enabled)
        {
            base.OnHit(hitableCollider);
        }
    }
/*
    protected virtual void OnCollisionStay2D(Collision2D collider)
    {
        Hitable hitableCollider = collider.gameObject.GetComponent<Hitable>();

        if (hitableCollider != null)
        {
            base.OnHit(hitableCollider);
        }
    }
*/
}
