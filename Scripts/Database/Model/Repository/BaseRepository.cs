using Assets.Database.Model.Design;
using Assets.Scripts.Manager;
using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;

namespace Assets.Database.Model.Repository
{
    public abstract class BaseRepository<T> : IReadRepository<T>, IWriteRepository<T>
        where T : BaseDbData /* T : data row class */
    {
        public List<T> GetAll(bool isNested = false)
        {
            try
            {
                List<T> rows = new List<T>();

                using (SqliteConnection connexion = DatabaseHelper.GetConnexion())
                {
                    var command = new SqliteCommand($"SELECT {string.Join(",", GetFields())} FROM {typeof(T).Name.ToLower()}", connexion);

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            rows.Add(DbDataToModel(reader));
                        }
                    }
                }

                return rows ?? new List<T>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public T GetBy(string fieldName, T fieldValue, bool isNested = false)
        {
            try
            {
                using (var connexion = DatabaseHelper.GetConnexion())
                {
                    string query =
                        "SELECT " +
                            $"{string.Join(",", GetFields())} " +
                        $"FROM {typeof(T).Name.ToLower()} " +
                        $"WHERE {fieldName} = @Param AND actif = 1 " +
                            "LIMIT 1";

                    var command = new SqliteCommand(query, connexion);
                    command.Parameters.AddWithValue("Param", fieldValue);

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return DbDataToModel(reader);
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public T GetByMany(Dictionary<string, T> fields, bool isNested = false)
        {
            try
            {
                List<string> whereClauses = new List<string>();
                foreach (KeyValuePair<string, T> field in fields)
                {
                    whereClauses.Add($"{field.Key} = {field.Value}");
                }

                using (var connexion = DatabaseHelper.GetConnexion())
                {
                    string query =
                        "SELECT " +
                            $"{string.Join(",", GetFields())} " +
                        $"FROM {typeof(T).Name.ToLower()} " +
                        $"WHERE {string.Join(" AND ", whereClauses)} AND actif = 1 " +
                            "LIMIT 1";

                    var command = new SqliteCommand(query, connexion);

                    //command.Parameters.AddRange(parameters);

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return DbDataToModel(reader);
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public T GetByCode(string code, bool isNested = false)
        {
            try
            {
                using (var connexion = DatabaseHelper.GetConnexion())
                {
                    string query =
                        "SELECT " +
                            $"{string.Join(",", GetFields())} " +
                        $"FROM {typeof(T).Name.ToLower()} " +
                        "WHERE name_code = @Code AND actif = 1 " +
                            "LIMIT 1";

                    var command = new SqliteCommand(query, connexion);
                    command.Parameters.AddWithValue("Code", code);

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return DbDataToModel(reader);
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public T GetById(int id, bool isNested = false)
        {
            try
            {
                using (var connexion = DatabaseHelper.GetConnexion())
                {
                    return ContextualGetById(connexion, id, isNested);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public virtual T ContextualGetById(SqliteConnection dbcon, int id, bool isNested = false)
        {
            try
            {
                string query =
                    "SELECT " +
                        $"{GetQueryFields()} " +
                    $"FROM {typeof(T).Name.ToLower()} " +
                    "WHERE id = @Id AND actif = 1 " +
                        "LIMIT 1";

                var command = new SqliteCommand(query, dbcon);
                command.Parameters.AddWithValue("Id", id);

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return DbDataToModel(reader);
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int Create(T model)
        {
            try
            {
                if (model == null) throw new Exception("");

                using (var connexion = DatabaseHelper.GetConnexion())
                {
                    //entityToPersist = PrePersist(entityToPersist, model);
                    //Validate(entityToPersist);

                    string query =
                        $"INSERT INTO {typeof(T).Name.ToLower()} " +
                        $"({GetQueryFields()}) VALUES " +
                        $"({string.Join(",", GetFieldsValues(model))})";

                    var command = new SqliteCommand(query, connexion);

                    IDataReader reader = command.ExecuteReader();

                    return (int)new SqliteCommand("SELECT last_insert_rowid()", connexion).ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Update(T model)
        {
            try
            {
                if (model == null) throw new Exception("");

                List<string> fields = GetFields();
                fields.Remove("Id");

                List<string> updates = new List<string>();
                foreach (string field in fields)
                {
                    updates.Add($"{field} = {GetFieldValue(field, model)}");
                }

                using (var connexion = DatabaseHelper.GetConnexion())
                {
                    //entityToPersist = PrePersist(entityToPersist, model);
                    //Validate(entityToPersist);

                    string query =
                        $"UPDATE {typeof(T).Name.ToLower()} " +
                        $"SET {string.Join("", updates)} " +
                        $"WHERE id = {model.Id}";

                    var command = new SqliteCommand(query, connexion);

                    /*
                    foreach (string field in fields)
                    {
                        command.Parameters.AddWithValue($"@{field}", GetFieldValue(field, model));
                    }
                    */

                    IDataReader reader = command.ExecuteReader();

                    return model.Id;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public abstract T DbDataToModel(IDataReader reader);

        public virtual List<string> GetFields()
        {
            return new List<string>()
            {
                "id",
                "name_libelle",
                "name_code",
                "actif"
            };
        }

        public virtual string GetQueryFields()
        {
            List<string> finalFields = GetFields();
            finalFields.Remove("id");

            return string.Join(",", finalFields);
        }

        public List<string> GetFieldsValues(T model)
        {
            List<string> values = new List<string>();

            List<string> fields = GetFields();
            fields.Remove("id");
            foreach (string fieldName in fields)
            {
                var value = typeof(Item).GetProperty(fieldName).GetValue(model);
                Type valueType = value.GetType();

                switch (valueType.Name)
                {
                    case "string":
                        values.Add($"'{value}'");
                        break;

                    case "int":
                        values.Add($"{value}");
                        break;

                    default:
                        break;
                }
            }

            return values;
        }

        public string GetFieldValue(string fieldName, T model)
        {
            var value = typeof(Item).GetProperty(fieldName).GetValue(model);
            Type valueType = value.GetType();

            switch (valueType.Name)
            {
                case "string":
                    return $"'{value}'";

                case "int":
                    return $"{value}";

                default:
                    break;
            }

            return null;
        }

        public virtual string GetTableFields()
        {
            return "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                "name_libelle VARCHAR(100) UNIQUE," +
                "name_code VARCHAR(100) UNIQUE," +
                "actif BOOLEAN";
        }

        public void ResetTable()
        {
            using (var dbConn = DatabaseHelper.GetConnexion())
            {
                IDbCommand dbcmd2 = dbConn.CreateCommand();
                dbcmd2.CommandText = $"DROP TABLE IF EXISTS {typeof(T).Name.ToLower()}";
                dbcmd2.ExecuteNonQuery();

                IDbCommand dbcmd = dbConn.CreateCommand();
                dbcmd.CommandText = $"CREATE TABLE IF NOT EXISTS {typeof(T).Name.ToLower()} (" +
                    GetTableFields() +
                ")";
                dbcmd.ExecuteNonQuery();

                FillTable(dbConn);
            }
        }

        public abstract void FillTable(SqliteConnection dbConn);

        public void Delete()
        {
            using (var dbConn = DatabaseHelper.GetConnexion())
            {
                IDbCommand drop_cmd = dbConn.CreateCommand();
                drop_cmd.CommandText = $"DELETE FROM {typeof(T).Name.ToLower()}";
                drop_cmd.ExecuteNonQuery();
            }                
        }

        /*
        public abstract void Validate(D entity);
        */

    }

    public interface IReadRepository<T> where T : BaseDbData /* D : data row class */
    {
        List<T> GetAll(bool isNested = false);
        T GetByCode(string code, bool isNested = false);
        T GetById(int id, bool isNested = false);
        T DbDataToModel(IDataReader reader);
    }

    public interface IWriteRepository<T> where T : BaseDbData /* D : data row class */
    {

        int Create(T model);
        int Update(T model);
        void Delete();
        T DbDataToModel(IDataReader reader);
    }

}
