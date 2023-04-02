using UnityEngine;

public class ChaseBrain : Brain
{
    public string targetTag = "Player";
    public GameObject target;
    public float detectionRange = 4;

    private void Start()
    {
        this.target = GameObject.FindGameObjectWithTag(targetTag);
    }

    public override Vector3? Think(ThinkParam param = null)
    {
        if (Vector2.Distance(transform.position, target.transform.position) > detectionRange)
        {
            return Vector3.zero;
        }

        return target.transform.position;
    }

    public override short? Behave(BehaveParam param)
    {
        return null;
    }

}
