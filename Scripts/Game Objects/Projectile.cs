using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class Projectile : TriggerHit
{
    public float speed = 1;
    public float aliveMaxDuration = 6f;
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
        if (collider is TilemapCollider2D)
        {
            Destroy(gameObject);
            return;
        }

        if (!canCollid)
            return;

        Hitable hitableCollider = collider.GetComponent<Hitable>();
        if (hitableCollider != null && hitableCollider.enabled)
        {
            base.OnHit(hitableCollider);

            Destroy(gameObject);
            return;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collider)
    {
        canCollid = true;
    }

}
