using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/PlayerQuest")]
public class PlayerQuest : ScriptableObject
{
    public List<Quest> PlayerQuests;

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
            .Where(x => x.Goals.LastOrDefault().IsCompleted != inProgress)
            .ToList();
    }

    public List<Goal> GetCompletedGoals(Quest quest)
    {
        return quest.Goals.Where(x => x.IsCompleted).ToList();
    }

    public Goal GetCurrentGoal(Quest quest)
    {
        return quest.Goals.FirstOrDefault<Goal>(x => !x.IsCompleted);
    }

}
