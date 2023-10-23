using UnityEngine;

[System.Serializable]
public class Effect
{
    [SerializeField]
    public EffectTypeEnum effectType = EffectTypeEnum.neutral;

    [SerializeField]
    public float amount = 1;
    
    public Effect(EffectTypeEnum effectType, float amout)
    {
        this.effectType = effectType;
        this.amount = amout;
    }

}