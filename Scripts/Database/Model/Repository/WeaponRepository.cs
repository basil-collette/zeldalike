using Assets.Database.Model.Design;

namespace Assets.Database.Model.Repository
{
    public sealed class WeaponRepository : ItemRepository<WeaponScriptable, Weapon>
    {
        public WeaponRepository() : base ("Weapon") { }

        public override Weapon DbDataToModel(WeaponScriptable data)
        {
            return new Weapon(data);
        }

        /*
        public override Weapon DbDataToModel(IDataReader reader)
        {
            return new Weapon(reader);
        }
        
        public override List<string> GetFields()
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

        public override string GetTableFields()
        {
            return "weapon_type VARCHAR(20)," +
                "attack_delay DECIMAL(3,2)," + //time in seconds
                "speed DECIMAL(3,2)," + //time in seconds
                base.GetTableFields();
        }

        public override void FillTable(SqliteConnection dbConn)
        {
            IDbCommand insertSword = dbConn.CreateCommand();
            insertSword.CommandText = $"INSERT INTO weapon ({GetQueryFields()}) VALUES ('sword', 1.5, 0.25, 'gfx/weapons/sword', 0.7, 'common', 0, 'Eppee poussiereuse', 'weapon', 'Epee', 'sword', 1)";
            insertSword.ExecuteNonQuery();

            IDbCommand insertAxe = dbConn.CreateCommand();
            insertAxe.CommandText = $"INSERT INTO weapon ({GetQueryFields()}) VALUES ('axe', 2, 0.5, 'gfx/weapons/axe', 0.7, 'common', 0, 'Hache viking', 'weapon', 'Axe', 'axe', 1)";
            insertAxe.ExecuteNonQuery();

            IDbCommand insertSpear = dbConn.CreateCommand();
            insertSpear.CommandText = $"INSERT INTO weapon ({GetQueryFields()}) VALUES ('spear', 2.2, 0.5, 'gfx/weapons/spear', 0.7, 'common', 0, 'Lance spartiate', 'weapon', 'Spear', 'spear', 1)";
            insertSpear.ExecuteNonQuery();

            IDbCommand insertDagger = dbConn.CreateCommand();
            insertDagger.CommandText = $"INSERT INTO weapon ({GetQueryFields()}) VALUES ('dagger', 0.75, 0.3, 'gfx/weapons/dagger', 0.7, 'common', 0, 'Dague perse', 'weapon', 'Dagger', 'dagger', 1)";
            insertDagger.ExecuteNonQuery();
        }
        */

    }
}
