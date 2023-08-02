using System.Collections.Generic;
using UnityEngine;

public class OneShotHitable : Hitable
{
    List<EffectEnum> effectTypeTriggerable = new List<EffectEnum>() {
        EffectEnum.neutral,
        EffectEnum.slash,
        EffectEnum.bump,
        EffectEnum.pierce,
        EffectEnum.knockback
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
