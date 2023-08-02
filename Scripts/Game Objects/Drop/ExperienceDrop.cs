using UnityEngine;

public class ExperienceDrop : Drop
{
    public int amount;

    protected override void OnTriggerEnter2DIsPlayer(Collider2D collider)
    {
        Rewards reward = new Rewards()
        {
            Experience = amount
        };

        collider.GetComponent<Player>().inventory.GetReward(reward);
    }

}