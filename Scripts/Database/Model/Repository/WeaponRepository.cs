using Assets.Database.Model.Design;
using Mono.Data.Sqlite;
using System.Collections.Generic;
using System.Data;

namespace Assets.Database.Model.Repository
{
    public sealed class WeaponRepository : ItemRepository<Weapon>
    {
        public sealed override Weapon DbDataToModel(IDataReader reader)
        {
            return new Weapon(reader);
        }

        public sealed override List<string> GetFields()
        {
            List<string> fields = new List<string>()
            {
                "weapon_type",
                "attack_delay",
                "speed"
            };

            //Adding id, name_libelle, name_code, actif
            fields.AddRange(base.GetFields());

            return fields;
        }

        public sealed override string GetTableFields()
        {
            return "weapon_type VARCHAR(20)," +
                "attack_delay DECIMAL(3,2)," +
                "speed DECIMAL(3,2)," +
                base.GetTableFields();
        }

        public sealed override void Insert(SqliteConnection dbConn)
        {
            IDbCommand insertSword = dbConn.CreateCommand();
            insertSword.CommandText = $"INSERT INTO weapon ({GetQueryFields()}) VALUES ('sword', 1.5, 1, 'gfx/weapons/sword', 0.7, 'common', 0, 'Eppee poussiereuse', 'weapon', 'Epee', 'sword', 1)";
            insertSword.ExecuteNonQuery();

            IDbCommand insertAxe = dbConn.CreateCommand();
            insertAxe.CommandText = $"INSERT INTO weapon ({GetQueryFields()}) VALUES ('axe', 1.5, 1, 'gfx/weapons/axe', 0.7, 'common', 0, 'Hache viking', 'weapon', 'Axe', 'axe', 1)";
            insertAxe.ExecuteNonQuery();
        }

    }
}
