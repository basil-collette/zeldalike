using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class GraphSaveUtility
{
    private DialogueGraphView _targetGraphView;
    private DialogueContainer _containerCache;

    private List<Edge> Edges => _targetGraphView.edges.ToList();
    private List<BaseNode> Nodes => _targetGraphView.nodes.ToList().Cast<BaseNode>().ToList();

    public static GraphSaveUtility GetInstance(DialogueGraphView targetGraphView)
    {
        return new GraphSaveUtility
        {
            _targetGraphView = targetGraphView
        };
    }

    public void SaveGraph(string fileName)
    {
        var entryPoint = Edges.Find(x => x.output.node.title == "Start");
        if (entryPoint == null)
        {
            EditorUtility.DisplayDialog("Error", "Start Node must be connected before saving.", "OK");
            return;
        }

        DialogueContainer dialogueContainer = ScriptableObject.CreateInstance<DialogueContainer>();

        if (!SaveNodes(dialogueContainer))
        {
            EditorUtility.DisplayDialog("Error", "Saving Node process failed.", "OK");
            return;
        }

        if (!SaveExposedProperties(dialogueContainer))
        {
            EditorUtility.DisplayDialog("Error", "Saving Exposed-Properties process failed.", "OK");
            return;
        }

        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            AssetDatabase.CreateFolder("Assets", "Resources");

        if (!AssetDatabase.IsValidFolder("Assets/Resources/Dialogue"))
            AssetDatabase.CreateFolder("Assets/Resources", "Dialogue");

        AssetDatabase.CreateAsset(dialogueContainer, $"Assets/Resources/Dialogue/{fileName}.asset"); // //ScriptableObject/Objects/Dialog/littletown/
        AssetDatabase.SaveAssets();
    }

    private bool SaveExposedProperties(DialogueContainer dialogueContainer)
    {
        try
        {
            dialogueContainer.ExposedProperties.AddRange(_targetGraphView.ExposedProperties);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    private bool SaveNodes(DialogueContainer dialogueContainer)
    {
        if (!Edges.Any()) return false;

        //var connectedPorts = Edges.Where(edge => edge.input.node != null).OrderByDescending(edge => ((BaseNode)(edge.output.node)).EntryPoint).ToArray();
        var connectedPorts = Edges.Where(x => x.input.node != null).ToArray();
        for (var i = 0; i < connectedPorts.Length; i++)
        {
            var outputNode = connectedPorts[i].output.node as BaseNode;
            var inputNode = connectedPorts[i].input.node as BaseNode;

            dialogueContainer.NodeLinks.Add(new NodeLinkData
            {
                BaseNodeGuid = outputNode.Guid,
                PortName = connectedPorts[i].output.portName,
                TargetNodeGuid = inputNode.Guid
            });
        }

        foreach (var nodeData in Nodes.Where(n => !n.EntryPoint))
        {
            if (nodeData is DialogueNode)
            {
                DialogueNode nodeCasted = nodeData as DialogueNode;
                dialogueContainer.NodeDatas.Add(new DialogueNodeData
                {
                    Guid = nodeCasted.Guid,
                    Position = nodeCasted.GetPosition().position,
                    title = nodeCasted.title,
                    DialogueText = nodeCasted.DialogueText,
                    Side = nodeCasted.Side,
                    Pnj = nodeCasted.Pnj
                });
            }
            else if (nodeData is EventNode)
            {
                EventNode nodeCasted = nodeData as EventNode;
                dialogueContainer.NodeDatas.Add(new EventNodeData
                {
                    Guid = nodeCasted.Guid,
                    Position = nodeCasted.GetPosition().position,
                    title = nodeCasted.title,
                    EventSO = nodeCasted.EventSO,
                    Param = nodeCasted.Param
                });
            }
        }

        return true;
    }

    public void LoadGraph(string fileName)
    {
        _containerCache = GetDialogue(fileName);

        if (_containerCache == null)
        {
            EditorUtility.DisplayDialog("File Not Found", "target dialogue graph file does not exists!", "OK");
            return;
        }

        ClearGraph();
        CreateNodes();
        ConnectNodes();
        CreateExposedProperties();
    }

    private void CreateExposedProperties()
    {
        _targetGraphView.ClearBlackBoardAndExposedproperties();

        foreach (var exposedProperty in _containerCache.ExposedProperties)
        {
            _targetGraphView.AddPropertyToBlackBoard(exposedProperty);
        }
    }

    private void ConnectNodes()
    {
        for (var i = 0; i < Nodes.Count; i++)
        {
            var connections = _containerCache.NodeLinks.Where(x => x.BaseNodeGuid == Nodes[i].Guid).ToList();
            for (var j = 0; j < connections.Count; j++)
            {
                var targetNodeGuid = connections[j].TargetNodeGuid;
                var targetNode = Nodes.First(x => x.Guid == targetNodeGuid);

                LinkNodes(Nodes[i].outputContainer[j].Q<Port>(), (Port)targetNode.inputContainer[0]);

                targetNode.SetPosition(new Rect(
                    _containerCache.NodeDatas.First(x => x.Guid == targetNodeGuid).Position,
                    _targetGraphView.defaultNodeSize
                ));
            }
        }
    }

    private void LinkNodes(Port output, Port input)
    {
        var tempEdge = new Edge
        {
            output = output,
            input = input
        };

        tempEdge?.input.Connect(tempEdge);
        tempEdge?.output.Connect(tempEdge);

        _targetGraphView.Add(tempEdge);
    }

    private void CreateNodes()
    {
        foreach (var nodeData in _containerCache.NodeDatas)
        {
            BaseNode baseNode = null;

            if (nodeData is DialogueNodeData)
            {
                var castedNode = GetDialogueNodeFromData(nodeData as DialogueNodeData);
                baseNode = _targetGraphView.CreateDialogueNode(castedNode, Vector2.zero);
            }
            else if (nodeData is EventNodeData)
            {
                var castedNode = GetEventNodeFromData(nodeData as EventNodeData);
                baseNode = _targetGraphView.CreateEventNode(castedNode, Vector2.zero);
            }

            if (baseNode == null)
            {
                EditorUtility.DisplayDialog("Error", "Creating Nodes process failed.", "OK");
                return;
            }

            _targetGraphView.AddElement(baseNode);

            //var nodePorts = _containerCache.NodeLinks.Where(x => x.BaseNodeGuid == nodeData.Guid).ToList();
            //nodePorts.ForEach(x => _targetGraphView.AddChoicePort(baseNode, x.PortName));

            if (nodeData is DialogueNodeData)
            {
                var nodePorts = _containerCache.NodeLinks.Where(x => x.BaseNodeGuid == nodeData.Guid).ToList();
                if (nodePorts.Count() > 0)
                {
                    nodePorts.ForEach(x => _targetGraphView.AddChoicePort(baseNode, x.PortName));
                }
                else
                {
                    _targetGraphView.AddChoicePort(baseNode, "Ok");
                }

            }
            else if (nodeData is EventNodeData)
            {
                var nodePorts = _containerCache.NodeLinks.Where(x => x.BaseNodeGuid == nodeData.Guid).ToList();
                if (nodePorts.Count() > 0)
                {
                    nodePorts.ForEach(x => _targetGraphView.AddChoicePort(baseNode, x.PortName));
                }
                else
                {
                    _targetGraphView.AddChoicePort(baseNode, "Output");
                }
            }
        }
    }

    DialogueContainer GetDialogue(string fileName)
    {
        return Resources.Load<DialogueContainer>("Dialogue/" + fileName);
    }

    private void ClearGraph()
    {
        Nodes.Find(x => x.EntryPoint).Guid = _containerCache.NodeLinks[0].BaseNodeGuid;

        foreach (var node in Nodes)
        {
            if (node.EntryPoint) continue;

            Edges.Where(x => x.input.node == node).ToList()
                .ForEach(edge => _targetGraphView.RemoveElement(edge));

            _targetGraphView.RemoveElement(node);
        }
    }

    DialogueNode GetDialogueNodeFromData(DialogueNodeData nodeData)
    {
        return new DialogueNode
        {
            Guid = nodeData.Guid,
            title = nodeData.title,
            Position = nodeData.Position,
            EntryPoint = nodeData.EntryPoint,
            DialogueText = nodeData.DialogueText,
            Side = nodeData.Side,
            Pnj = nodeData.Pnj
        };
    }

    EventNode GetEventNodeFromData(EventNodeData nodeData)
    {
        return new EventNode
        {
            Guid = nodeData.Guid,
            title = nodeData.title,
            Position = nodeData.Position,
            EntryPoint = nodeData.EntryPoint,
            EventSO = nodeData.EventSO,
            Param = nodeData.Param
        };
    }

}