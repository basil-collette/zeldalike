using System.Collections.Generic;
using UnityEngine;

public class OneShotHitable : Hitable
{
    List<EffectTypeEnum> effectTypeTriggerable = new List<EffectTypeEnum>() {
        EffectTypeEnum.neutral,
        EffectTypeEnum.slash,
        EffectTypeEnum.bump,
        EffectTypeEnum.pierce,
        EffectTypeEnum.knockback
    };

    public override void Hit(GameObject attacker, List<Effect> hit, string attackerTag)
    {
        foreach (Effect effect in hit)
        {
            if (hit.Exists(effect => effectTypeTriggerable.Contains(effect.effectType)))
            {
                Die();
                return;
            }
        }
    }

    public override void Effect(Vector3 attackerPos, Effect effect)
    {
        //nothing, intentionnaly
    }

}
