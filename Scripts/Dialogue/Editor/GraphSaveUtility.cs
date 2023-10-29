using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class GraphSaveUtility
{
    public string _currentFileName;
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

        _currentFileName = fileName;
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
        var connectedPorts = Edges.ToArray(); //.Where(x => x.input.node != null)
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
                string[] outputs = nodeCasted.outputContainer.Children().Select(x => (x as Port).portName).ToArray();

                dialogueContainer.NodeDatas.Add(new DialogueNodeData
                {
                    Guid = nodeCasted.Guid,
                    Position = nodeCasted.GetPosition().position,
                    title = nodeCasted.title,
                    DialogueText = nodeCasted.DialogueText,
                    DialogueCode = nodeCasted.DialogueCode,
                    Side = nodeCasted.Side,
                    Type = nodeCasted.Type,
                    Pnj = nodeCasted.Pnj,
                    Outputs = outputs
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
                    Type = nodeCasted.Type,
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

        _currentFileName = fileName;

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
        foreach (BaseNodeData nodeData in _containerCache.NodeDatas)
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
                string[] outputPorts = (nodeData as DialogueNodeData).Outputs;
                var outputConnexions = _containerCache.NodeLinks.Where(x => x.BaseNodeGuid == nodeData.Guid).ToList();

                if (outputPorts.Length > 0)
                {
                    outputPorts.ToList().ForEach(portName => _targetGraphView.AddChoicePort(baseNode, portName));
                }
                else
                {
                    _targetGraphView.AddChoicePort(baseNode, ">");
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

    public void ClearGraph(string fileName = null)
    {
        _currentFileName ??= fileName;

        _containerCache = GetDialogue(_currentFileName);
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
            DialogueCode = nodeData.DialogueCode,
            Side = nodeData.Side,
            Type = nodeData.Type,
            Pnj = nodeData.Pnj,
            Outputs = nodeData.Outputs
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
            Type = nodeData.Type,
            Param = nodeData.Param
        };
    }

}