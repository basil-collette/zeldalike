using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
class HealthEffect
{
    public HealthState healthState = HealthState.neutral;
    public List<HealthState> healthStateImunnities = new List<HealthState>();
    public List<EffectModificator> effectMods = new List<EffectModificator>();
    public List<Effect> effects;

    protected virtual void SetHealthState(HealthState newHealthState)
    {
        if (this.healthState != newHealthState
            && !healthStateImunnities.Contains(newHealthState))
        {
            this.healthState = newHealthState;
        }
    }

    async void CycleEffect(Effect effect, int durationSeconds = 5)
    {
        int i = 0;
        while (i < durationSeconds
            && effects.Exists(eff => eff.effectType == effect.effectType))
        {
            Dammage(effect);
            new WaitForSeconds(1f);
            i++;
        }
    }

    void RaiseEffect(Effect effect)
    {
        int index = AddEffect(effect);
        if (index != -1)
        {
            effects[index].ammount += effect.ammount;
        }
    }

    int AddEffect(Effect effect)
    {
        int index = effectMods.FindIndex(effMod => effMod.effectType == effect.effectType);
        if (index == -1)
        {
            effects.Add(effect);
        }
        return index;
    }

    void RemoveEffect(Effect effectToRemove)
    {
        Effect effect = effects.Find(effMod => effMod.effectType == effectToRemove.effectType);
        if (effect != null)
        {
            effects.Remove(effect);
        }
    }
}
*/