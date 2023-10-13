using UnityEngine;

public class HealthDrop : Drop
{
    public float amount;

    protected override bool OnTriggerEnter2DIsPlayer(Collider2D collider)
    {
        collider.GetComponent<Health>().Heal(amount);
        return true;
    }

}