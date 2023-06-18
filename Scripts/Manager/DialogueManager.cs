using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public PauseManager pauseManager;
    public GameObject dialogueButtonPrefab;
    public float textAnimPauseSeconds = 0.01f;

    private DialogueContainer _dialogueContainer;

    public void StartDialogue(DialogueContainer dialogueContainer)
    {
        _dialogueContainer = dialogueContainer;

        pauseManager.ShowPausedInterface("DialogScene", () =>
        {
            BaseNodeData node = GraphSaveUtility.GetFirstNode(_dialogueContainer);
            NextNode(node);
        });
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

        GameObject dialogueCanva = FindGameObjectHelper.FindInactiveObjectByName("Dialog Canva");

        FindGameObjectHelper.FindInactiveObjectByName("Dialogue Name").GetComponent<Text>().text = node.Pnj.Name + ":";
        FindGameObjectHelper.FindInactiveObjectByName("Interlocutor").GetComponent<Image>().sprite = node.Pnj.Sprite;

        var textComp = FindGameObjectHelper.FindInactiveObjectByName("Dialogue Text").GetComponent<Text>();

        Action showButtons = () =>
        {
            var connections = GraphSaveUtility.GetOutputs(_dialogueContainer, node);
            if (connections.Count > 0)
            {
                foreach (var conn in connections)
                {
                    InstanciateChoiceButton(
                        (conn.PortName == string.Empty) ? "continue" : conn.PortName,
                        () =>
                        {
                            BaseNodeData nextNode = GraphSaveUtility.GetNodeByGuid(_dialogueContainer, conn.TargetNodeGuid);
                            NextNode(nextNode);
                        }
                    );
                }
            }
            else
            {
                InstanciateChoiceButton("Ok", () =>
                {
                    pauseManager.Resume();
                });
            }
        };

        StartCoroutine(TextAnimationByLetter(textComp, node.DialogueText, showButtons));
    }

    void InstanciateChoiceButton(string portName, Action clickAction)
    {
        GameObject choiceButton = Instantiate(dialogueButtonPrefab, Vector3.zero, Quaternion.identity);
        Vector3 scale = choiceButton.transform.localScale;

        GameObject dialogueButtonsLayout = FindGameObjectHelper.FindInactiveObjectByName("Dialogue Buttons Layout");
        choiceButton.transform.SetParent(dialogueButtonsLayout.transform, true);
        choiceButton.transform.localScale = scale;

        choiceButton.GetComponentInChildren<Text>().text = portName;
        choiceButton.GetComponent<Button>().onClick.AddListener(() => { clickAction?.Invoke(); });
    }

    void ProcessEvent(EventNodeData node)
    {
        node.Event?.Invoke();

        var connections = GraphSaveUtility.GetOutputs(_dialogueContainer, node);
        if (connections.Count > 0)
        {
            BaseNodeData nextNode = GraphSaveUtility.GetNodeByGuid(_dialogueContainer, connections[0].TargetNodeGuid);
            NextNode(nextNode);
        }
        else
        {
            pauseManager.Resume();
        }
    }

    void ClearButtonsLayout()
    {
        GameObject dialogueButtonsLayout = FindGameObjectHelper.FindInactiveObjectByName("Dialogue Buttons Layout");
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