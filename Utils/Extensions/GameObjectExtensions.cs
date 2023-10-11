using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtensions
{
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        T component = gameObject.GetComponent<T>();
        return component != null ? component : gameObject.AddComponent<T>();
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        for (var i = list.Count - 1; i > 1; i--)
        {
            var j = Random.Range(0, i + 1);
            var value = list[j];
            list[j] = list[i];
            list[i] = value;
        }
    }

    /// <summary>
    /// change the current direction of a rigidbody while keeping it’s velocity
    /// </summary>
    /// <param name="rb"></param>
    /// <param name="direction"></param>
    public static void ChangeDirection(this Rigidbody2D rb, Vector3 direction)
    {
        rb.velocity = direction.normalized * rb.velocity.magnitude;
    }

}