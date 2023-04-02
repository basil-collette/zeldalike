using UnityEngine;

public abstract class Brain : MonoBehaviour
{
    public abstract Vector3? Think(ThinkParam? param);

    public abstract short? Behave(BehaveParam? param);

}