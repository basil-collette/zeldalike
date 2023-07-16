using UnityEngine;

[CreateAssetMenu(menuName="ScriptableObject/Float")]
public class FloatValue : ScriptableObject, ISerializationCallbackReceiver
{
    public float initialValue = 1;
    public float RuntimeValue;

    public void OnAfterDeserialize()
    {
        RuntimeValue = initialValue;
    }

    public void OnBeforeSerialize()
    {
        
    }

}
