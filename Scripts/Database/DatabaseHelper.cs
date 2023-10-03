using Assets.Database.Model.Design;
using Assets.Database.Model.Repository;
using Mono.Data.Sqlite;
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

		public static void ResetTables()
		{
			Singleton<ItemRepository<Item>>.Instance.ResetTable();
			Singleton<WeaponRepository>.Instance.ResetTable();

			//var items = Singleton<ItemRepository<Item>>.Instance.GetAll();
		}

		public static bool DbExists()
		{
			using (SqliteConnection connexion = DatabaseHelper.GetConnexion())
			{
				string query = "SELECT name FROM sqlite_master WHERE type='table' AND (name='item' OR name='weapon');";
				var command = new SqliteCommand(query, connexion);
				command.ExecuteNonQuery();
				var result = command.ExecuteReader();
				if (result.HasRows)
				{
					return true;
				}
			}

			return false;
		}

	}
}
