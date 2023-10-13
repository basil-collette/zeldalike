using UnityEditor;
using UnityEngine;

//[CanEditMultipleObjects()]
[CustomEditor(typeof(LootTable), true)]
public class LootTableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        LootTable lootTable = target as LootTable;

        EditorGUILayout.LabelField("Adding");

        if (GUILayout.Button("Default"))
        {
            lootTable._loots.Add(new DefaultLoot());

            EditorUtility.SetDirty(lootTable);
        }
        if (GUILayout.Button("Item"))
        {
            lootTable._loots.Add(new ItemLoot());

            EditorUtility.SetDirty(lootTable);
        }

        /*
        SerializedProperty effectsProperty = serializedObject.FindProperty("effects");
        EditorGUILayout.PropertyField(effectsProperty, true);
        */

        DrawDefaultInspector();

        serializedObject.ApplyModifiedProperties();
    }

}
