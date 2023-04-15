using UnityEngine;

public static class ColliderHelper
{

    public static float GetColliderDistance(Vector3 originPos, Vector3 direction, float maxDistance)
    {
        return ColliderHelper.GetColliderDistance(originPos, direction, maxDistance, Color.blue);
    }

    public static float GetColliderDistance(Vector3 originPos, Vector3 direction, float maxDistance, Color color)
    {
        RaycastHit2D raycast = Physics2D.Raycast(originPos, direction, maxDistance);
        //Debug.DrawRay(originPos, direction, color, 0.01f);
        return raycast.collider == null ? maxDistance + 1 : raycast.distance;
    }

    public static bool FirstDirectionHaveLessCollisions(Vector3 clockwiseDirection, Vector3 antiClockwiseDirection, Vector3 originPos, float maxDistance)
    {
        return GetColliderDistance(originPos, clockwiseDirection, maxDistance) >= GetColliderDistance(originPos, antiClockwiseDirection, maxDistance);
    }

    public static bool IsTargetVisible(Vector3 originPos, Transform target, float maxDistance)
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(originPos, target.position, maxDistance);

        return hitInfo.collider == null
            || ReferenceEquals(hitInfo.transform.gameObject, target.gameObject);
    }

}