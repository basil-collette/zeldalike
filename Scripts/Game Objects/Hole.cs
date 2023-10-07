using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Hole : MonoBehaviour
{
    protected void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.isTrigger
            && collider.TryGetComponent(out AliveEntity entity))
        {
            Vector3 fallPos = DirectionHelper.GetPosAfterDirection(entity.transform.position, entity.direction / 2);
            Vector3 respawnPos = DirectionHelper.GetPosAfterDirection(entity.transform.position, -entity.direction / 2);

            entity.Fall(fallPos, respawnPos);
        }
    }

}