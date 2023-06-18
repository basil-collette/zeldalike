using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public PauseManager pauseManager;

    private Image InterlocutorSprite;
    private Text DialogueBoxtext;

    public void StartDialogue(DialogueContainer dialogueContainer)
    {
        pauseManager.ShowPausedInterface("DialogScene", () =>
        {
            BaseNodeData node = GraphSaveUtility.GetFirstNode(dialogueContainer);

            if (node is DialogueNodeData)
            {
                var connections = GraphSaveUtility.GetOutputs(dialogueContainer, node as DialogueNodeData);
                DisplayDialogue(node as DialogueNodeData, connections);
            }
            else if (node is EventNodeData)
            {
                ProcessEvent(node as EventNodeData);
            }
            else
            {
                //Error
            }
        });
    }

    void DisplayDialogue(DialogueNodeData node, List<NodeLinkData> connections)
    {
        GameObject dialogueCanva = FindGameObjectHelper.FindInactiveObjectByName("Dialog Canva");

        InterlocutorSprite = dialogueCanva.GetComponentInChildren<Image>();
        DialogueBoxtext = dialogueCanva.GetComponentInChildren<Text>();

        DialogueBoxtext.text = node.DialogueText;
        InterlocutorSprite.sprite = node.Pnj.Sprite;
        
        if (connections.Count > 0)
        {
            // show all buttons
        }
        else
        {
            //show ok button
        }
    }

    void ProcessEvent(EventNodeData node)
    {
        node.Event.Invoke();

        //Show next dialogue if exists
    }

}