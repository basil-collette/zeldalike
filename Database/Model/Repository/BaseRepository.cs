using Assets.Database.Model.Design;
using Assets.Scripts.Manager;
using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;

namespace Assets.Database.Model.Repository

{
    public abstract class BaseRepository<R, D>
        where R : BaseRepository<R, D>, new() /* repository child class, used fo Singleton */
        where D : BaseDbData /* D : data row class */
    {
        #region [Setup]

        private static R _instance;
        private static object _objLock = new object();

        public static R Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (_objLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new R();
                        }
                    }
                }

                return _instance;
            }
        }

        #endregion

        public List<D> GetAll(bool isNested = false)
        {
            try
            {
                List<D> rows = new List<D>();

                using (SqliteConnection connexion = DatabaseHelper.GetConnexion())
                {
                    var command = new SqliteCommand($"SELECT {Current.GetFields()} FROM {typeof(D).Name.ToLower()}", connexion);

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            rows.Add(DbDataToModel(reader));
                        }
                    }
                }

                return rows ?? new List<D>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public D GetBy<T>(string fieldName, T fieldValue, bool isNested = false)
        {
            try
            {
                using (var connexion = DatabaseHelper.GetConnexion())
                {
                    string query =
                        "SELECT " +
                            $"{Current.GetFields()} " +
                        $"FROM {typeof(D).Name.ToLower()} " +
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

        public D GetByMany<T>(Dictionary<string, T> fields, bool isNested = false)
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
                            $"{Current.GetFields()} " +
                        $"FROM {typeof(D).Name.ToLower()} " +
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

        public D GetByCode(string code, bool isNested = false)
        {
            try
            {
                using (var connexion = DatabaseHelper.GetConnexion())
                {
                    string query =
                        "SELECT " +
                            $"{string.Join(",", Current.GetFields())} " +
                        $"FROM {typeof(D).Name.ToLower()} " +
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

        public D GetById(int id, bool isNested = false)
        {
            try
            {
                using (var connexion = DatabaseHelper.GetConnexion())
                {
                    return ContextualGetById(connexion, id, isNested);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public D ContextualGetById(SqliteConnection dbcon, int id, bool isNested = false)
        {
            try
            {
                string query =
                    "SELECT " +
                        $"{Current.GetQueryFields()} " +
                    $"FROM {typeof(D).Name.ToLower()} " +
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

        public int Create(D model)
        {
            try
            {
                if (model == null) throw new Exception("");

                using (var connexion = DatabaseHelper.GetConnexion())
                {
                    //entityToPersist = PrePersist(entityToPersist, model);
                    //Validate(entityToPersist);

                    string query =
                        $"INSERT INTO {typeof(D).Name.ToLower()} " +
                        $"({Current.GetQueryFields()}) VALUES " +
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

        private int Update(D model)
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
                        $"UPDATE {typeof(D).Name.ToLower()} " +
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

        public abstract D DbDataToModel(IDataReader reader);

        public virtual List<string> GetFields()
        {
            return new List<string>()
            {
                "id",
                "actif"
            };
        }

        public string GetQueryFields()
        {
            List<string> finalFields = GetFields();
            finalFields.Remove("Id");

            return string.Join(",", finalFields);
        }

        public List<string> GetFieldsValues(D model)
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

        public string GetFieldValue(string fieldName, D model)
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

        /*
        public abstract void Validate(D entity);

        public override string GetFields()
        {
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
         }

        */

    }
}
