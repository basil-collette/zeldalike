using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : Hitable
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
                StartCoroutine(breackCo());
                return;
            }
        }
    }

    public override void Effect(Vector3 attackerPos, Effect effect)
    {
        //nothing, intentionnaly
    }

    IEnumerator breackCo()
    {
        Animator anim = GetComponent<Animator>();

        anim.SetBool("active", true);

        yield return new WaitForSeconds(0.3f);

        anim.SetBool("active", false);

        this.gameObject.SetActive(false);
    }

}
