using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraph : EditorWindow
{
    private DialogueGraphView _graphView;
    private string _fileName = "New_narrative";

    [MenuItem("Graph/Dialogue Graph")]
    public static void OpenDialogueGraphWindow()
    {
        var window = GetWindow<DialogueGraph>();
        window.titleContent = new GUIContent("Dialogue Graph");
    }

    private void GenerateToolBar()
    {
        var toolbar = new Toolbar();

        var fileNameTextField = new TextField("File Name:");
        fileNameTextField.SetValueWithoutNotify(_fileName);
        fileNameTextField.MarkDirtyRepaint();
        fileNameTextField.RegisterValueChangedCallback(evt => _fileName = evt.newValue);
        toolbar.Add(fileNameTextField);

        var loadButton = new Button(() => RequestDataOperation(false)) { text = "Load Data" };
        loadButton.style.backgroundColor = new Color(0.1f, 0.1f, 0.5f);
        toolbar.Add(loadButton);

        var saveButton = new Button(() => RequestDataOperation(true)) { text = "Save Data" };
        saveButton.style.backgroundColor = new Color(0.1f, 0.5f, 0.1f);
        toolbar.Add(saveButton);

        var clearButton = new Button(() => ClearGraph()) { text = "Clear Graph" };
        clearButton.style.backgroundColor = new Color(0.5f, 0.1f, 0.1f);
        toolbar.Add(clearButton);

        /*
        var nodeCreateButton = new Button(() => { _graphView.CreateNode("Dialogue Node"); });
        nodeCreateButton.text = "Create Node";
        toolbar.Add(nodeCreateButton);
        */

        rootVisualElement.Add(toolbar);
    }

    private void ClearGraph()
    {
        if (string.IsNullOrEmpty(_fileName))
        {
            //EditorUtility.DisplayDialog("Invalid file Name", "Please enter a valid file name.", "OK");
            return;
        }

        var saveUtility = GraphSaveUtility.GetInstance(_graphView);
        saveUtility.ClearGraph(_fileName);
    }

    private void GenerateMiniMap()
    {
        var miniMap = new MiniMap { anchored = true };
        //var cords = _graphView.contentViewContainer.WorldToLocal(new Vector2(this.maxSize.x - 10, 30));
        //miniMap.SetPosition(new Rect(cords.x, cords.y, 200, 140));
        miniMap.SetPosition(new Rect(this.position.width - 210, 30, 200, 140));
        _graphView.Add(miniMap);
    }

    private void RequestDataOperation(bool save)
    {
        if (string.IsNullOrEmpty(_fileName))
        {
            EditorUtility.DisplayDialog("Invalid file Name", "Please enter a valid file name.", "OK");
            return;
        }

        var saveUtility = GraphSaveUtility.GetInstance(_graphView);
        if (save)
        {
            saveUtility.SaveGraph(_fileName);
        }
        else
        {
            saveUtility.LoadGraph(_fileName);
        }
    }

    private void OnEnable()
    {
        ConstructGraphView();
        GenerateToolBar();
        GenerateMiniMap();
        GenerateBlackBoard();
    }

    private void GenerateBlackBoard()
    {
        var blackboard = new Blackboard(_graphView);
        blackboard.Add(new BlackboardSection{ title = "Exposed Properties" });
        blackboard.addItemRequested = _blackboard => { _graphView.AddPropertyToBlackBoard(new ExposedProperty()); };
        blackboard.editTextRequested = (blackboard, element, newValue) =>
        {
            var oldPropertyName = ((BlackboardField)element).text;
            if (_graphView.ExposedProperties.Any(x => x.PropertyName == newValue))
            {
                EditorUtility.DisplayDialog("Error", "This property name already exists, please chose another one.", "OK");
                return;
            }

            var propertyIndex = _graphView.ExposedProperties.FindIndex(x => x.PropertyName == oldPropertyName);
            _graphView.ExposedProperties[propertyIndex].PropertyName = newValue;
            ((BlackboardField)element).text = newValue;
        };
        blackboard.scrollable = true;

        blackboard.SetPosition(new Rect(10, 30, 200, 300));
        
        _graphView.Add(blackboard);
        _graphView.Blackboard = blackboard;
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(_graphView);
    }

    private void ConstructGraphView()
    {
        _graphView = new DialogueGraphView(this)
        {
            name = "Dialogue Graph"
        };

        _graphView.StretchToParentSize();
        rootVisualElement.Add(_graphView);
    }

    private void OnGUI()
    {
        if (Event.current.rawType == EventType.Layout)
        {
            MiniMap _miniMap = _graphView.contentContainer.Q<MiniMap>();
            _miniMap.SetPosition(new Rect(this.position.width - 210, 30, 200, 140));
        }
    }

    /*
    [UnityEditor.Callbacks.OnOpenAsset]
    public static bool OnOpenAsset(int instanceID, int line)
    {
        //Get the instanceID of the DialogueGraphContainer to find it in the project.

        string assetPath = AssetDatabase.GetAssetPath(instanceID);

        DialogueContainer dgc = AssetDatabase.LoadAssetAtPath<DialogueContainer>(assetPath);

        if (dgc != null)
        {
            DialogueGraph window = GetWindow<DialogueGraph>();

            window.titleContent = new GUIContent($"{dgc.name} (Dialogue Graph)");

            //Once the window is opened, we load the content of the scriptable object.
            //Even if the new name doesn't show up in the TextField, we need to assign the _fileName

            //to load the appropriate file.
            window._fileName = dgc.name;
            window.RequestDataOperation(DataOperation.Load);

            return true;
        }

        //If object not found, won't open anything since we need the object to draw the window.
        return false;
    }
    */

}
