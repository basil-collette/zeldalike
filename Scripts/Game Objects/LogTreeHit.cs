using System.Collections.Generic;
using UnityEngine;

public class LogTreeHit : Hitable
{
    public bool hasGrown;
    public GameObject logPrefab;
    public int maxSpawnCount = 4;

    List<EffectEnum> effectTypeTriggerable = new List<EffectEnum>() { 
        EffectEnum.neutral,
        EffectEnum.slash,
        EffectEnum.bump,
        EffectEnum.pierce
    };

    void Start()
    {
        hasGrown = true;
    }



    public override void Effect(Vector3 attackerPos, Effect effect)
    {
        //
    }

    public override void Hit(Vector3 attackerPos, List<Effect> hit)
    {
        if (hasGrown
            && hit.Exists(effect => effectTypeTriggerable.Contains(effect.effectType))
            && SpawnCountNotReached())
        {
            SpawnLog();
        }
    }

    bool SpawnCountNotReached()
    {
        return FindObjectsOfType<LogThinker>().Length + FindObjectsOfType<TimedAutoReplace>().Length < maxSpawnCount;
    }

    void SpawnLog()
    {
        GameObject logSeed = Instantiate(logPrefab, transform.position, Quaternion.identity);
        logSeed.GetComponent<Rigidbody2D>().velocity = GetRandomSeedThrowDirection();
    }

    Vector3 GetRandomSeedThrowDirection()
    {
        Vector3 reference = transform.position; //8.22 1.49

        Vector3 newPos = Vector3.zero;
        newPos.x = Random.Range(reference.x - 1f, reference.x + 1f);
        newPos.y = Random.Range(reference.y, reference.y + 1f);

        Vector3 relativeDirection = DirectionHelper.GetDirection(transform.position, newPos).normalized;

        relativeDirection.x *= Random.RandomRange(5, 6);
        relativeDirection.y *= Random.RandomRange(5, 10);

        return relativeDirection;
    }

}
