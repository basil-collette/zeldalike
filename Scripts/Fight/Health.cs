using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EffectModificator
{
    public EffectEnum effectType;
    public float mod;
}

public class Health : Hitable
{
    public FloatValue health;
    public Signal healthSignal;
    public List<EffectModificator> effectMods = new List<EffectModificator>();
    public List<Effect> effects;
    public List<Effect> timedEffects;

    protected AliveEntity entity;
    protected Rigidbody2D rigidBody;

    protected void Start()
    {
        this.rigidBody = GetComponent<Rigidbody2D>();
        this.entity = GetComponent<AliveEntity>();
    }

    public override void Hit(Vector3 attackerPos, List<Effect> hit)
    {
        foreach (Effect effect in hit)
        {
            Effect(attackerPos, effect);
        }
    }

    public override void Effect(Vector3 attackerPos, Effect effect)
    {
        EffectModificator effectMod = effectMods.Find(em => em.effectType == effect.effectType);
        if (effectMod != null
            && effectMod.mod <= 0)
        {
            return;
        }

        switch (effect.effectType)
        {
            case EffectEnum.neutral:
            case EffectEnum.bump:
            case EffectEnum.slash:
            case EffectEnum.pierce:
                Dammage(effect);
                break;

            case EffectEnum.frost:
                float frostAmount = RaiseEffect(effect);
                if (IsEnaughtEffect(EffectEnum.frost, frostAmount))
                    Freeze();
                break;

            case EffectEnum.poison:
                //float poisonAmount = RaiseEffect(effect);
                CycleEffect(effect);
                break;

            case EffectEnum.fire:
                float fireAmount = RaiseEffect(effect);
                if (IsEnaughtEffect(EffectEnum.fire, fireAmount)) CycleEffect(effect);
                break;

            case EffectEnum.knockback:
                KnockBack(attackerPos, effect);
                break;

            default:
                break;
        }
    }

    public float GetModifiedAmount(Effect effect)
    {
        EffectModificator effMod = effectMods.Find(effMod => effMod.effectType == effect.effectType);
        
        if (effMod != null) effect.amount *= effMod.mod;

        return effect.amount;
    }

    public void Dammage(Effect effect)
    {
        health.RuntimeValue -= GetModifiedAmount(effect);
        
        if (healthSignal != null)
        {
            healthSignal.Raise();
        } 

        CheckDeath();
    }

    protected virtual void CheckDeath()
    {
        if (this.health.RuntimeValue <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        this.gameObject.SetActive(false);
    }

    // EFFECT PROCESS _________________________________________________________________ EFFECT PROCESS

    protected virtual void CycleEffect(Effect effect, int durationSeconds = 5)
    {
        int index = timedEffects.FindIndex(te => te.effectType == effect.effectType);
        if (index == -1)
        {
            //timedEffects.Add(new Effect(effect.effectType, durationSeconds));

            StartCoroutine(CycleEffectCo(effect.effectType));
        }
        else
        {
            timedEffects[index].amount = durationSeconds;
        }
    }

    protected virtual IEnumerator CycleEffectCo(EffectEnum effectType)
    {
        new WaitForSeconds(1f);
        int index = timedEffects.FindIndex(te => te.effectType == effectType);

        while (index != -1)
        {
            Dammage(timedEffects[index]);

            timedEffects[index].amount--;

            if (timedEffects[index].amount <= 0)
            {
                timedEffects.RemoveAt(index);
            }

            new WaitForSeconds(1f);
            index = timedEffects.FindIndex(te => te.effectType == effectType);
        }

        return null;
    }

    protected virtual float RaiseEffect(Effect effect)
    {
        int index = effectMods.FindIndex(effMod => effMod.effectType == effect.effectType);
        if (index == -1)
        {
            effects.Add(effect);
            return effect.amount;
        }
        else
        {
            effects[index].amount += effect.amount;
            return effects[index].amount;
        }
    }

    protected virtual float AddEffect(Effect effect)
    {
        int index = effectMods.FindIndex(effMod => effMod.effectType == effect.effectType);
        if (index == -1)
        {
            effects.Add(effect);
        }
        else if (effects[index].amount < effect.amount)
        {
            effects[index].amount = effect.amount;
        }
        return effects[index].amount;
    }

    protected virtual void RemoveEffect(Effect effectToRemove)
    {
        Effect effect = effects.Find(effMod => effMod.effectType == effectToRemove.effectType);
        if (effect != null)
        {
            effects.Remove(effect);
        }
    }

    protected virtual bool IsEnaughtEffect(EffectEnum effectType)
    {
        Effect ownedEffect = effects.Find(effMod => effMod.effectType == effectType);

        return IsEnaughtEffect(effectType, ownedEffect.amount);
    }

    protected virtual bool IsEnaughtEffect(EffectEnum effectType, float currentAmount)
    {
        return currentAmount >= GetEffectResistance(effectType);
    }

    protected virtual float GetEffectResistance(EffectEnum effectType)
    {
        float result;

        EffectModificator effectmod = effectMods.Find(effMod => effMod.effectType == effectType);
        if (effectmod != null)
        {
            result = health.initialValue - (health.initialValue * effectmod.mod);
        }
        else
        {
            result = health.initialValue / 4;
        }
        
        return (result < 1) ? 1 : result;
    }

    //EFFECT ACTION _____________________________________________________________________ EFFECT ACTION

    protected virtual void Freeze()
    {
        //
    }

    protected void KnockBack(Vector3 attackerPos, Effect effect)
    {
        KnockBackEffect kbEffect = (KnockBackEffect)effect;
        
        StartCoroutine(KnockCo(attackerPos, kbEffect));
    }

    IEnumerator KnockCo(Vector3 attackerPos, KnockBackEffect kbEffect)
    {
        this.entity.SetState(EntityState.unavailable);

        Vector3 difference = (transform.position - attackerPos).normalized
            * KnockBackEffect.THRUST
            * GetModifiedAmount(kbEffect);

        GetComponent<Rigidbody2D>().AddForce(difference, ForceMode2D.Impulse);
        //this.rigidBody.velocity = difference;
        //transform.position = new Vector2(transform.position.x + difference.x, transform.position.y + difference.y);

        yield return new WaitForSeconds(kbEffect.knockTime);

        this.rigidBody.velocity = Vector2.zero;
        this.entity.SetState(EntityState.idle);
        //other.GetComponent<AliveEntity>().currentEntityState = EntityState.idle;
    }

}
    
    
