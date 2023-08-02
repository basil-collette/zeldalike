using UnityEngine;

public class HealthDrop : Drop
{
    public float amount;

    protected override void OnTriggerEnter2DIsPlayer(Collider2D collider)
    {
        collider.GetComponent<Health>().Heal(amount);
    }

}