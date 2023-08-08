using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/PlayerQuest")]
public class PlayerQuest : ScriptableObject
{
    public static void AddQuest(string questName)
    {
        PlayerQuest pq = FindAnyObjectByType<Player>().playerQuest;
        pq.AddQuest(pq.FindQuest(questName));
    }

    public List<Quest> PlayerQuests = new List<Quest>();

    public void AddQuest(Quest quest)
    {
        PlayerQuests.Add(quest);
    }

    public Quest GetQuestByName(string name)
    {
        return PlayerQuests.FirstOrDefault(x => x.Name == name);
    }

    public List<Quest> GetQuestsByState(bool inProgress)
    {
        return PlayerQuests.AsEnumerable<Quest>()
            .Where(x => x.QuestSteps.LastOrDefault().IsCompleted != inProgress)
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

    public Quest FindQuest(string questName)
    {
        return Resources.Load<Quest>($"ScriptableObjects/quests/{questName}");
    }

}
