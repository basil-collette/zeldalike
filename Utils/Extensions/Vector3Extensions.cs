using System;
using UnityEngine;

public static class Vector3Extensions
{
    /// <summary>
    /// Sets any values of the Vector3
    /// </summary>
    public static Vector3 With(this Vector3 vector, float? x = null, float? y = null, float? z = null)
    {
        return new Vector3(x ?? vector.x, y ?? vector.y, z ?? vector.z);
    }

    /// <summary>
    /// Adds to any values of the Vector3
    /// </summary>
    public static Vector3 Add(this Vector3 vector, float? x = null, float? y = null, float? z = null)
    {
        return new Vector3(vector.x + (x ?? 0), vector.y + (y ?? 0), vector.z + (z ?? 0));
    }

    public static Vector3 GetClosestVector2From(this Vector3 vector, Vector3[] otherVectors)
    {
        if (otherVectors.Length == 0) throw new Exception("The list of other vectors is empty");
        var minDistance = Vector3.Distance(vector, otherVectors[0]);
        var minVector = otherVectors[0];
        for (var i = otherVectors.Length - 1; i > 0; i--)
        {
            var newDistance = Vector3.Distance(vector, otherVectors[i]);
            if (newDistance < minDistance)
            {
                minDistance = newDistance;
                minVector = otherVectors[i];
            }
        }
        return minVector;
    }

}