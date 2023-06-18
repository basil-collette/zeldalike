using UnityEngine;

[System.Serializable]
public class Effect
{
    [SerializeField]
    public EffectEnum effectType = EffectEnum.neutral;

    [SerializeField]
    public float amount = 1;
    
    public Effect(EffectEnum effectType, float amout)
    {
        this.effectType = effectType;
        this.amount = amout;
    }

}