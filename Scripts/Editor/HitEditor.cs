using UnityEditor;
using UnityEngine;

//[CanEditMultipleObjects()]
[CustomEditor(typeof(Hit), true)]
public class HitEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        Hit hit = target as Hit;

        SerializedProperty attackerTagProperty = serializedObject.FindProperty("attackerTag");
        EditorGUILayout.PropertyField(attackerTagProperty);

        EditorGUILayout.LabelField("Adding");

        if (GUILayout.Button("Effect"))
        {
            hit.effects.Add(new Effect(EffectEnum.neutral, 0));

            EditorUtility.SetDirty(hit);
        }

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("KnockBack"))
        {
            KnockBackEffect kbEffect = new KnockBackEffect(
                EffectEnum.knockback,
                0
            );

            hit.effects.Add(kbEffect);

            EditorUtility.SetDirty(hit);
        }
        if (GUILayout.Button("Cycle"))
        {
            //
        }
        EditorGUILayout.EndHorizontal();

        SerializedProperty effectsProperty = serializedObject.FindProperty("effects");
        EditorGUILayout.PropertyField(effectsProperty, true);

        //Show list on editor
        InitEffectList(effectsProperty);

        serializedObject.ApplyModifiedProperties();
    }

    void InitEffectList(SerializedProperty effectsProperty)
    {
        for (int i = 0; i < effectsProperty.arraySize; i++)
        {
            SerializedProperty effectProperty = effectsProperty.GetArrayElementAtIndex(i);

            //InitItemMenu(effectProperty);
        }
    }

    void InitItemMenu(SerializedProperty effectProperty)
    {
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(effectProperty, true);
        
        /*
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Type");
        EditorGUILayout.LabelField(effectProperty.FindPropertyRelative("effectType").);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Amount");
        EditorGUILayout.FloatField(effectProperty.FindPropertyRelative("amount").floatValue);
        EditorGUILayout.EndHorizontal();
        */
        EditorGUI.indentLevel--;
    }

}
