using Assets.Scripts.Manager;
using System;
using System.Collections;
using UnityEngine;

public class RoseMotherThinker : Bot
{
    public float openDuration;

    public GameObject rootParent;
    public Projectile spikePrefab;

    SoundManager soundManager;

    new void Start()
    {
        soundManager = FindGameObjectHelper.FindByName("Main Sound Manager").GetComponent<SoundManager>();
        soundManager.PlayEffect("monster_scream", 0.5f);

        base.Start();

        StartCoroutine(CycleCo());

        Health.OnDeath += RoseDeath;
    }

    private void OnDestroy()
    {
        Health.OnDeath -= RoseDeath;
    }

    void RoseDeath(string[] parameters)
    {
        if (Array.Exists(parameters, x => x == "rose_mother"))
        {
            GameObject.Find("Wall").GetComponent<Animator>().SetTrigger("open");

            GameObject.Find("Scene Transition").GetComponent<BoxCollider2D>().enabled = true;

            soundManager.musicSource.mute = true;
            //play victory music ?

            StopAllCoroutines();

            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
    }

    IEnumerator CycleCo()
    {
        yield return new WaitForSeconds(10f);

        GetComponent<Animator>().SetTrigger("open");
        //clip audio ?

        yield return new WaitForSeconds(10f);

        soundManager.PlayEffect("monster_deny", 0.2f);
        yield return new WaitForSeconds(1f);
        soundManager.PlayEffect("monster_deny", 0.2f);
        yield return new WaitForSeconds(1);

        GetComponent<Animator>().SetTrigger("close");
        //clip audio ?

        yield return new WaitForSeconds(5f);

        soundManager.PlayEffect("monster_movement");
        DeployRoot();

        yield return new WaitForSeconds(5f);

        soundManager.PlayEffect("monster_scream", 0.5f);

        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(ThrowSpikesCo());

        yield return new WaitForSeconds(5f);
    }

    public void StartCycle()
    {
        rootParent.SetActive(false);
        StartCoroutine(CycleCo());
    }

    void DeployRoot()
    {
        rootParent.SetActive(true);
    }

    IEnumerator ThrowSpikesCo()
    {
        Vector3 direction = DirectionHelper.GetDirection(transform.position, target.transform.position).normalized;

        soundManager.PlayEffect("dagger", 0.2f);
        ThrowProjectile(direction);

        int[] anglesSalves = { 40, 80, 60, 20 };
        foreach (var angle in anglesSalves)
        {
            yield return new WaitForSeconds(0.2f);

            soundManager.PlayEffect("dagger", 0.2f);
            ThrowProjectile(DirectionHelper.RotateVector3DirectionByAngle(direction, -angle));
            ThrowProjectile(DirectionHelper.RotateVector3DirectionByAngle(direction, angle));
        }

        soundManager.PlayEffect("dagger", 0.2f);
        ThrowProjectile(direction);
    }

    void ThrowProjectile(Vector3 direction)
    {
        Vector3 instantiatePos = transform.position + direction;

        Projectile instanciateProjectile = Instantiate(spikePrefab, instantiatePos, Quaternion.identity);
        instanciateProjectile.direction = direction;

        var spikeSpriteTransform = instanciateProjectile.GetComponentInChildren<SpriteRenderer>().transform;
        DirectionHelper.PointTo(spikeSpriteTransform, spikeSpriteTransform.position + direction, 180);
    }

    public void ActiveLittleOne()
    {
        transform.Find("LittleOne").gameObject.SetActive(true);
    }

    public void InactiveLittleOne()
    {
        transform.Find("LittleOne").gameObject.SetActive(false);
    }

}