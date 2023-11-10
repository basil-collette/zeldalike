using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>, ISavable
{
    public List<Quest> _quests = new List<Quest>();

    public void AddQuest(string questName)
    {
        if (!_quests.Any(x => x.Name == questName))
        {
            Quest quest = new Quest(Resources.Load<ScriptableQuest>($"ScriptableObjects/quests/{questName}"));
            ConcreteAddQuest(quest);
        }
    }

    public void AddQuest(Quest quest)
    {
        if (!_quests.Any(x => x == quest))
        {
            ConcreteAddQuest(quest);
        }
    }

    void ConcreteAddQuest(Quest quest)
    {
        /*
        quest.QuestSteps.ForEach(step => {
            step.Goals.ForEach(goal => goal.GoalCheck(new string[] { }));
        });
        */

        _quests.Add(quest);
    }

    public Quest GetQuestByName(string name)
    {
        return _quests.FirstOrDefault(x => x.Name == name);
    }

    public Quest GetQuestByCode(string code)
    {
        return _quests.FirstOrDefault(x => x.Code == code);
    }

    public List<Quest> GetQuestsByState(bool completed)
    {
        return _quests.AsEnumerable<Quest>()
            .Where(x => x.IsCompleted == completed)
            .ToList();
    }

    public List<QuestStep> GetCompletedQuestSteps(Quest quest)
    {
        return quest.QuestSteps.Where(x => x.IsCompleted).ToList();
    }

    public QuestStep GetCurrentStep(Quest quest)
    {
        return quest.QuestSteps.FirstOrDefault(x => !x.IsCompleted);
    }

    public Quest GetQuest(string questName)
    {
        return _quests.Find(x => x.Name == questName);
    }

    public string ToJsonString()
    {
        return JsonUtility.ToJson(Get());
    }

    public void Load(string json)
    {
        Set(JsonUtility.FromJson<QuestbookSaveModel>(json));
    }

    public QuestbookSaveModel Get()
    {
        return new QuestbookSaveModel
        {
            Quests = _quests
        };
    }

    public void Set(QuestbookSaveModel saveModel)
    {
        _quests = saveModel.Quests;

        foreach (Quest quest in _quests)
        {
            quest.Init();
        }
    }

}

[Serializable]
public class QuestbookSaveModel
{
    public List<Quest> Quests = new List<Quest>();
}