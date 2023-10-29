using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraphView : GraphView
{
    public readonly Vector2 defaultNodeSize = new Vector2(150, 200);
    public Blackboard Blackboard;
    public List<ExposedProperty> ExposedProperties = new List<ExposedProperty>();
    private NodeSearchWindow _searchWindow;

    public DialogueGraphView(EditorWindow editorWindow)
    {
        styleSheets.Add(Resources.Load<StyleSheet>("DialogueGraph"));
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var grid = new GridBackground();
        Insert(0, grid);
        grid.StretchToParentSize();

        AddElement(GenerateEntryPointNode());
        AddSearchWindow(editorWindow);
    }

    private void AddSearchWindow(EditorWindow editorWindow)
    {
        _searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
        _searchWindow.Init(editorWindow, this);
        nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new List<Port>();
        ports.ForEach(port =>
        {
            if (startPort != port && startPort.node != port.node)
                compatiblePorts.Add(port);
        });

        return compatiblePorts;
    }

    private Port GeneratePort(Node node, Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
    {
        return node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float)); //Arbitrary type
    }

    private DialogueNode GenerateEntryPointNode()
    {
        var node = new DialogueNode
        {
            title = "Start",
            Guid = Guid.NewGuid().ToString(),
            DialogueText = "ENTRYPOINT",
            DialogueCode = "",
            EntryPoint = true
        };

        var generatedPort = GeneratePort(node, Direction.Output);
        generatedPort.portName = "Next";
        node.outputContainer.Add(generatedPort);

        node.capabilities &= ~Capabilities.Movable;
        node.capabilities &= ~Capabilities.Deletable;

        node.RefreshExpandedState();
        node.RefreshPorts();

        node.SetPosition(new Rect(100, 200, 100, 150));
        return node;
    }

    public void ClearBlackBoardAndExposedproperties()
    {
        ExposedProperties.Clear();
        Blackboard.Clear();
    }

    public void AddPropertyToBlackBoard(ExposedProperty exposedProperty)
    {
        var localPropertyName = exposedProperty.PropertyName;
        var localPropertyValue = exposedProperty.PropertyValue;
        while (ExposedProperties.Any(x => x.PropertyName == localPropertyName))
            localPropertyName = $"{localPropertyName}(1)";

        var property = new ExposedProperty();
        property.PropertyName = localPropertyName;
        property.PropertyValue = localPropertyValue;
        ExposedProperties.Add(property);

        var container = new VisualElement();
        var blackboardField = new BlackboardField
        {
            text = property.PropertyName,
            typeText = "string property"
        };
        container.Add(blackboardField);

        var propertyValueTextField = new TextField("Value:")
        {
            value = localPropertyValue
        };
        propertyValueTextField.RegisterValueChangedCallback(evt => {
            var changingPropertyIndex = ExposedProperties.FindIndex(x => x.PropertyName == property.PropertyName);
            ExposedProperties[changingPropertyIndex].PropertyValue = evt.newValue;
        });

        var blackBoardValueRow = new BlackboardRow(blackboardField, propertyValueTextField);
        container.Add(blackBoardValueRow);

        Blackboard.Add(container);
    }

    public DialogueNode CreateNode(Vector2 position)
    {
        var node = CreateDialogueNode(position);
        AddElement(node);
        return node;
    }

    public EventNode CreateEvent(Vector2 position)
    {
        var node = CreateEventNode(position);
        AddElement(node);
        return node;
    }

    public DialogueNode CreateDialogueNode(DialogueNode dialogueNode, Vector2 position)
    {
        dialogueNode.styleSheets.Add(Resources.Load<StyleSheet>("NodeStyle"));

        var inputPort = GeneratePort(dialogueNode, Direction.Input, Port.Capacity.Multi);
        inputPort.portName = "Input";
        dialogueNode.inputContainer.Add(inputPort);

        var addPortButton = new Button(() => { AddChoicePort(dialogueNode); });
        addPortButton.text = "New Choice";
        dialogueNode.titleContainer.Add(addPortButton);

        var dialogueTextField = new TextField("Text");
        dialogueTextField.SetValueWithoutNotify(dialogueNode.DialogueText);
        dialogueTextField.RegisterValueChangedCallback((evt) => { dialogueNode.DialogueText = evt.newValue; });
        dialogueNode.mainContainer.Add(dialogueTextField);

        var dialogueCodeField = new TextField("Code");
        dialogueCodeField.SetValueWithoutNotify(dialogueNode.DialogueCode);
        dialogueCodeField.RegisterValueChangedCallback((evt) => { dialogueNode.DialogueCode = evt.newValue; });
        dialogueNode.mainContainer.Add(dialogueCodeField);

        var TypeField = new EnumField();
        TypeField.Init(DialogueTypeEnum.talk);
        TypeField.SetValueWithoutNotify(dialogueNode.Type);
        TypeField.RegisterValueChangedCallback((evt) => dialogueNode.Type = (DialogueTypeEnum)evt.newValue);
        dialogueNode.mainContainer.Add(TypeField);

        var sideField = new EnumField();
        sideField.Init(DialogueNodeSide.left);
        sideField.SetValueWithoutNotify(dialogueNode.Side);
        sideField.RegisterValueChangedCallback((evt) => dialogueNode.Side = (DialogueNodeSide)evt.newValue);
        dialogueNode.mainContainer.Add(sideField);

        var spriteField = new ObjectField
        {
            objectType = typeof(Pnj)
        };
        spriteField.SetValueWithoutNotify(dialogueNode.Pnj);
        spriteField.RegisterValueChangedCallback((evt) => dialogueNode.Pnj = (Pnj)evt.newValue);
        dialogueNode.mainContainer.Add(spriteField);

        /*
        var audioField = new ObjectField
        {
            objectType = typeof(AudioClip)
        };
        audioField.SetValueWithoutNotify(dialogueNode.AudioClip);
        audioField.RegisterValueChangedCallback((evt) => dialogueNode.AudioClip = (AudioClip)evt.newValue);
        dialogueNode.mainContainer.Add(audioField);
        */

        foreach (string output in dialogueNode.Outputs)
        {
            var generatedPort = GeneratePort(dialogueNode, Direction.Output);
            generatedPort.portName = output;
        }

        dialogueNode.RefreshExpandedState();
        dialogueNode.RefreshPorts();
        dialogueNode.SetPosition(new Rect(position, defaultNodeSize));

        return dialogueNode;
    }

    public DialogueNode CreateDialogueNode(Vector2 position)
    {
        var dialogueNode = new DialogueNode
        {
            title = "Dialogue",
            Guid = Guid.NewGuid().ToString()
        };

        return CreateDialogueNode(dialogueNode, position);
    }

    public EventNode CreateEventNode(EventNode eventNode, Vector2 position)
    {
        var inputPort = GeneratePort(eventNode, Direction.Input, Port.Capacity.Multi);
        inputPort.portName = "Input";
        eventNode.inputContainer.Add(inputPort);

        /*
        var outputPort = GeneratePort(eventNode, Direction.Output);
        outputPort.portName = "Output";
        eventNode.inputContainer.Add(outputPort);
        */

        eventNode.styleSheets.Add(Resources.Load<StyleSheet>("EventStyle"));

        var eventField = new EnumField();
        eventField.Init(EventTypeEnum.StartQuest);
        eventField.SetValueWithoutNotify(eventNode.Type);
        eventField.RegisterValueChangedCallback((evt) => eventNode.Type = (EventTypeEnum)evt.newValue);
        eventNode.mainContainer.Add(eventField);

        var textField = new TextField();
        textField.SetValueWithoutNotify(eventNode.Param);
        textField.RegisterValueChangedCallback((evt) => eventNode.Param = evt.newValue);
        eventNode.mainContainer.Add(textField);

        eventNode.RefreshExpandedState();
        eventNode.RefreshPorts();
        eventNode.SetPosition(new Rect(position, defaultNodeSize));

        return eventNode;
    }

    public EventNode CreateEventNode(Vector2 position)
    {
        var dialogueEvent = new EventNode
        {
            title = "Event",
            Guid = Guid.NewGuid().ToString()
        };

        return CreateEventNode(dialogueEvent, position);
    }

    public void AddChoicePort(BaseNode dialogueNode, string overriddenPortName = "")
    {
        var generatedPort = GeneratePort(dialogueNode, Direction.Output);

        var outputPortCount = dialogueNode.outputContainer.Query("connector").ToList().Count;
        generatedPort.portName = $"Choice {outputPortCount}";

        var choicePortName = string.IsNullOrEmpty(overriddenPortName)
            ? $"Choice {outputPortCount}"
            : overriddenPortName;

        if (dialogueNode is DialogueNode)
        {
            var oldLabel = generatedPort.contentContainer.Q<Label>("type");
            generatedPort.contentContainer.Remove(oldLabel);

            var textField = new TextField
            {
                name = string.Empty,
                value = choicePortName
            };
            textField.RegisterValueChangedCallback((evt) => generatedPort.portName = evt.newValue);
            generatedPort.contentContainer.Add(new Label("  "));
            generatedPort.contentContainer.Add(textField);

            var deleteButton = new Button(() => RemovePort(dialogueNode, generatedPort))
            {
                text = "X"
            };
            generatedPort.contentContainer.Add(deleteButton);
        }

        generatedPort.portName = choicePortName;
        dialogueNode.outputContainer.Add(generatedPort);
        dialogueNode.RefreshPorts();
        dialogueNode.RefreshExpandedState();
    }

    private void RemovePort(BaseNode dialogueNode, Port generatedPort)
    {
        var targetEdge = edges.ToList().Where(x =>
            x.output.portName == generatedPort.portName
            && x.output.node == generatedPort.node);

        if (targetEdge.Any())
        {
            var edge = targetEdge.First();
            edge.input.Disconnect(edge);
            RemoveElement(targetEdge.First());
        }

        dialogueNode.outputContainer.Remove(generatedPort);
        dialogueNode.RefreshPorts();
        dialogueNode.RefreshExpandedState();
    }

}
