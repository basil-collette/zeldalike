using Assets.Database.Model.Design;
using Assets.Database.Model.Repository;
using Assets.Scripts.Enums;
using System;

namespace Assets.Scripts.Manager
{
    public class ItemManager
    {
        public static Item GetItem(string itemParams) // dataTableName;itemName
        {
            string[] itemParamsArray = itemParams.Split(";");

            ItemTypeEnum type = (ItemTypeEnum)Enum.Parse(typeof(ItemTypeEnum), itemParamsArray[0]);
            string name = itemParamsArray[1];

            return GetItem(name, type);
        }

        public static Item GetItem(string itemName, ItemTypeEnum type)
        {
            switch (type)
            {
                case ItemTypeEnum.weapon:
                    return Singleton<WeaponRepository>.Instance.GetByCode(itemName);

                case ItemTypeEnum.item:
                    return Singleton<ItemRepository<Item>>.Instance.GetByCode(itemName);

                default: return null;
            }
        }

    }
}
