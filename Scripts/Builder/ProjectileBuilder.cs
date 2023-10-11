using UnityEngine;

public class ProjectileBuilder : MonoBehaviour
{
    GameObject _projectilePrefab;
    float _speed;
    float _duration;

    /*
    void Test()
    {
        new ProjectileBuilder()
            .WithProjectilePrefab(projectile)
            .WithSpeed(speed)
            .Build(origin);
    }
    */

    public ProjectileBuilder WithProjectilePrefab(GameObject projectilePrefab)
    {
        _projectilePrefab = projectilePrefab;
        return this;
    }

    public ProjectileBuilder WithSpeed(float speed)
    {
        _speed = speed;
        return this;
    }

    public ProjectileBuilder WithDuration(float duration)
    {
        _duration = duration;
        return this;
    }

    public GameObject Build(Transform origin)
    {
        Vector3 instantiatePosition = origin.position + origin.forward * 2;

        GameObject projectile = Instantiate(_projectilePrefab, instantiatePosition.With(y: 1), Quaternion.identity);
        //ParticleMover mover = projectile.GetOrAddComponent<PartcileMover>();
        //SelfDestruct selfDestruct = projectile.GetOrAddComponent<SelfDestruct>();

        //mover.Initalize(_speed);
        //selfDestruct.Initialize(_duration);

        return projectile;
    }

}
