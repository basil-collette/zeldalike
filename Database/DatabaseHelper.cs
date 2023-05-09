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
			CreateTableItem();
		}

		public static void CreateTableItem()
		{
			/*
			IDbCommand dbcmd2 = dbcon.CreateCommand();
			dbcmd2.CommandText = "DROP TABLE item";
			dbcmd2.ExecuteNonQuery();
			*/
			IDbCommand dbcmd = GetConnexion().CreateCommand();
			dbcmd.CommandText = "CREATE TABLE IF NOT EXISTS item (" +
				"id INTEGER PRIMARY KEY AUTOINCREMENT," +
				"name_libelle VARCHAR(100) UNIQUE," +
				"name_code VARCHAR(100) UNIQUE," +
				"sprite_name VARCHAR(100)," +
				"rarity_code VARCHAR(20)," +
				"weight DECIMAL(3,2)," +
				"description VARCHAR(255)," +
				"actif BOOLEAN" +
			")";
			dbcmd.ExecuteNonQuery();

			/*
			IDbCommand drop_cmd = dbcon.CreateCommand();
			drop_cmd.CommandText = "DELETE FROM item";
			drop_cmd.ExecuteNonQuery();
			
			IDbCommand cmnd = dbcon.CreateCommand();
			cmnd.CommandText = "INSERT INTO item (name_libelle, name_code, sprite_name, rarity_code, weight, description, actif) VALUES ('Test', 'test_code', 'testSprite', 'common', 111.11, 'description', 1)";
			cmnd.ExecuteNonQuery();
			*/
		}

	}
}
