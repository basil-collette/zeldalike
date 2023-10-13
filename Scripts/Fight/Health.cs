using Assets.Scripts.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Health : Hitable
{
    public static event Action<string[]> OnDeath;

    public FloatValue _health;
    public Signal _healthSignal;
    public string[] OnDeathParam;
    public List<EffectModificator> _effectMods = new List<EffectModificator>();
    public List<Effect> _effects;
    public List<Effect> _timedEffects;

    public Action _dieOverride;

    bool canBeDammaged = true;

    private void Start()
    {
        //Volontairement vide, permet d'avoir le booleen active dans l'editeur
    }

    public override void Hit(GameObject attacker, List<Effect> hit, string attackerTag)
    {
        foreach (Effect effect in hit)
        {
            if (!canBeDammaged && effect.effectType != EffectEnum.knockback) continue;

            Effect(attacker.transform.position, effect);
        }
    }

    public override void Effect(Vector3 attackerPos, Effect effect)
    {
        EffectModificator? effectMod = _effectMods.Find(em => em.effectType == effect.effectType);
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
        EffectModificator effMod = _effectMods.Find(effMod => effMod.effectType == effect.effectType);
        
        if (effMod != null) effect.amount *= effMod.mod;

        return effect.amount;
    }

    public void Dammage(Effect effect)
    {
        _health.RuntimeValue -= GetModifiedAmount(effect);

        var audioSource = GetComponent<AudioSource>();
        if (audioSource != null && hitSound != null)
        {
            audioSource.clip = hitSound;
            audioSource.Play();
        }

        StartCoroutine(ColorDamageCo());

        if (_healthSignal != null)
        {
            _healthSignal.Raise();
        }

        CheckDeath();
    }

    IEnumerator ColorDamageCo()
    {
        canBeDammaged = false;

        var sprites = GetComponentsInChildren<SpriteRenderer>();

        Array.ForEach(sprites, x => x.color = new Color(1, 0.3f, 0.3f));
        yield return new WaitForSeconds(0.1f);

        Array.ForEach(sprites, x => x.color = new Color(1, 1, 1));
        yield return new WaitForSeconds(0.1f);

        Array.ForEach(sprites, x => x.color = new Color(1, 0.3f, 0.3f));
        yield return new WaitForSeconds(0.1f);

        Array.ForEach(sprites, x => x.color = new Color(1, 1, 1));
        yield return new WaitForSeconds(0.1f);

        Array.ForEach(sprites, x => x.color = new Color(1, 0.3f, 0.3f));
        yield return new WaitForSeconds(0.1f);

        Array.ForEach(sprites, x => x.color = new Color(1, 1, 1));

        canBeDammaged = true;
    }

    public void Heal(float healAmount)
    {
        _health.RuntimeValue += healAmount;
        _health.RuntimeValue = Mathf.Min(_health.RuntimeValue, _health.initialValue);

        FindAnyObjectByType<SoundManager>().PlayEffect("heal", 0.5f);

        if (_healthSignal != null)
        {
            _healthSignal.Raise();
        }
    }

    protected virtual void CheckDeath()
    {
        if (this._health.RuntimeValue <= 0)
        {
            Die();
        }
    }

    public override sealed void Die()
    {
        //Ask the sound helper to play cause this audioSource is destroyed
        //deathSound

        OnDeath?.Invoke(OnDeathParam);

        if (_dieOverride == null)
        {
            base.Die();
        }
        else
        {
            _dieOverride.Invoke();
        }
    }

    public void AddHeartMax()
    {
        _health.initialValue += 1;
        _health.RuntimeValue += 1;

        //update hearts ui
        if (_healthSignal != null) _healthSignal.Raise();
    } 

    // EFFECT PROCESS _________________________________________________________________ EFFECT PROCESS

    protected virtual void CycleEffect(Effect effect, int durationSeconds = 5)
    {
        int index = _timedEffects.FindIndex(te => te.effectType == effect.effectType);
        if (index == -1)
        {
            //timedEffects.Add(new Effect(effect.effectType, durationSeconds));

            StartCoroutine(CycleEffectCo(effect.effectType));
        }
        else
        {
            _timedEffects[index].amount = durationSeconds;
        }
    }

    protected virtual IEnumerator CycleEffectCo(EffectEnum effectType)
    {
        new WaitForSeconds(1f);
        int index = _timedEffects.FindIndex(te => te.effectType == effectType);

        while (index != -1)
        {
            Dammage(_timedEffects[index]);

            _timedEffects[index].amount--;

            if (_timedEffects[index].amount <= 0)
            {
                _timedEffects.RemoveAt(index);
            }

            new WaitForSeconds(1f);
            index = _timedEffects.FindIndex(te => te.effectType == effectType);
        }

        return null;
    }

    protected virtual float RaiseEffect(Effect effect)
    {
        int index = _effectMods.FindIndex(effMod => effMod.effectType == effect.effectType);
        if (index == -1)
        {
            _effects.Add(effect);
            return effect.amount;
        }
        else
        {
            _effects[index].amount += effect.amount;
            return _effects[index].amount;
        }
    }

    protected virtual float AddEffect(Effect effect)
    {
        int index = _effectMods.FindIndex(effMod => effMod.effectType == effect.effectType);
        if (index == -1)
        {
            _effects.Add(effect);
        }
        else if (_effects[index].amount < effect.amount)
        {
            _effects[index].amount = effect.amount;
        }
        return _effects[index].amount;
    }

    protected virtual void RemoveEffect(Effect effectToRemove)
    {
        Effect effect = _effects.Find(effMod => effMod.effectType == effectToRemove.effectType);
        if (effect != null)
        {
            _effects.Remove(effect);
        }
    }

    protected virtual bool IsEnaughtEffect(EffectEnum effectType)
    {
        Effect ownedEffect = _effects.Find(effMod => effMod.effectType == effectType);

        return IsEnaughtEffect(effectType, ownedEffect.amount);
    }

    protected virtual bool IsEnaughtEffect(EffectEnum effectType, float currentAmount)
    {
        return currentAmount >= GetEffectResistance(effectType);
    }

    protected virtual float GetEffectResistance(EffectEnum effectType)
    {
        float result;

        EffectModificator effectmod = _effectMods.Find(effMod => effMod.effectType == effectType);
        if (effectmod != null)
        {
            result = _health.initialValue - (_health.initialValue * effectmod.mod);
        }
        else
        {
            result = _health.initialValue / 4;
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
        GetComponent<AliveEntity>().SetState(EntityState.unavailable);

        Vector3 difference = (transform.position - attackerPos).normalized
            * KnockBackEffect.THRUST
            * GetModifiedAmount(kbEffect);

        GetComponent<Rigidbody2D>().AddForce(difference, ForceMode2D.Impulse);
        //this.rigidBody.velocity = difference;
        //transform.position = new Vector2(transform.position.x + difference.x, transform.position.y + difference.y);

        yield return new WaitForSeconds(kbEffect.knockTime);

        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<AliveEntity>().SetState(EntityState.idle);
        //other.GetComponent<AliveEntity>().currentEntityState = EntityState.idle;
    }

}

[System.Serializable]
public class EffectModificator
{
    public EffectEnum effectType;
    public float mod;
}


