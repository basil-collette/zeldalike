using UnityEngine;

[System.Serializable]
public class KnockBackEffect : Effect
{
    [SerializeField]
    public float knockTime = 0.3f;

    [HideInInspector]
    public static float THRUST = 4f;

    public KnockBackEffect(
        EffectEnum effectType,
        float amount,
        float knockTime = 0.3f
    ) : base(effectType, amount)
    {
        this.knockTime = knockTime;
    }

}
