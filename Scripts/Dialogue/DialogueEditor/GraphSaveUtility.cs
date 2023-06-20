using System.Collections.Generic;
using System.Linq;

public static class GraphSaveUtility
{
    public static BaseNodeData GetFirstNode(DialogueContainer dialogueContainer)
    {
        var linkFromEntry = dialogueContainer.NodeLinks[0];
        return dialogueContainer.NodeDatas.FirstOrDefault(x => x.Guid == linkFromEntry.TargetNodeGuid);
    }

    public static List<NodeLinkData> GetOutputs(DialogueContainer dialogueContainer, BaseNodeData node)
    {
        return dialogueContainer.NodeLinks.Where(x => x.BaseNodeGuid == node.Guid).ToList();
    }

    public static List<NodeLinkData> GetNodeLinksByGuid(DialogueContainer dialogueContainer, string nodeGuid)
    {
        return dialogueContainer.NodeLinks.Where(x => x.BaseNodeGuid == nodeGuid).ToList();
    }

    public static BaseNodeData GetNodeByGuid(DialogueContainer dialogueContainer, string targetNodeGuid)
    {
        if (targetNodeGuid == null)
            return null;

        return dialogueContainer.NodeDatas.FirstOrDefault(x => x.Guid == targetNodeGuid);
    }

}
