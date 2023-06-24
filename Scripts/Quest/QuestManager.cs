using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public List<Quest> PlayerQuests;

    public GameObject QuestInListPrefab;
    public RectTransform ListTransform;

    public RectTransform QuestDescContainer;
    public Text QuestNameText;
    public Text QuestDescText;
    public RectTransform QuestRewardsContainer;
    public Text QuestRewardText;

    public GameObject QuestLogObject;
    public Button[] QuestButtons;

    //Quest CurrentQuest;
    int PreviousButtonIndex;

    private void Start()
    {
        PreviousButtonIndex = 0;
        ShowQuestsByState(true);
    }

    public void SetShowQuestLog(bool show)
    {
        QuestLogObject.SetActive(show);
    }

    public void AddQuest(Quest quest)
    {
        PlayerQuests.Add(quest);
        ShowQuestsByState(true);
    }

    public void ShowQuestsByState(bool inProgress)
    {
        List<Quest> quests = PlayerQuests.AsEnumerable<Quest>()
            .Where(x => x.Goals.LastOrDefault().IsCompleted != inProgress)
            .ToList();

        SetQuestListContainer(quests);
    }

    void SetQuestListContainer(List<Quest> quests)
    {
        ListTransform.sizeDelta = new Vector2(0, quests.Count * 80);
        Array.Resize(ref QuestButtons, quests.Count);
        for (int i = 0; i < quests.Count; i++)
        {
            QuestButtons[i] = InitializeButton(quests[i], i);
            UpdateQuestText(QuestButtons[i], quests[i]);
            //
        }
    }

    void ShowQuestDesc(int index, Button button, Quest quest)
    {
        SetQuestSelectionColor(QuestButtons[PreviousButtonIndex], false);
        SetQuestSelectionColor(button, true);
        PreviousButtonIndex = index;

        //CurrentQuest = quest;

        QuestDescContainer.gameObject.SetActive(quest != null);
        if (quest == null) return;

        QuestNameText.text = quest.Name;
        //QuestDescText.text = "";
        //QuestRewardsContainer
        QuestDescText.rectTransform.sizeDelta = new Vector2(0, QuestDescText.preferredHeight);
        //
    }

    void SetQuestSelectionColor(Button button, bool isSelected)
    {
        button.image.color = (isSelected) ? Color.green : new Color(0, 0, 0, 0);
    }

    Button InitializeButton(Quest quest, int index)
    {
        Button button = Instantiate(QuestInListPrefab, ListTransform).GetComponent<Button>();
        button.image.rectTransform.sizeDelta = new Vector2(0, 80);
        button.image.rectTransform.anchoredPosition = new Vector2(0, -80 * index);
        button.onClick.AddListener(delegate { ShowQuestDesc(index, button, quest); });
        return button;
    }

    void UpdateQuestText(Button button, Quest quest)
    {
        Text text = button.GetComponentInChildren<Text>();
        text.text = quest.Name;
    }

}
