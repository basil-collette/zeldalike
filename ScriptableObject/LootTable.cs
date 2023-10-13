using Assets.Scripts.Enums;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/LootTable")]
public class LootTable : ScriptableObject
{
    public float TotalDropWeight => _loots.Sum(x => x.LootWeight);
    public float TotalAmountOfDropWeight => _lootAmountChances.Sum(x => x.Chance);

    [SerializeReference] public List<Loot> _loots;
    public List<LootAmount> _lootAmountChances;

    public List<Loot> GetLootsByChanceWheel()
    {
        float randomDropAmount = Random.Range(0, TotalAmountOfDropWeight);

        float traveledWeight = 0;
        int traveledIndex = -1;
        while (traveledWeight < randomDropAmount)
        {
            traveledIndex++;
            traveledWeight += _lootAmountChances[traveledIndex].Chance;
        }

        int dropAmount = _lootAmountChances[traveledIndex].AmountOfDrops;

        List<Loot> lootsResult = new List<Loot>();

        List<Loot> tempLoots = _loots.ToList();
        for (int i = 0; i < dropAmount; i++)
        {
            float random = Random.Range(0, tempLoots.Sum(x => x.LootWeight));

            float traveledWeightX = 0;
            int traveledIndexX = -1;
            while (traveledWeightX < random)
            {
                traveledIndexX++;
                traveledWeightX += tempLoots.ElementAt(traveledIndexX).LootWeight;
            }

            lootsResult.Add(tempLoots.ElementAt(traveledIndexX));
            tempLoots.Remove(tempLoots.ElementAt(traveledIndexX));
        }

        return lootsResult;
    }

    public float GetPercentileChanceLoot(Loot loot)
    {
        return loot.LootWeight / TotalDropWeight * 100;
    }

}

[System.Serializable]
public abstract class Loot
{
    public float LootWeight;
}

[System.Serializable]
public class DefaultLoot : Loot
{
    public float Amount;
    public DropTypeEnum Type;
}

[System.Serializable]
public class ItemLoot : Loot
{
    public string NameCode;
    public ItemTypeEnum Type;
}

[System.Serializable]
public class LootAmount
{
    [Range(0, 10)] public int AmountOfDrops;
    public float Chance;
}