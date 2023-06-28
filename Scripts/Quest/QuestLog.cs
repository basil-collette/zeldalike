using Assets.Database.Model.Design;
using Assets.Database.Model.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class QuestLog : MonoBehaviour
{
    public GameObject SelectQuestButtonPrefab;
    public RectTransform ListTransform;
    public Text QuestDetailsText;
    public Text QuestObjectiveText;
    public Button[] QuestButtons;

    public GameObject RewardXP;
    public GameObject RewardGold;
    public List<GameObject> Items;

    int PreviousButtonIndex;
    PlayerQuest PlayerQuest;

    private void OnEnable()
    {
        PreviousButtonIndex = 0;
        PlayerQuest = FindAnyObjectByType<Player>().playerQuest; //PlayerQuest = FindGameObjectHelper.FindByName("Player").GetComponent<Player>().playerQuest;
        ShowQuestsByState(true);

        QuestButtons[0].onClick.Invoke();
    }

    public void ShowQuestsByState(bool inProgress)
    {
        List<Quest> quests = PlayerQuest.GetQuestsByState(inProgress);

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

        QuestStep questStep = PlayerQuest.GetCurrentStep(quest);

        QuestDetailsText.text = quest.Description;
        QuestObjectiveText.text = string.Join("\n", questStep.Goals.AsEnumerable<Goal>().Select(x => x.Objective));

        bool haveRewardXP = questStep.Rewards.Xp != null && questStep.Rewards.Xp != 0;
        RewardXP.SetActive(haveRewardXP);
        if (haveRewardXP)
        {
            RewardXP.transform.Find("Text XPValue").GetComponent<Text>().text = questStep.Rewards.Xp.ToString();
        }

        bool haveRewardGold = questStep.Rewards.Money != null && questStep.Rewards.Money != 0;
        RewardGold.SetActive(haveRewardGold);
        if (haveRewardGold)
        {
            RewardGold.transform.Find("Text GoldValue").GetComponent<Text>().text = questStep.Rewards.Money.ToString();
        }
        
        for (int i = 0; i < 3; i++)
        {
            bool haveItemReward = questStep.Rewards.ItemsRef.Count() >= i + 1;
            Items[i].SetActive(haveItemReward);

            if (haveItemReward)
            {
                Item item = ItemRepository.Current.GetByCode(questStep.Rewards.ItemsRef[i].ItemName);
                Items[i].transform.Find("Item Image").GetComponent<Image>().sprite = item.Sprite;
                int amount = questStep.Rewards.ItemsRef[i].Amount;
                Transform amountShadow = Items[i].transform.Find("Amount Shadow");
                amountShadow.gameObject.SetActive(amount > 1);
                amountShadow.GetComponent<Text>().text = "x" + amount;
                Transform amountValue = Items[i].transform.Find("Amount");
                amountValue.gameObject.SetActive(amount > 1);
                amountValue.GetComponent<Text>().text = "x" + amount;
            }
        }
    }

    void SetQuestSelectionColor(Button button, bool isSelected)
    {
        //button.image.color = (isSelected) ? Color.green : new Color(0, 0, 0, 0);
        button.transform.Find("Selected").gameObject.SetActive(isSelected);
    }

    Button InitializeButton(Quest quest, int index)
    {
        Button button = Instantiate(SelectQuestButtonPrefab, ListTransform).GetComponent<Button>();
        button.GetComponentInChildren<Text>().text = quest.Name;
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
