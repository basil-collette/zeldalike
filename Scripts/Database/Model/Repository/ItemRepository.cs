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
                "sprite_path",
                "sprite_scale",
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
            return "sprite_path VARCHAR(100)," +
                "sprite_scale FLOAT," +
                "rarity_code VARCHAR(20)," +
                "weight DECIMAL(3,2)," +
                "description VARCHAR(255)," +
                "item_type VARCHAR(20)," +
                base.GetTableFields();
        }

        public override void Insert(SqliteConnection dbConn)
        {
            IDbCommand key = dbConn.CreateCommand();
            key.CommandText = $"INSERT INTO item ({GetQueryFields()}) VALUES ('gfx/items/key', 1, 'common', 0, 'Petite Clé', 'consommable', 'Clé', 'key', 1)";
            key.ExecuteNonQuery();

            /*
            IDbCommand letter = dbConn.CreateCommand();
            letter.CommandText = $"INSERT INTO item ({GetQueryFields()}) VALUES ('gfx/items/letter', 1, 'rare', 0, 'Lettre adressée à Mathilde', 'item', 'Lettre', 'letter', 1)";
            letter.ExecuteNonQuery();
            */

            IDbCommand heart = dbConn.CreateCommand();
            heart.CommandText = $"INSERT INTO item ({GetQueryFields()}) VALUES ('gfx/items/heart', 1, 'rare', 0, 'Une vie supplémentaire !', 'consommable', 'Heart', 'heart', 1)";
            heart.ExecuteNonQuery();

            IDbCommand arrosoir = dbConn.CreateCommand();
            arrosoir.CommandText = $"INSERT INTO item ({GetQueryFields()}) VALUES ('gfx/items/arrosoir', 1, 'common', 0, 'Arrosoir de Clara', 'quest', 'Arrosoir', 'arrosoir', 1)";
            arrosoir.ExecuteNonQuery();

            IDbCommand idCard = dbConn.CreateCommand();
            idCard.CommandText = $"INSERT INTO item ({GetQueryFields()}) VALUES ('gfx/items/id_card', 1, 'common', 0, 'Ma carte d''identité', 'quest', 'Carte d''identité', 'id_card', 1)";
            idCard.ExecuteNonQuery();

            IDbCommand diplomas = dbConn.CreateCommand();
            diplomas.CommandText = $"INSERT INTO item ({GetQueryFields()}) VALUES ('gfx/items/diplomas', 1, 'common', 0, 'Mes diplômes !', 'quest', 'Diplômes', 'diplomas', 1)";
            diplomas.ExecuteNonQuery();

            IDbCommand shoes = dbConn.CreateCommand();
            shoes.CommandText = $"INSERT INTO item ({GetQueryFields()}) VALUES ('gfx/items/shoes', 1, 'common', 0, 'Bottes du marchant', 'quest', 'Bottes du marchant', 'shoes', 1)";
            shoes.ExecuteNonQuery();

            IDbCommand rope = dbConn.CreateCommand();
            rope.CommandText = $"INSERT INTO item ({GetQueryFields()}) VALUES ('gfx/items/rope', 1, 'rare', 0, 'Corde solide', 'quest', 'Corde solide', 'rope', 1)";
            rope.ExecuteNonQuery();

            IDbCommand darkWoodPlank = dbConn.CreateCommand();
            darkWoodPlank.CommandText = $"INSERT INTO item ({GetQueryFields()}) VALUES ('gfx/items/dark_wood_plank', 1, 'common', 0, 'Planche d''Ecorcique', 'item', 'Planche d''Ecorcique', 'dark_wood_plank', 1)";
            darkWoodPlank.ExecuteNonQuery();
        }

    }
}
