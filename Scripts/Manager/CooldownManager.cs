using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    public class Cooldown
    {
        public string Name;
        public float Time;
    }

    public class CooldownManager : MonoBehaviour
    {
        List<Cooldown> Cooldowns = new List<Cooldown>();

        public bool IsAvailable(string name)
        {
            return !Cooldowns.Any(item => item.Name == name);
        }

        public void StartCooldown(string name, float time, Action OnLoop = null, Action OnEnd = null)
        {
            if (IsAvailable(name))
            {
                StartCoroutine(CooldownCo(name, time, OnLoop, OnEnd));
            }
        }

        public void StartOrIncreaseCooldown(string name, float time, Action OnLoop = null, Action OnEnd = null)
        {
            if (IsAvailable(name))
            {
                StartCoroutine(CooldownCo(name, time, OnLoop, OnEnd));
            }
            else
            {
                var cooldown = GetCooldown(name);
                cooldown.Time = time;
            }
        }

        public Cooldown GetCooldown(string name)
        {
            return Cooldowns.Where(n => n.Name == name).FirstOrDefault();
        }

        IEnumerator CooldownCo(string name, float time, Action OnLoop = null, Action OnEnd = null)
        {
            var newCooldown = new Cooldown()
            {
                Name = name,
                Time = time
            };

            Cooldowns.Add(newCooldown);

            var cooldown = GetCooldown(name);
            while (cooldown != null)
            {
                OnLoop?.Invoke();

                cooldown.Time -= Time.deltaTime;

                if (cooldown.Time <= 0)
                {
                    Cooldowns.Remove(cooldown);
                }

                cooldown = GetCooldown(name);

                yield return null;
            }

            OnEnd?.Invoke();
        }

    }
}
