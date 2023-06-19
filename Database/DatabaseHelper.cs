using Mono.Data.Sqlite;
using System.Data;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    public static class DatabaseHelper
	{
		//exemples:
		//Debug.Log(GetAll<ItemRepository, Item>().ElementAt(0).NameCode);
		//Debug.Log(ItemRepository.Current.GetByCode("test_code").NameLibelle);

		public static SqliteConnection GetConnexion()
		{
			string databasePath = "URI=file:" + Application.persistentDataPath + "/portfolio_database";

			SqliteConnection dbcon = new SqliteConnection(databasePath);
			dbcon.Open();

			return dbcon;
		}

		public static void CreateTables()
		{
			ResetTableItem();
		}

		public static void ResetTableItem()
        {
			SqliteConnection dbConn = GetConnexion();

			IDbCommand dbcmd2 = dbConn.CreateCommand();
			dbcmd2.CommandText = "DROP TABLE IF EXISTS item";
			dbcmd2.ExecuteNonQuery();

			IDbCommand dbcmd = dbConn.CreateCommand();
			dbcmd.CommandText = "CREATE TABLE IF NOT EXISTS item (" +
				"id INTEGER PRIMARY KEY AUTOINCREMENT," +
				"name_libelle VARCHAR(100) UNIQUE," +
				"name_code VARCHAR(100) UNIQUE," +
				"sprite_name VARCHAR(100)," +
				"rarity_code VARCHAR(20)," +
				"weight DECIMAL(3,2)," +
				"description VARCHAR(255)," +
				"item_type VARCHAR(20)," +
				"actif BOOLEAN" +
			")";
			dbcmd.ExecuteNonQuery();

			/*
			IDbCommand drop_cmd = dbConn.CreateCommand();
			drop_cmd.CommandText = "DELETE FROM item";
			drop_cmd.ExecuteNonQuery();
			*/

			IDbCommand key = dbConn.CreateCommand();
			key.CommandText = "INSERT INTO item (name_libelle, name_code, sprite_name, rarity_code, weight, description, item_type, actif) VALUES ('Clé', 'key', 'gfx/key', 'common', 0, 'Petite Clé', 'consommable', 1)";
			key.ExecuteNonQuery();

			IDbCommand letter = dbConn.CreateCommand();
			letter.CommandText = "INSERT INTO item (name_libelle, name_code, sprite_name, rarity_code, weight, description, item_type, actif) VALUES ('Lettre', 'letter', 'gfx/letter', 'common', 0, 'Lettre adressée à Mathilde', 'item', 1)";
			letter.ExecuteNonQuery();

			IDbCommand apple = dbConn.CreateCommand();
			apple.CommandText = "INSERT INTO item (name_libelle, name_code, sprite_name, rarity_code, weight, description, item_type, actif) VALUES ('Pomme', 'apple', 'gfx/apple', 'common', 0, 'Pomme juteuse', 'consommable', 1)";
			apple.ExecuteNonQuery();
		}

	}
}
