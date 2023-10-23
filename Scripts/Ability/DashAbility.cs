using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Ability/Dash")]
public class DashAbility : Ability
{
    public float _dashVelocity;

    public override void Activate(GameObject parent)
    {
        AliveEntity entity = parent.GetComponent<AliveEntity>();
        Rigidbody2D rb = parent.GetComponent<Rigidbody2D>();

        rb.velocity = entity.direction.normalized * _dashVelocity;
    }

    public override void AfterActivate(GameObject parent)
    {
        //
    }

}