using System.Collections;
using UnityEngine;

/*
public class Knockback : MonoBehaviour
{
    public float? thrust;
    public float? knockTime;

    void Start()
    {
        thrust ??= 4f;
        knockTime ??= 0.3f;
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.gameObject.GetComponent<AliveEntity>())
        {
            Rigidbody2D other = otherCollider.GetComponent<Rigidbody2D>();
            if (other != null)
            {
                other.GetComponent<AliveEntity>().currentEntityState = EntityState.stagger;
                Vector2 difference = other.transform.position - transform.position;
                difference = difference.normalized * (float)thrust;
                other.AddForce(difference, ForceMode2D.Impulse);

                StartCoroutine(KnockCo(other));
            }
        }
    }

    IEnumerator KnockCo(Rigidbody2D other)
    {
        yield return new WaitForSeconds((float)knockTime);
        other.velocity = Vector2.zero;
        other.GetComponent<AliveEntity>().currentEntityState = EntityState.idle;
    }

}
*/