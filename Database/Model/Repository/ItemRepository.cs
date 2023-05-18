using Assets.Database.Model.Design;
using System.Collections.Generic;
using System.Data;

namespace Assets.Database.Model.Repository
{
    public sealed class ItemRepository : BaseRepository<ItemRepository, Item>
    {
        public sealed override Item DbDataToModel(IDataReader reader)
        {
            return new Item(reader);
        }

        public sealed override List<string> GetFields()
        {
            List<string> fields = new List<string>()
            {
                "name_libelle",
                "sprite_name",
                "rarity_code",
                "weight",
                "description",
                "item_type"
            };

            //Adding id, name_libelle, name_code, actif
            fields.AddRange(base.GetFields());

            return fields;
        }

    }
}
