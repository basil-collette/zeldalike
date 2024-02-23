using Assets.Database.Model.Design;
using Assets.Database.Model.Repository;
using System;
using System.Collections;
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
    [SerializeField] Button InProgressButton;
    [SerializeField] Button CompletedButton;

    public GameObject RewardXP;
    public GameObject RewardGold;
    public List<GameObject> Items;

    Button[] QuestButtons;
    int PreviousButtonIndex;
    QuestManager _questbook;

    private void OnEnable()
    {
        PreviousButtonIndex = 0;
        _questbook = MainGameManager._questManager;
        ShowQuestsByState(false);
    }

    public void ShowQuestsByState(bool completed)
    {
        ClearQuestDesc();

        List<Quest> quests = MainGameManager._questManager.GetQuestsByState(completed);

        InProgressButton.interactable = completed;
        CompletedButton.interactable = !completed;

        SetQuestListContainer(quests);

        if (QuestButtons.Length > 0)
        {
            QuestButtons[0].onClick.Invoke();
        }
    }

    void SetQuestListContainer(List<Quest> quests)
    {
        foreach (Transform child in ListTransform)
        {
            Destroy(child.gameObject);
        }

        ListTransform.sizeDelta = new Vector2(0, quests.Count * 80);
        Array.Resize(ref QuestButtons, quests.Count);
        for (int i = 0; i < quests.Count; i++)
        {
            QuestButtons[i] = InitializeButton(quests[i], i);
            UpdateQuestText(QuestButtons[i], quests[i]);
        }
    }

    private void ShowRewards(QuestStep questStep)
    {
        bool haveRewardXP = questStep.Rewards.Experience != null && questStep.Rewards.Experience != 0;
        RewardXP.SetActive(haveRewardXP);
        if (haveRewardXP)
        {
            RewardXP.transform.Find("Text XPValue").GetComponent<Text>().text = questStep.Rewards.Experience.ToString();
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
                Item item = new Item(Singleton<ItemRepository<ItemScriptable, Item>>.Instance.GetByCode(questStep.Rewards.ItemsRef[i].ItemCode));
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

    void ShowQuestDesc(int index, Button button, Quest quest)
    {
        SetQuestSelectionColor(QuestButtons[PreviousButtonIndex], false);
        SetQuestSelectionColor(button, true);
        PreviousButtonIndex = index;

        QuestDetailsText.text = quest.Description;

        QuestStep questStep = _questbook.GetCurrentStep(quest);

        if (questStep == null)
            return;

        QuestObjectiveText.text = string.Join("\n", questStep.Goals.AsEnumerable<Goal>().Select(x => x.Objective));

        ShowRewards(questStep);
    }

    void ClearQuestDesc()
    {
        QuestDetailsText.text = string.Empty;
        QuestObjectiveText.text = string.Empty;
        RewardXP.SetActive(false);
        RewardGold.SetActive(false);

        for (int i = 0; i < 3; i++)
        {
            Items[i].SetActive(false);
        }
    }

    void SetQuestSelectionColor(Button button, bool isSelected)
    {
        button.GetComponent<Image>().color = (isSelected) ? new Color(1, 1, 1, 0.27f) : new Color(0, 0, 0, 0);
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
