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
            FindAnyObjectByType<SoundManager>().PlayEffect(deathSound);

        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }

    public void Drop()
    {
        List<Loot> loots = lootTable.GetLootsByChanceWheel();

        if (loots.Count == 0) return;

        foreach (Loot loot in loots)
        {
            Drop drop = null;

            if (loot is DefaultLoot)
            {
                var currentLoot = (loot as DefaultLoot);
                switch (currentLoot.Type)
                {
                    case DropTypeEnum.heart:
                        drop = Resources.Load<HealthDrop>($"Prefabs/Drop/HealthDrop");
                        (drop as HealthDrop).amount = currentLoot.Amount;
                        break;

                    case DropTypeEnum.experience:
                        drop = Resources.Load<ExperienceDrop>($"Prefabs/Drop/ExperienceDrop");
                        (drop as ExperienceDrop).amount = currentLoot.Amount;
                        break;

                    case DropTypeEnum.money:
                        drop = Resources.Load<MoneyDrop>($"Prefabs/Drop/MoneyDrop");
                        (drop as MoneyDrop).amount = currentLoot.Amount;
                        break;

                    default: return;
                }
            }
            else if (loot is ItemLoot)
            {
                var currentLoot = (loot as ItemLoot);
                switch ((currentLoot as ItemLoot).Type)
                {
                    case ItemTypeEnum.weapon:
                        drop = Resources.Load<ItemDrop>($"Prefabs/Drop/ItemDrop");
                        (drop as ItemDrop).itemNameCode = currentLoot.NameCode;
                        (drop as ItemDrop).itemType = ItemTypeEnum.weapon;
                        break;

                    case ItemTypeEnum.item:
                        drop = Resources.Load<ItemDrop>($"Prefabs/Drop/ItemDrop");
                        (drop as ItemDrop).itemNameCode = currentLoot.NameCode;
                        (drop as ItemDrop).itemType = ItemTypeEnum.item;
                        break;

                    default: return;
                }
            }
            
            Instantiate(drop, transform.position, Quaternion.identity);
        }
    }

}