using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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

        public void StartCooldown(string name, float time, Action OnEnd = null)
        {
            if (IsAvailable(name))
            {
                StartCoroutine(CooldownCo(name, time, OnEnd));
            }
        }

        public void StartOrIncreaseCooldown(string name, float time, Action OnEnd = null)
        {
            if (IsAvailable(name))
            {
                StartCoroutine(CooldownCo(name, time, OnEnd));
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

        IEnumerator CooldownCo(string name, float time, Action OnEnd = null)
        {
            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();

            var newCooldown = new Cooldown()
            {
                Name = name,
                Time = time
            };

            Cooldowns.Add(newCooldown);

            yield return new WaitForSeconds(time);

            //stopwatch.Stop();
            //UnityEngine.Debug.Log(name + " : " + stopwatch.Elapsed.TotalSeconds);

            Cooldowns.Remove(newCooldown);

            OnEnd?.Invoke();
        }

    }
}
