using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Quest")]
public class ScriptableQuest : ScriptableObject
{
    public string Name;
    public string Code;
    public string Description;
    public List<QuestStep> QuestSteps;
    public bool IsCompleted;
}

[System.Serializable]
public class Quest
{
    public string Name;
    public string Code;
    public string Description;
    public bool IsCompleted;
    public List<QuestStep> QuestSteps;

    public Quest(ScriptableQuest quest)
    {
        Name = quest.Name;
        Code = quest.Code;
        Description = quest.Description;
        IsCompleted = false;
        QuestSteps = quest.QuestSteps;

        Init();
    }

    public void Init()
    {
        foreach (QuestStep questStep in QuestSteps)
        {
            questStep._Quest = this;

            foreach (Goal goal in questStep.Goals)
            {
                goal._QuestStep = questStep;
            }
        }
    }

    public bool VerifyIfCompleted()
    {
        if (QuestSteps.Exists(x => !x.IsCompleted))
        {
            IsCompleted = false;
        }
        else
        {
            IsCompleted = true;
        }

        return IsCompleted;
    }

} 