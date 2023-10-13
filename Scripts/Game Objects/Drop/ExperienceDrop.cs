using UnityEngine;

public class ExperienceDrop : Drop
{
    public float amount;

    protected override bool OnTriggerEnter2DIsPlayer(Collider2D collider)
    {
        Rewards reward = new Rewards()
        {
            Experience = amount
        };

        MainGameManager._inventoryManager.GetReward(reward);

        return true;
    }

}