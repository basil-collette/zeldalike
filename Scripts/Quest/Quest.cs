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

}

[System.Serializable]
public class TempQuest
{
    public string Name;
    public string Description;
    public bool IsCompleted;
    public List<QuestStep> QuestSteps;
} 