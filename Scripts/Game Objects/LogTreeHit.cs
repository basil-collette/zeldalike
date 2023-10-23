using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogTreeHit : Hitable
{
    [SerializeField] float spawnCooldownDuration;
    public GameObject logPrefab;
    public int maxSpawnCount = 4;

    List<EffectTypeEnum> effectTypeTriggerable = new List<EffectTypeEnum>() {
        EffectTypeEnum.neutral,
        EffectTypeEnum.slash,
        EffectTypeEnum.bump,
        EffectTypeEnum.pierce
    };

    bool canSpawn = true;

    void Start()
    {
        
    }

    public override void Effect(Vector3 attackerPos, Effect effect)
    {
        //
    }

    public override void Hit(GameObject attacker, List<Effect> hit, string attackerTag)
    {
        if (canSpawn
            && attackerTag == "Player" // prevent the "friendly fire" from enemies 
            && hit.Exists(effect => effectTypeTriggerable.Contains(effect.effectType))
            && SpawnCountNotReached())
        {
            canSpawn = false;
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

        StartCoroutine(SpawnCooldownCo());
    }

    Vector3 GetRandomSeedThrowDirection()
    {
        Vector3 reference = transform.position; //8.22 1.49

        Vector3 newPos = Vector3.zero;
        newPos.x = Random.Range(reference.x - 1f, reference.x + 1f);
        newPos.y = Random.Range(reference.y, reference.y + 1f);

        Vector3 relativeDirection = DirectionHelper.GetDirection(transform.position, newPos).normalized;

        relativeDirection.x *= Random.Range(5, 6);
        relativeDirection.y *= Random.Range(5, 10);

        return relativeDirection;
    }

    IEnumerator SpawnCooldownCo()
    {
        yield return new WaitForSeconds(spawnCooldownDuration);

        canSpawn = true;
    }

}
