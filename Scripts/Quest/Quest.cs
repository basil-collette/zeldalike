using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Quest")]
public class Quest : ScriptableObject
{
    public string Name;
    public string Description;
    public bool IsCompleted;
    public List<QuestStep> QuestSteps;

    /*
    private void OnValidate()
    {
        foreach (QuestStep questStep in QuestSteps)
        {
            questStep.Rewards.OnAfterDeserialize();
        }
    }
    */

    /*
    public void AddQuestStep()
    {

        PlayerQuests = new List<Quest>(PlayerQuests);
    }
    */

    public void SetQuestSteps()
    {
        QuestSteps = new List<QuestStep>(QuestSteps);
    }

}

[System.Serializable]
public class TempQuest
{
    public string Name;
    public string Description;
    public bool IsCompleted;
    public List<QuestStep> QuestSteps;
} 