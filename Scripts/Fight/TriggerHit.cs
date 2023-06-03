using UnityEngine;

[System.Serializable]
public class TriggerHit : Hit
{
    protected new void Start()
    {
        base.Start();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        Hitable hitableCollider = collider.GetComponent<Hitable>();

        if (hitableCollider != null)
        {
            base.OnHit(hitableCollider);
        }
    }

}
