using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

//[CanEditMultipleObjects()]
[CustomEditor(typeof(PNJDialogues), true)]
public class PNJDialoguesEditor : Editor
{
    private ReorderableList _itemList;

    private PNJDialogues _pnjDialogues;
    private bool _showPnjDialogues = true;
    private List<DialogueReference> _dialoguesRefSelected;

    private void OnEnable()
    {
        _itemList = new ReorderableList(serializedObject, serializedObject.FindProperty("Dialogues"), true, true, true, true);
        _itemList.drawHeaderCallback = DrawHeader;
        _itemList.drawElementCallback = DrawElement;
        _itemList.onAddCallback = OnAddItem;
        _itemList.onRemoveCallback = OnRemoveItem;
        _itemList.elementHeightCallback = GetElementHeight;
        _itemList.headerHeight = 0f;

        _dialoguesRefSelected = new List<DialogueReference>();
    }

    public override void OnInspectorGUI()
    {
        //Selection

        serializedObject.Update();

        _pnjDialogues = target as PNJDialogues;

        EditorGUILayout.LabelField("Adding");

        EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Started Quest"))
            {
                AddCondition(new DialogueConditionStartedQuest());
            }
            if (GUILayout.Button("End Quest"))
            {
                AddCondition(new DialogueConditionEndQuest());
            }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Possess"))
            {
                AddCondition(new DialogueConditionPossess());
            }
            if (GUILayout.Button("Have Talk"))
            {
                AddCondition(new DialogueConditionHaveTalk());
            }
        EditorGUILayout.EndHorizontal();

        /*
        EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Location"))
            {
                AddCondition();
            }
        EditorGUILayout.EndHorizontal();
        */

        GUILayout.Space(2);

        EditorGUILayout.BeginHorizontal();
        _showPnjDialogues = EditorGUILayout.Foldout(_showPnjDialogues, "Dialogues");
        EditorGUILayout.IntField((_pnjDialogues.Dialogues == null) ? 0 : _pnjDialogues.Dialogues.Count, GUILayout.Width(40));
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(4);
        if (_showPnjDialogues)
        {
            EditorGUI.indentLevel++;
            _itemList.DoLayoutList();
            EditorGUI.indentLevel--;
        }

        serializedObject.ApplyModifiedProperties();
    }

    void AddCondition(DialogueCondition dialogueCondition)
    {
        foreach (DialogueReference dialogueRef in _dialoguesRefSelected)
        {
            dialogueRef.Conditions.Add(dialogueCondition);
        }

        EditorUtility.SetDirty(_pnjDialogues);
    }

    private void DrawHeader(Rect rect)
    {
        //EditorGUI.LabelField(rect, "Dialogues");
    }

    private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        SerializedProperty element = _itemList.serializedProperty.GetArrayElementAtIndex(index);

        if (isActive)
        {
            if (!_dialoguesRefSelected.Contains(_pnjDialogues.Dialogues[index]))
                _dialoguesRefSelected.Add(_pnjDialogues.Dialogues[index]);
        }
        else
        {
            if (_dialoguesRefSelected.Contains(_pnjDialogues.Dialogues[index]))
                _dialoguesRefSelected.Remove(_pnjDialogues.Dialogues[index]);
        }

        rect.x += 10;

        if (index % 2 != 0)
        {
            GUI.Box(new Rect(rect.x - 30, rect.y - 2, rect.width + 26, rect.height + 2), GUIContent.none);
        }

        var newRect = new Rect(rect.x, rect.y, rect.width - 10, rect.height);

        EditorGUI.PropertyField(newRect, element, true);
    }

    private void OnAddItem(ReorderableList list)
    {
        SerializedProperty listProperty = list.serializedProperty;
        listProperty.arraySize++;
        list.index = listProperty.arraySize - 1;
        SerializedProperty newItem = listProperty.GetArrayElementAtIndex(list.index);
        newItem.objectReferenceValue = null;
    }

    private void OnRemoveItem(ReorderableList list)
    {
        SerializedProperty listProperty = list.serializedProperty;

        if (list.index >= 0 && list.index < listProperty.arraySize)
        {
            listProperty.DeleteArrayElementAtIndex(list.index);
            if (list.index >= listProperty.arraySize)
            {
                list.index = listProperty.arraySize - 1;
            }
        }
    }

    private float GetElementHeight(int index)
    {
        return EditorGUI.GetPropertyHeight(_itemList.serializedProperty.GetArrayElementAtIndex(index)) + 6;
    }

}
