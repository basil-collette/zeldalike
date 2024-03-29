﻿using UnityEngine;

public abstract class DirectionHelper
{
    public static void PointTo(Transform transform, Vector2 target, int? angleOffset = null)
    {
        Vector2 direction = target - (Vector2)transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angleOffset != null) angle += (int)angleOffset;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        float scalingFactor = 1; // Bigger for slower
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, Time.deltaTime / scalingFactor);

        //transform.rotation = Quaternion.AngleAxis(currentQuantity + modAngle, Vector3.forward * Time.deltaTime);
    }

    public static Vector3 RotateVector3DirectionByAngle(Vector3 direction, float angle)
    {
        return Quaternion.AngleAxis(angle, Vector3.forward) * direction;
    }

    public static Vector3 GetDirection(Vector3 position, Vector3 target)
    {
        return target - position;
    }

    public static Vector3 GetPosAfterDirection(Vector3 position, Vector3 direction)
    {
        return position + direction;
    }

    public static Vector3 GetRelativeAxis(Vector3 position, Vector3 target)
    {
        return GetAxis(target - position);
    }

    public static Vector3 GetAxis(Vector3 direction)
    {
        if (direction == Vector3.zero)
            return Vector3.zero;

        float angleDown = Vector3.Angle(direction, Vector3.down);
        float angleLeft = Vector3.Angle(direction, Vector3.left);
        float angleRight = Vector3.Angle(direction, Vector3.right);
        float angleUp = Vector3.Angle(direction, Vector3.up);

        float minAngle = Mathf.Min(angleDown, angleLeft, angleRight, angleUp);

        if (minAngle == angleUp) { return Vector3.up; }
        else if (minAngle == angleLeft) { return Vector3.left; }
        else if (minAngle == angleRight) { return Vector3.right; }
        else { return Vector3.down; }
    }

    public static bool IsFacingUp(Vector3 direction)
    {
        if (direction == Vector3.zero)
            return false;
        
        float angleLeft = Vector3.Angle(direction, Vector3.left);
        float angleRight = Vector3.Angle(direction, Vector3.right);
        float angleUp = Vector3.Angle(direction, Vector3.up);

        return Mathf.Min(angleLeft, angleRight, angleUp) == angleUp;
    }

    public static bool IsFacingDown(Vector3 direction)
    {
        if (direction == Vector3.zero)
            return false;

        float angleDown = Vector3.Angle(direction, Vector3.down);
        float angleLeft = Vector3.Angle(direction, Vector3.left);
        float angleRight = Vector3.Angle(direction, Vector3.right);

        return Mathf.Min(angleDown, angleLeft, angleRight) == angleDown;
    }

    public static bool IsFacingLeft(Vector3 direction)
    {
        if (direction == Vector3.zero)
            return false;

        float angleDown = Vector3.Angle(direction, Vector3.down);
        float angleLeft = Vector3.Angle(direction, Vector3.left);
        float angleUp = Vector3.Angle(direction, Vector3.up);

        return Mathf.Min(angleDown, angleLeft, angleUp) == angleLeft;
    }

    public static bool IsFacingRight(Vector3 direction)
    {
        if (direction == Vector3.zero)
            return false;

        float angleDown = Vector3.Angle(direction, Vector3.down);
        float angleRight = Vector3.Angle(direction, Vector3.right);
        float angleUp = Vector3.Angle(direction, Vector3.up);

        return Mathf.Min(angleDown, angleRight, angleUp) == angleRight;
    }

    public static float MapValue(float value, float minValue, float maxValue, bool willClamp = true)
    {
        if (maxValue > minValue)
        {
            float mappedValue = (value - minValue) / (maxValue - minValue);
            if (willClamp)
            {
                mappedValue = Mathf.Clamp(mappedValue, 0.0f, 1.0f);
            }
            return mappedValue;
        }
        else
        {
            return (value >= minValue) ? 1.0f : 0.0f;
        }
    }

    public static float WrapAngle(float angle, float min, float max)
    {
        float range = max - min;
        angle = (angle - min) % range;
        if (angle < 0)
            angle += range;
        return angle + min;
    }

    //move to an appropriate place
    /*
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
    */

}