using UnityEngine;

[System.Serializable]
public class CycleDammageEffect : Effect
{
    [SerializeField]
    public float duration = 1;

    public CycleDammageEffect(
        EffectTypeEnum effectType,
        float amout,
        float duration
    ) : base(effectType, amout)
    {

        this.duration = duration;
    }

}