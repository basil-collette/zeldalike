using Assets.Scripts.Enums;

public class DialogueNode : BaseNode
{
    public DialogueTypeEnum Type = DialogueTypeEnum.talk;
    public string DialogueText;
    public string DialogueCode;
    public DialogueNodeSide Side = DialogueNodeSide.left;
    public Pnj Pnj;
}
