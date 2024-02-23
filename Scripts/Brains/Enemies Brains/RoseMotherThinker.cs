using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class RoseMotherThinker : Bot
{
    public float openDuration;
    [SerializeField] GameObject[] enemiesPrefabs;
    [SerializeField] Animator _poisonGasAnimator;
    public GameObject rootParent;
    public Projectile spikePrefab;

    [SerializeField] GameObject plantDestroyEffect;
    [SerializeField] GameObject explosionSmokeEffect;
    [SerializeField] DialogueContainer successDialog;
    [SerializeField] GameObject gentleLittleOne;

    ObjectPool<Projectile> _spikeProjectilePool;    

    new void Start()
    {
        MainGameManager._soundManager.PlayEffect("monster_scream", 0.5f);

        base.Start();

        StartCoroutine(CycleCo());

        GetComponentInChildren<Health>()._dieOverride = () => { RoseDeath(); };
        GetComponentInChildren<Health>().OnDammaged += OnDammaged;

        _spikeProjectilePool = new ObjectPool<Projectile>(CreateSpike, null, OnSpikeBackPool, defaultCapacity: 16);
    }

    void OnSpikeBackPool(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
    }

    Projectile CreateSpike()
    {
        return Instantiate(spikePrefab);
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }

    void OnDammaged(float health)
    {
        if (health <= 2 && !_poisonGasAnimator.GetBool("activated"))
        {
            _poisonGasAnimator.SetBool("activated", true);
        }
    }

    void RoseDeath()
    {
        GetComponentInChildren<Health>().OnDammaged -= OnDammaged;

        _poisonGasAnimator.SetBool("activated", false);

        GameObject.Find("Wall").GetComponent<Animator>().SetTrigger("open");
        GameObject.Find("Scene Transition").GetComponent<BoxCollider2D>().enabled = true;

        MainGameManager._soundManager.StopMusic();

        StopAllCoroutines();

        GetComponent<Animator>().SetTrigger("die");
        StartCoroutine(OnDeathCo());
    }

    public IEnumerator OnDeathCo()
    {
        yield return new WaitForSeconds(0.4f);
        InactiveLittleOne();

        MainGameManager._soundManager.PlayEffect("monster_deny", 0.2f);
        Vector3 positionOrigin = transform.position;

        float shakingDuration = 3f;
        float shakingTimeEnd = Time.time + shakingDuration;
        while (Time.time < shakingTimeEnd)
        {
            Vector3 pos = positionOrigin - new Vector3(Random.RandomRange(-0.1f, 0.1f), Random.RandomRange(-0.1f, 0.1f), 1);
            transform.localPosition = pos;

            yield return new WaitForSeconds(0.1f);
        }

        Instantiate(plantDestroyEffect, transform.position, Quaternion.identity);
        Instantiate(explosionSmokeEffect, transform.position, Quaternion.identity);
        gentleLittleOne.SetActive(true);

        MainGameManager._soundManager.PlayEffect("teleport");

        yield return new WaitForSeconds(0.1f);

        Invoke(nameof(OnDeathAnimationEnd), 2f);

        gameObject.SetActive(false);
    }

    void OnDeathAnimationEnd()
    {
        MainGameManager._soundManager.PlayMusic("peacefull");

        FindAnyObjectByType<DialogueManager>().StartDialogue(successDialog);

        MainGameManager._storyEventManager.AddScenarioEvent("defeated_rose_mother");

        Destroy(gameObject);
    }

    IEnumerator CycleCo()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);

            MainGameManager._soundManager.PlayEffect("monster_movement");
            DeployRoot();

            //yield return new WaitForSeconds(5f);
            //yield return SpawnRandomEnemies();

            yield return new WaitForSeconds(5f);
            MainGameManager._soundManager.PlayEffect("monster_scream", 0.5f);

            yield return new WaitForSeconds(2f);
            yield return ThrowSpikesCo();

            yield return new WaitForSeconds(5f);
            GetComponent<Animator>().SetTrigger("open");
            //clip audio ?

            yield return new WaitForSeconds(5f);
            MainGameManager._soundManager.PlayEffect("monster_deny", 0.2f);
            yield return new WaitForSeconds(1f);
            MainGameManager._soundManager.PlayEffect("monster_deny", 0.2f);
            yield return new WaitForSeconds(1);
            GetComponent<Animator>().SetTrigger("close");
            //clip audio ?
        }
    }

    public void StartCycle() { rootParent.SetActive(false); }

    void DeployRoot() { rootParent.SetActive(true); }

    IEnumerator ThrowSpikesCo()
    {
        return ThrowSpikesSpiralCo();
    }

    IEnumerator ThrowSpikesSpiralCo()
    {
        Vector3 direction = DirectionHelper.GetDirection(transform.position, target.transform.position).normalized;

        const int ITERATION = 2;
        int[] anglesSalves = { -67, -45, -22, 0, 22, 45, 67, 90 };
        for (int i = 0; i < ITERATION; i++)
        {
            foreach (var angle in anglesSalves)
            {
                MainGameManager._soundManager.PlayEffect("dagger", 0.2f);
                ThrowProjectile(DirectionHelper.RotateVector3DirectionByAngle(direction, angle));

                yield return new WaitForSeconds(0.15f);
            }
        }
    }

    IEnumerator ThrowSpikesEventailCo()
    {
        Vector3 direction = DirectionHelper.GetDirection(transform.position, target.transform.position).normalized;

        MainGameManager._soundManager.PlayEffect("dagger", 0.2f);
        ThrowProjectile(direction);

        int[] anglesSalves = { 40, 80, 60, 20 };
        foreach (var angle in anglesSalves)
        {
            yield return new WaitForSeconds(0.2f);

            MainGameManager._soundManager.PlayEffect("dagger", 0.2f);
            ThrowProjectile(DirectionHelper.RotateVector3DirectionByAngle(direction, -angle));
            ThrowProjectile(DirectionHelper.RotateVector3DirectionByAngle(direction, angle));
        }

        MainGameManager._soundManager.PlayEffect("dagger", 0.2f);
        ThrowProjectile(direction);
    }

    void ThrowProjectile(Vector3 direction)
    {
        Vector3 instantiatePos = transform.position + direction;

        Projectile instanciateProjectile = _spikeProjectilePool.Get();
        instanciateProjectile.Init(instantiatePos, direction, _spikeProjectilePool);

        var spikeSpriteTransform = instanciateProjectile.GetComponentInChildren<SpriteRenderer>().transform;
        DirectionHelper.PointTo(spikeSpriteTransform, spikeSpriteTransform.position + direction, 180);
    }

    IEnumerator SpawnRandomEnemies()
    {
        var rings = GetComponentsInChildren<PolygonCollider2D>();
        foreach (PolygonCollider2D ring in rings)
        {
            ring.enabled = false;
        }

        GameObject enemyFirst = Instantiate(enemiesPrefabs[Random.Range(0, enemiesPrefabs.Length)], transform.position, Quaternion.identity);
        enemyFirst.GetComponent<Rigidbody2D>().velocity = GetRandomSeedThrowDirection();

        //GameObject enemySecond = Instantiate(enemiesPrefabs[Random.Range(0, enemiesPrefabs.Length)], transform.position, Quaternion.identity);
        //enemySecond.GetComponent<Rigidbody2D>().velocity = GetRandomSeedThrowDirection();

        yield return new WaitForSeconds(0.5f);

        foreach (PolygonCollider2D ring in rings)
        {
            ring.enabled = true;
        }
    }

    Vector3 GetRandomSeedThrowDirection()
    {
        Vector3 reference = transform.position; //8.22 1.49

        Vector3 newPos = Vector3.zero;
        newPos.x = Random.Range(reference.x - 1f, reference.x + 1f);
        newPos.y = Random.Range(reference.y, reference.y + 1f);

        Vector3 relativeDirection = DirectionHelper.GetDirection(transform.position, newPos).normalized;

        relativeDirection.x *= 7;
        relativeDirection.y *= 10;

        return relativeDirection;
    }

    public void ActiveLittleOne()
    {
        GameObject littleOne = transform.Find("LittleOne").gameObject;
        littleOne.SetActive(true);
        littleOne.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }

    public void InactiveLittleOne()
    {
        transform.Find("LittleOne").gameObject.SetActive(false);
    }

}