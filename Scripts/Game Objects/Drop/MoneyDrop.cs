using UnityEngine;

public class MoneyDrop : Drop
{
    public int amount;

    protected override void OnTriggerEnter2DIsPlayer(Collider2D collider)
    {
        Rewards reward = new Rewards()
        {
            Money = amount
        };

        MainGameManager._inventoryManager.GetReward(reward);
    }

}