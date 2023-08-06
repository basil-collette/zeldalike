using Assets.Database.Model.Design;
using Assets.Database.Model.Repository;
using Assets.Scripts.Enums;
using Assets.Scripts.Manager;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Hitable : Effectable
{
    public AudioClip hitSound;
    public AudioClip deathSound;
    public GameObject deathEffect;
    public LootTable lootTable;    

    public abstract void Hit(GameObject attacker, List<Effect> hit, string attackerTag);

    public virtual void Die()
    {
        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);

        if (lootTable != null)
            Drop();

        if (deathSound != null)
            FindGameObjectHelper.FindByName("Main Sound Manager").GetComponent<SoundManager>().PlayEffect(deathSound);

        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }

    public void Drop()
    {
        List<Loot> loots = lootTable.GetLootsByChanceWheel();

        if (loots.Count == 0) return;

        foreach (Loot loot in loots)
        {
            Drop drop;

            switch (loot.type)
            {
                case DropTypeEnum.weapon:
                    drop = Resources.Load<ItemDrop>($"Prefabs/Drop/ItemDrop");
                    (drop as ItemDrop).item = Singleton<WeaponRepository>.Instance.GetByCode(loot.lootCodeValue);
                    break;

                case DropTypeEnum.item:
                    drop = Resources.Load<ItemDrop>($"Prefabs/Drop/ItemDrop");
                    (drop as ItemDrop).item = Singleton<ItemRepository<Item>>.Instance.GetByCode(loot.lootCodeValue);
                    break;

                case DropTypeEnum.heart:
                    drop = Resources.Load<HealthDrop>($"Prefabs/Drop/HealthDrop");
                    (drop as HealthDrop).amount = int.Parse(loot.lootCodeValue);
                    break;

                case DropTypeEnum.experience:
                    drop = Resources.Load<ExperienceDrop>($"Prefabs/Drop/ExperienceDrop");
                    (drop as ExperienceDrop).amount = int.Parse(loot.lootCodeValue);
                    break;

                case DropTypeEnum.money:
                    drop = Resources.Load<MoneyDrop>($"Prefabs/Drop/MoneyDrop");
                    (drop as MoneyDrop).amount = int.Parse(loot.lootCodeValue);
                    break;

                default:
                    return;
            }

            Instantiate(drop, transform.position, Quaternion.identity);
        }
    }

}