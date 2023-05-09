using Assets.Database.Model.Design;
using System.Data;

namespace Assets.Database.Model.Repository
{
    public sealed class ItemRepository : BaseRepository<ItemRepository, Item>
    {
        public override Item DbDataToModel(IDataReader reader)
        {
            return new Item(reader);
        }

        public override string GetFields()
        {
            return "sprite_name, rarity_code, weight, description";

            /*
            List<string> fields = new List<string>();

            foreach (PropertyInfo prop in typeof(D).GetProperties())
            {
                if (Nullable.GetUnderlyingType(prop.PropertyType) != null)
                {
                    fields.Add(Nullable.GetUnderlyingType(prop.PropertyType).ToString());
                    break;
                }

                fields.Add(Codify(prop.Name).ToLower());
            }

            return String.Join(",", fields);
            */
        }
    }
}
