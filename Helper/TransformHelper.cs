using UnityEngine;

public abstract class TransformHelper
{
    public static void LookAt(Transform transform, Vector2 target)
    {
        const int ANGLE_CORRECTOR = -90; //-90 to towarding up at start

        Vector2 direction = target - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + ANGLE_CORRECTOR;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        float scalingFactor = 1; // Bigger for slower
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, Time.deltaTime / scalingFactor);

        //transform.rotation = Quaternion.AngleAxis(currentQuantity + modAngle, Vector3.forward * Time.deltaTime);
    }

    public static void FallTo(Transform transform, Vector2 target)
    {
        
    }

    public static void Slash(Transform transform, Vector2 target)
    {
        
    }

    public static void Pierce(Transform transform, Vector2 target)
    {
        
    }

    public static void JumpTo(Transform transform, Vector2 target)
    {
        
    }

    public static Vector3 GetRelativeAxis(Vector3 position, Vector3 target)
    {
        return GetAxis(target - position);
    }

    public static Vector3 GetAxis(Vector3 position)
    {
        float angleUp = Vector3.Angle(position, Vector3.up);
        float angleLeft = Vector3.Angle(position, Vector3.left);
        float angleRight = Vector3.Angle(position, Vector3.right);
        float angleDown = Vector3.Angle(position, Vector3.down);

        float minAngle = Mathf.Min(angleUp, angleLeft, angleRight, angleDown);

        if (minAngle == angleUp) { return Vector3.up; }
        else if (minAngle == angleLeft) { return Vector3.left; }
        else if (minAngle == angleRight) { return Vector3.right; }
        else { return Vector3.down; }
    }

}