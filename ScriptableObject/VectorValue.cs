using UnityEngine;

[CreateAssetMenu(menuName="ScriptableObject/Vector")]
public class VectorValue : ScriptableObject, ISerializationCallbackReceiver
{
    public Vector2 initalValue;
    public Vector2 deffaultValue;

    public void OnAfterDeserialize()
    {
        initalValue = deffaultValue;
    }

    public void OnBeforeSerialize() {}

}
