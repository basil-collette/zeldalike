using Assets.Scripts.Enums;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : SignletonGameObject<DialogueManager>
{
    public static event Action<string[]> OnDiscuss;

    public PauseManager pauseManager;
    public GameObject dialogueButtonPrefab;
    public float textAnimPauseSeconds = 0.01f;

    DialogueContainer _dialogueContainer;
    Pnj _pnj;

    public void StartDialogue(Pnj pnj, DialogueReference dialogueRef)
    {
        _pnj = pnj;
        _dialogueContainer = dialogueRef.DialogueContainer;

        pauseManager.ShowPausedInterface("DialogueScene", () =>
        {
            BaseNodeData node = GraphHelper.GetFirstNode(_dialogueContainer);
            NextNode(node);
        }, false, false);
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

        GameObject dialogueCanva = FindGameObjectHelper.FindByName("Dialog Canva");

        FindGameObjectHelper.FindByName("Dialogue Name").GetComponent<Text>().text = node.Pnj.Name + ":";
        FindGameObjectHelper.FindByName("Interlocutor").GetComponent<Image>().sprite = node.Pnj.Sprite;

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
                            _pnj.AddSaid(node.DialogueCode);
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
                    _pnj.AddSaid(node.DialogueCode);
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
        switch (node.Type)
        {
            case EventTypeEnum.StartQuest: PlayerQuest.AddQuest(node.Param); break;
            case EventTypeEnum.AddItem: Inventory.AddItem(node.Param); break;
            case EventTypeEnum.RemoveItem: Inventory.StaticRemoveItem(node.Param); break;
            case EventTypeEnum.SetDialogueSaid: break;
            case EventTypeEnum.StartCinematic: break;
            default: break;
        }

        var connections = GraphHelper.GetOutputs(_dialogueContainer, node);
        if (connections.Count > 0)
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