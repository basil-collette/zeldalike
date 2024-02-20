using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Hole : MonoBehaviour
{
    protected void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.isTrigger
            && collider.TryGetComponent(out AliveEntity entity))
        {
            Vector3 collidePos = GetComponent<Collider2D>().ClosestPoint(entity.transform.position);

            Vector3 dir = DirectionHelper.GetDirection(entity.transform.position, collidePos);

            Vector3 fallPos = DirectionHelper.GetPosAfterDirection(entity.transform.position, dir * 2);

            Vector3 respawnPos = DirectionHelper.GetPosAfterDirection(entity.transform.position, -(dir / 2));

            entity.Fall(fallPos, respawnPos);
        }
    }
}