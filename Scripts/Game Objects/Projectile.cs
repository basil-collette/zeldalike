using UnityEngine;

[System.Serializable]
public class Projectile : TriggerHit
{
    [SerializeReference] public float speed = 1;
    [SerializeReference] public float aliveMaxDuration = 6f;
    public Vector3 direction = Vector3.zero;

    protected bool canCollid = false;

    void Update()
    {
        aliveMaxDuration -= Time.deltaTime;

        if (aliveMaxDuration <= 0)
            Destroy(gameObject);

        if (GetComponent<Rigidbody2D>().velocity == Vector2.zero)
            GetComponent<Rigidbody2D>().velocity = direction.normalized * speed;
    }

    protected new void OnTriggerEnter2D(Collider2D collider)
    {
        if (!canCollid)
            return;

        Hitable hitableCollider = collider.GetComponent<Hitable>();

        if (hitableCollider != null)
        {
            base.OnHit(hitableCollider);
        }

        Destroy(gameObject);
    }

    protected virtual void OnTriggerExit2D(Collider2D collider)
    {
        canCollid = true;
    }

}
