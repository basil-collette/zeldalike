using UnityEngine;

[System.Serializable]
public class TriggerHit : Hit
{
    private new void Start()
    {
        base.Start();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Hitable hitableCollider = collider.GetComponent<Hitable>();

        if (hitableCollider != null)
        {
            base.OnHit(hitableCollider);
        }
    }

}
