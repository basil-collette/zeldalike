using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Tilemaps;

[System.Serializable]
public class Projectile : TriggerHit
{
    public float speed = 1;
    public float aliveMaxDuration = 6f;

    protected float _lifeTime = 0;
    protected bool _canCollid = false;
    protected bool _initialized = false;
    protected Vector3 _direction;
    protected ObjectPool<Projectile> _pool;

    public void Init(Vector3 pos, Vector3 direction, ObjectPool<Projectile> pool = null)
    {
        transform.position = pos;
        Init(direction, pool);
    }
    public void Init(Vector3 direction, ObjectPool<Projectile> pool = null)
    {
        _lifeTime = 0;
        _initialized = true;
        _canCollid = false;
        _pool = pool;

        _direction = direction;

        gameObject.SetActive(true);

        //Invoke(nameof(Destroy), aliveMaxDuration);
    }

    void Update()
    {
        if (!_initialized) return;

        _lifeTime += Time.deltaTime;
        if (_lifeTime >= aliveMaxDuration)
        {
            Destroy();
            return;
        }

        if (GetComponent<Rigidbody2D>().velocity == Vector2.zero)
            GetComponent<Rigidbody2D>().velocity = _direction.normalized * speed;
    }

    protected new void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider is TilemapCollider2D)
        {
            Destroy();
            return;
        }

        if (!_canCollid)
            return;

        Hitable hitableCollider = collider.GetComponent<Hitable>();
        if (hitableCollider != null && hitableCollider.enabled)
        {
            base.OnHit(hitableCollider);

            Destroy();
            return;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collider)
    {
        _canCollid = true;
    }

    void Destroy()
    {
        if (_pool != null)
        {
            _pool.Release(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
