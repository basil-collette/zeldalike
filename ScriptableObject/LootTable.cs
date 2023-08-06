using Assets.Scripts.Enums;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Loot
{
    public string lootCodeValue;
    public DropTypeEnum type;
    public float lootWeight;
}

[CreateAssetMenu(menuName = "ScriptableObject/LootTable")]
public class LootTable : ScriptableObject, ISerializationCallbackReceiver
{
    public float TotalDropWeight => loots.Sum(x => x.lootWeight);
    public float TotalLevelWeight => dropAmountLevels.values.Sum(x => x);

    public Loot[] loots;
    public SerializableDic<int, float> dropAmountLevels = new SerializableDic<int, float>();

    public List<Loot> GetLootsByChanceWheel()
    {
        float randomDropAmount = Random.Range(0, TotalLevelWeight);

        float traveledWeight = 0;
        int traveledIndex = -1;
        while (traveledWeight < randomDropAmount)
        {
            traveledIndex++;
            traveledWeight += dropAmountLevels.values.ElementAt(traveledIndex);
        }

        int dropAmount = dropAmountLevels.keys.ElementAt(traveledIndex);

        List<Loot> lootsResult = new List<Loot>();

        List<Loot> tempLoots = loots.ToList();
        for (int i = 0; i < dropAmount; i++)
        {
            float random = Random.Range(0, tempLoots.Sum(x => x.lootWeight));

            float traveledWeightX = 0;
            int traveledIndexX = -1;
            while (traveledWeightX < random)
            {
                traveledIndexX++;
                traveledWeightX += tempLoots.ElementAt(traveledIndexX).lootWeight;
            }

            lootsResult.Add(tempLoots.ElementAt(traveledIndexX));
            tempLoots.Remove(tempLoots.ElementAt(traveledIndexX));
        }

        return lootsResult;
    }

    public float GetPercentileChanceLoot(Loot loot)
    {
        return loot.lootWeight / TotalDropWeight * 100;
    }

    public void OnAfterDeserialize()
    {
        if (dropAmountLevels.keys.Count == 0)
            dropAmountLevels.Add(1, 1);
    }

    public void OnBeforeSerialize()
    {

    }

}