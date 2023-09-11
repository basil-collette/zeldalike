using Assets.Database.Model.Design;
using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;

namespace Assets.Database.Model.Repository
{
    public class ItemRepository<T> : BaseRepository<T> where T : Item
    {
        public override T DbDataToModel(IDataReader reader)
        {
            return (T)Activator.CreateInstance(typeof(T), reader);
        }

        public override List<string> GetFields()
        {
            List<string> fields = new List<string>()
            {
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

        public override string GetTableFields()
        {
            return "sprite_name VARCHAR(100)," +
                "rarity_code VARCHAR(20)," +
                "weight DECIMAL(3,2)," +
                "description VARCHAR(255)," +
                "item_type VARCHAR(20)," +
                base.GetTableFields();
        }

        public override void Insert(SqliteConnection dbConn)
        {
            IDbCommand key = dbConn.CreateCommand();
            key.CommandText = $"INSERT INTO item ({GetQueryFields()}) VALUES ('gfx/items/key', 'common', 0, 'Petite Clé', 'consommable', 'Clé', 'key', 1)";
            key.ExecuteNonQuery();

            IDbCommand letter = dbConn.CreateCommand();
            letter.CommandText = $"INSERT INTO item ({GetQueryFields()}) VALUES ('gfx/items/letter', 'common', 0, 'Lettre adressée à Mathilde', 'item', 'Lettre', 'letter', 1)";
            letter.ExecuteNonQuery();

            IDbCommand arrosoir = dbConn.CreateCommand();
            arrosoir.CommandText = $"INSERT INTO item ({GetQueryFields()}) VALUES ('gfx/items/arrosoir', 'common', 0, 'Arrosoir de Clara', 'quest', 'Arrosoir', 'arrosoir', 1)";
            arrosoir.ExecuteNonQuery();

            IDbCommand idCard = dbConn.CreateCommand();
            idCard.CommandText = $"INSERT INTO item ({GetQueryFields()}) VALUES ('gfx/items/id_card', 'common', 0, 'Ma carte d''identité', 'quest', 'Carte d''identité', 'id_card', 1)";
            idCard.ExecuteNonQuery();
        }

    }
}
