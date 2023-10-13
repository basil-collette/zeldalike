using UnityEngine;

public class MoneyDrop : Drop
{
    public float amount;

    protected override bool OnTriggerEnter2DIsPlayer(Collider2D collider)
    {
        Rewards reward = new Rewards()
        {
            Money = amount
        };

        MainGameManager._inventoryManager.GetReward(reward);
        return true;
    }

}