using UnityEngine;

[CreateAssetMenu(menuName="ScriptableObject/Vector")]
public class VectorValue : ScriptableObject, ISerializationCallbackReceiver
{
    public Vector2 initalValue;
    public Vector2 defaultValue;

    public void OnAfterDeserialize()
    {
        initalValue = defaultValue;
    }

    public void OnBeforeSerialize() {}

}
