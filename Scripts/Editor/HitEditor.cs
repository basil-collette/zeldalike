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

        EditorGUILayout.LabelField("Adding");

        if (GUILayout.Button("Effect"))
        {
            hit.effects.Add(new Effect(EffectTypeEnum.neutral, 0));

            EditorUtility.SetDirty(hit);
        }

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("KnockBack"))
        {
            KnockBackEffect kbEffect = new KnockBackEffect(
                EffectTypeEnum.knockback,
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

        /*
        SerializedProperty effectsProperty = serializedObject.FindProperty("effects");
        EditorGUILayout.PropertyField(effectsProperty, true);
        */

        DrawDefaultInspector();

        serializedObject.ApplyModifiedProperties();
    }

}
