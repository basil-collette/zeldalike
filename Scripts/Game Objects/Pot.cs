using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : Hitable
{
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    public override void Hit(GameObject attacker, List<Effect> hit)
    {
        foreach (Effect effect in hit)
        {
            if (effect.effectType == EffectEnum.neutral
                || effect.effectType == EffectEnum.slash
                || effect.effectType == EffectEnum.bump
                || effect.effectType == EffectEnum.knockback
                || effect.effectType == EffectEnum.pierce)
            {
                StartCoroutine(breackCo());
                return;
            }
        }
    }

    public override void Effect(Vector3 attackerPos, Effect effect)
    {
        //
    }

    IEnumerator breackCo()
    {
        anim.SetBool("active", true);

        yield return new WaitForSeconds(0.3f);

        anim.SetBool("active", false);

        this.gameObject.SetActive(false);
    }

}
