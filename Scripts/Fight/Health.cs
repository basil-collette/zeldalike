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
    public event Action<float> OnDammaged;

    bool canBeDammaged = true;

    private void Start()
    {
        //Volontairement vide, permet d'avoir le booleen active dans l'editeur
    }

    private void OnEnable()
    {
        canBeDammaged = true;
    }

    public override void Hit(GameObject attacker, List<Effect> hit, string attackerTag)
    {
        foreach (Effect effect in hit)
        {
            if (!canBeDammaged && effect.effectType != EffectTypeEnum.knockback) continue;

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
            case EffectTypeEnum.neutral:
            case EffectTypeEnum.bump:
            case EffectTypeEnum.slash:
            case EffectTypeEnum.pierce:
                Dammage(effect);
                break;

            case EffectTypeEnum.frost:
                float frostAmount = RaiseEffect(effect);
                if (IsEnaughtEffect(EffectTypeEnum.frost, frostAmount))
                    Freeze();
                break;

            case EffectTypeEnum.poison:
                CycleEffect(effect, 5f, 2f);
                break;

            case EffectTypeEnum.fire:
                float fireAmount = RaiseEffect(effect);
                if (IsEnaughtEffect(EffectTypeEnum.fire, fireAmount)) CycleEffect(effect, 3, 1);
                break;

            case EffectTypeEnum.knockback:
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

        if (hitSound != null)
        {
            MainGameManager._soundManager.PlayEffect(hitSound);
        }

        StartCoroutine(ColorDamageCo());

        if (_healthSignal != null)
        {
            _healthSignal.Raise();
        }

        OnDammaged?.Invoke(_health.RuntimeValue);

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

        MainGameManager._soundManager.PlayEffect("heal", 0.1f);

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

    protected virtual void CycleEffect(Effect effect, float durationSeconds, float cycleDuration = 1)
    {
        int index = _timedEffects.FindIndex(te => te.effectType == effect.effectType);
        if (index == -1)
        {
            _timedEffects.Add(effect);

            StartCoroutine(CycleEffectCo(effect.effectType, durationSeconds, cycleDuration));
        }
        else
        {
            _timedEffects[index].amount = durationSeconds;
        }
    }

    protected virtual IEnumerator CycleEffectCo(EffectTypeEnum effectType, float durationSeconds, float cycleDuration)
    {
        GameObject poisonParticlePrefab = GetParticleByEffectType(effectType);
        StartCoroutine(ShowParticleCo(poisonParticlePrefab));

        yield return new WaitForSeconds(1f);
        int index = _timedEffects.FindIndex(te => te.effectType == effectType);

        while (index != -1)
        {
            StartCoroutine(ShowParticleCo(poisonParticlePrefab));

            Dammage(_timedEffects[index]);

            durationSeconds--;

            if (durationSeconds <= 0)
            {
                _timedEffects.RemoveAt(index);
            }

            yield return new WaitForSeconds(cycleDuration);

            index = _timedEffects.FindIndex(te => te.effectType == effectType);
        }

        yield return null;
    }

    GameObject GetParticleByEffectType(EffectTypeEnum effectType)
    {
        string path = GetParticlePathByEffectType(effectType);

        return Resources.Load<GameObject>(path); ;
    }

    string GetParticlePathByEffectType(EffectTypeEnum effectType)
    {
        return effectType switch
        {
            EffectTypeEnum.poison => "Prefabs/Particle Systems/Poison Particle System",
            EffectTypeEnum.fire => "Prefabs/Particle Systems/Fire Particle System",
            _ => string.Empty
        };
    }

    IEnumerator ShowParticleCo(GameObject particlePrefab, bool infinite = false)
    {
        GameObject particleInstance = Instantiate(particlePrefab, transform);

        particlePrefab.transform.localScale = new Vector3(0.5f, 0.5f, 1);

        if (!infinite)
        {
            yield return new WaitForSeconds(0.8f);
            Destroy(particleInstance);
        }
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

    protected virtual bool IsEnaughtEffect(EffectTypeEnum effectType)
    {
        Effect ownedEffect = _effects.Find(effMod => effMod.effectType == effectType);

        return IsEnaughtEffect(effectType, ownedEffect.amount);
    }

    protected virtual bool IsEnaughtEffect(EffectTypeEnum effectType, float currentAmount)
    {
        return currentAmount >= GetEffectResistance(effectType);
    }

    protected virtual float GetEffectResistance(EffectTypeEnum effectType)
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
    public EffectTypeEnum effectType;
    public float mod;
}


