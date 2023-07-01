using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

//[CanEditMultipleObjects()]
[CustomEditor(typeof(Quest), true)]
public class QuestEditor : Editor
{
    private ReorderableList _itemList;

    private Quest _quest;
    private bool _showQuestSteps = true;
    private List<QuestStep> _questStepsSelected;

    private void OnEnable()
    {
        _itemList = new ReorderableList(serializedObject, serializedObject.FindProperty("QuestSteps"), true, true, true, true);
        _itemList.drawHeaderCallback = DrawHeader;
        _itemList.drawElementCallback = DrawElement;
        _itemList.onAddCallback = OnAddItem;
        _itemList.onRemoveCallback = OnRemoveItem;
        _itemList.elementHeightCallback = GetElementHeight;
        _itemList.headerHeight = 0f;
        
        _questStepsSelected = new List<QuestStep>();
    }

    public override void OnInspectorGUI()
    {
        //Selection

        serializedObject.Update();

        _quest = target as Quest;

        SerializedProperty nameProperty = serializedObject.FindProperty("Name");
        EditorGUILayout.PropertyField(nameProperty, true);

        SerializedProperty descriptionProperty = serializedObject.FindProperty("Description");
        EditorGUILayout.PropertyField(descriptionProperty, true);

        SerializedProperty isCompletedProperty = serializedObject.FindProperty("IsCompleted");
        EditorGUILayout.PropertyField(isCompletedProperty, true);

        EditorGUILayout.LabelField("Adding");

        EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Kill"))
            {
                AddGoal(new KillGoal());
            }
            if (GUILayout.Button("Collect"))
            {
                AddGoal(new CollectGoal());
            }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Discussion"))
            {
                AddGoal(new DiscussionGoal());
            }
            if (GUILayout.Button("Location"))
            {
                AddGoal(new LocationGoal());
            }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Talk"))
            {
                AddGoal(new TalkGoal());
            }
            if (GUILayout.Button("Collect"))
            {
                AddGoal(new CollectGoal());
            }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(2);

        EditorGUILayout.BeginHorizontal();
            _showQuestSteps = EditorGUILayout.Foldout(_showQuestSteps, "Quest Steps");
            EditorGUILayout.IntField((_quest.QuestSteps == null) ? 0 :_quest.QuestSteps.Count, GUILayout.Width(40));
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(4);
        if (_showQuestSteps)
        {
            EditorGUI.indentLevel++;
            _itemList.DoLayoutList();
            EditorGUI.indentLevel--;
        }

        serializedObject.ApplyModifiedProperties();
    }

    void AddGoal(Goal goal)
    {
        foreach (QuestStep questStep in _questStepsSelected)
        {
            questStep.Goals.Add(goal);
        }

        EditorUtility.SetDirty(_quest);
    }

    private void DrawHeader(Rect rect)
    {
        //EditorGUI.LabelField(rect, "Quest Steps");
    }

    private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        SerializedProperty element = _itemList.serializedProperty.GetArrayElementAtIndex(index);

        if (isActive)
        {
            if (!_questStepsSelected.Contains(_quest.QuestSteps[index]))
                _questStepsSelected.Add(_quest.QuestSteps[index]);
        }
        else
        {
            if (_questStepsSelected.Contains(_quest.QuestSteps[index]))
                _questStepsSelected.Remove(_quest.QuestSteps[index]);
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
