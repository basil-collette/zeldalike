using UnityEngine;

[CreateAssetMenu(fileName = "DN", menuName = "ScriptableObject/Dialogue/Dialogue Node")]
public class DialogueNode : ScriptableObject
{
    public string dialogueText;
    public DialogueChoice[] choices;
    public DialogueNode[] nextNodes;
}

[CreateAssetMenu(fileName = "DC", menuName = "ScriptableObject/Dialogue/Dialogue Choice")]
public class DialogueChoice : ScriptableObject
{
    public string choiceText;
    public DialogueNode nextNode;
}

public class DialogueManager : MonoBehaviour
{
    public DialogueNode startingNode;
    private DialogueNode currentNode;

    private void Start()
    {
        StartDialogue();
    }

    private void StartDialogue()
    {
        currentNode = startingNode;
        DisplayDialogue(currentNode);
    }

    private void DisplayDialogue(DialogueNode node)
    {
        Debug.Log(node.dialogueText);

        if (node.choices.Length > 0)
        {
            for (int i = 0; i < node.choices.Length; i++)
            {
                DialogueChoice choice = node.choices[i];
                Debug.Log($"{i + 1}. {choice.choiceText}");
            }
            // Attente de l'entrée du joueur pour choisir une réponse
        }
        else
        {
            // Le dialogue est terminé, faire quelque chose (par exemple, fermer la fenêtre de dialogue)
        }
    }

    private void MakeChoice(int choiceIndex)
    {
        if (currentNode.choices.Length > choiceIndex)
        {
            DialogueChoice chosenChoice = currentNode.choices[choiceIndex];
            currentNode = chosenChoice.nextNode;
            DisplayDialogue(currentNode);
        }
    }
}
