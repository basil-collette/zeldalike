using Assets.Scripts.Enums;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : SingletonGameObject<DialogueManager>
{
    public static event Action<string[]> OnDiscuss;

    public PauseManager pauseManager;
    public GameObject dialogueButtonPrefab;
    public float textAnimPauseSeconds = 0.01f;
    [SerializeField] Sprite thinkingSprite;
    [SerializeField] Sprite outLoudSprite;

    DialogueContainer _dialogueContainer;

    public void StartDialogue(DialogueContainer dialogueContainer)
    {
        _dialogueContainer = dialogueContainer;

        ShowPauseInterface();
    }

    public void StartDialogue(DialogueReference dialogueRef)
    {
        _dialogueContainer = dialogueRef.DialogueContainer;

        ShowPauseInterface();
    }

    void ShowPauseInterface()
    {
        pauseManager.ShowPausedInterface(new PauseParameter()
        {
            InterfaceName = "DialogueScene",
            OnPauseProcessed = () =>
            {
                BaseNodeData node = GraphHelper.GetFirstNode(_dialogueContainer);
                NextNode(node);
            },
            PlaySound = false
        }) ;
    }

    void NextNode(BaseNodeData node)
    {
        if (node is DialogueNodeData)
        {
            DisplayDialogue(node as DialogueNodeData);
        }
        else if (node is EventNodeData)
        {
            ProcessEvent(node as EventNodeData);
        }
        else
        {
            //Error
        }
    }

    void DisplayDialogue(DialogueNodeData node)
    {
        ClearButtonsLayout();

        GameObject dialogueBox = FindGameObjectHelper.FindByName("Dialog Box");
        dialogueBox.GetComponent<Image>().sprite = (node.Type == DialogueTypeEnum.talk) ? outLoudSprite : thinkingSprite;
        if (node.Type == DialogueTypeEnum.think) dialogueBox.GetComponent<Image>().type = Image.Type.Tiled;

        GameObject pnjNameLeft = FindGameObjectHelper.FindByName("PNJ Name Left");
        pnjNameLeft.SetActive(node.Side == DialogueNodeSide.left);
        pnjNameLeft.GetComponent<Text>().text = node.Pnj.Name + ":";

        GameObject pnjSpriteLeft = FindGameObjectHelper.FindByName("PNJ Sprite Left");
        pnjSpriteLeft.SetActive(node.Side == DialogueNodeSide.left && node.Type != DialogueTypeEnum.think);
        pnjSpriteLeft.GetComponent<Image>().sprite = node.Pnj.Sprite;

        GameObject pnjNameRight = FindGameObjectHelper.FindByName("PNJ Name Right");
        pnjNameRight.SetActive(node.Side == DialogueNodeSide.right && node.Type != DialogueTypeEnum.think);
        pnjNameRight.GetComponent<Text>().text = node.Pnj.Name + ":";

        GameObject pnjSpriteRight = FindGameObjectHelper.FindByName("PNJ Sprite Right");
        pnjSpriteRight.SetActive(node.Side == DialogueNodeSide.right);
        pnjSpriteRight.GetComponent<Image>().sprite = node.Pnj.Sprite;

        var textComp = FindGameObjectHelper.FindByName("Dialogue Text").GetComponent<Text>();

        OnDiscuss?.Invoke(new string[] { node.DialogueCode });

        Action showButtons = () =>
        {
            var connections = GraphHelper.GetOutputs(_dialogueContainer, node);
            if (connections.Count > 0)
            {
                foreach (var conn in connections)
                {
                    InstanciateChoiceButton(
                        (conn.PortName == string.Empty) ? "continue" : conn.PortName,
                        () =>
                        {
                            node.Pnj.AddSaid(node.DialogueCode);
                            BaseNodeData nextNode = GraphHelper.GetNodeByGuid(_dialogueContainer, conn.TargetNodeGuid);
                            NextNode(nextNode);
                        }
                    );
                }
            }
            else
            {
                InstanciateChoiceButton("Ok", () =>
                {
                    node.Pnj.AddSaid(node.DialogueCode);
                    pauseManager.Resume();
                });
            }
        };

        StartCoroutine(TextAnimationByLetter(textComp, node.DialogueText, showButtons));
    }

    void InstanciateChoiceButton(string portName, Action clickAction)
    {
        //tester Instantiate(prefab, parent);
        GameObject choiceButton = Instantiate(dialogueButtonPrefab, Vector3.zero, Quaternion.identity);
        Vector3 scale = choiceButton.transform.localScale;

        GameObject dialogueButtonsLayout = FindGameObjectHelper.FindByName("Dialogue Buttons Layout");
        choiceButton.transform.SetParent(dialogueButtonsLayout.transform, true);
        choiceButton.transform.localScale = scale;

        choiceButton.GetComponentInChildren<Text>().text = portName;
        choiceButton.GetComponent<Button>().onClick.AddListener(() => { clickAction?.Invoke(); });
    }

    void ProcessEvent(EventNodeData node)
    {
        bool error = false;

        switch (node.Type)
        {
            case EventTypeEnum.StartQuest: MainGameManager._questbookManager.AddQuest(node.Param); break;
            case EventTypeEnum.AddItem: if (!MainGameManager._inventoryManager.AddItem(node.Param)) error = true; break;
            case EventTypeEnum.RemoveItem: MainGameManager._inventoryManager.RemoveItem(node.Param); break;
            case EventTypeEnum.AddMoney: MainGameManager._inventoryManager.AddMoney(int.Parse(node.Param)); break;
            case EventTypeEnum.RemoveMoney: MainGameManager._inventoryManager.RemoveMoney(int.Parse(node.Param)); break;
            //case EventTypeEnum.SetDialogueSaid: break;
            case EventTypeEnum.StartCinematic: break;
            default: break;
        }

        var connections = GraphHelper.GetOutputs(_dialogueContainer, node);
        if (connections.Count > 0 && !error)
        {
            BaseNodeData nextNode = GraphHelper.GetNodeByGuid(_dialogueContainer, connections[0].TargetNodeGuid);
            NextNode(nextNode);
        }
        else
        {
            pauseManager.Resume();
        }
    }

    void ClearButtonsLayout()
    {
        GameObject dialogueButtonsLayout = FindGameObjectHelper.FindByName("Dialogue Buttons Layout");
        foreach (Transform child in dialogueButtonsLayout.transform)
        {
            Destroy(child.gameObject);
        }
    }

    IEnumerator TextAnimationByLetter(Text textComp, string textValue, Action OnEndAnimation)
    {
        textComp.text = "";

        foreach (char letter in textValue.ToCharArray())
        {
            textComp.text += letter;

            //if (typeSound1 && typeSound2)
                //SoundManager.instance.RandomizeSfx(typeSound1, typeSound2);

            yield return new WaitForSecondsRealtime(textAnimPauseSeconds);
        }

        OnEndAnimation?.Invoke();
    }

}