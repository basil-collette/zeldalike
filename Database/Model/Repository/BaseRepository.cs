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
                    var command = new SqliteCommand($"SELECT id, name_libelle, name_code, {Current.GetFields()}, actif FROM {typeof(D).Name.ToLower()}", connexion);

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
                    var command = new SqliteCommand($"SELECT id, name_libelle, name_code, {Current.GetFields()}, actif FROM {typeof(D).Name.ToLower()} WHERE {fieldName} = @Param AND actif = 1 LIMIT 1", connexion);
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

        public D GetByCode(string code, bool isNested = false)
        {
            try
            {
                using (var connexion = DatabaseHelper.GetConnexion())
                {
                    var command = new SqliteCommand($"SELECT id, name_libelle, name_code, {Current.GetFields()}, actif FROM {typeof(D).Name.ToLower()} WHERE name_code = @Code AND actif = 1 LIMIT 1", connexion);
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
                    var command = new SqliteCommand($"SELECT id, name_libelle, name_code, {Current.GetFields()}, actif FROM {typeof(D).Name.ToLower()} WHERE id = @Id AND actif = 1 LIMIT 1", connexion);
                    command.Parameters.AddWithValue("Id", id);

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

        /*
        public D ContextualGetById(Db_ICareContext context, int idEntity, bool isNested = false)
        {
            try
            {
                return ((isNested) ? Include(context.Set<D>()) : context.Set<D>())
                        .FirstOrDefault(e => IsExactly(e, idEntity));
            }
            catch (Exception ex)
            {
                if (ex is ICareException) { throw ex; }
                LogManager.Current.Error(GetType().Name, "ContextualGetById(Db_ICareContext context, int idEntity, bool isNested = false)",
                    ex.Message, ex, new object[] { context, idEntity, isNested });
                throw new ICareException(ICareExceptionEnum.default_get_sub_err.ToString());
            }
        }

        #endregion

        public int CreateOrUpdate(D model, OperationTypeEnum operation, bool getOutput = false)
        {
            try
            {
                if (model == null)
                {
                    var message = CommonExceptionEnum.param_not_nullable_null.ToString();
                    LogManager.Current.Error(GetType().Name, "CreateOrUpdate(M model, OperationTypeEnum operation, bool getOutput = false)",
                        message, new ICareException(), new object[] { model, operation, getOutput });
                    throw new ICareException(message);
                }

                var dbResponse = (getOutput) ? new OutputParameter<short?>() : null;

                using (var context = new Db_ICareContext())
                {
                    int identifier = GetId(model);

                    D entityToPersist = (operation == OperationTypeEnum.Update) ?
                         ContextualGetById(context, identifier, false)
                         : new D() { };

                    entityToPersist = PrePersist(entityToPersist, model, operation);

                    Validate(entityToPersist);

                    CreateOrUpdateOperation(context, entityToPersist, dbResponse, operation);

                    context.SaveChanges();

                    if (!string.IsNullOrEmpty(_Cache)) ClearCache();

                    if (getOutput)
                    {
                        if (dbResponse == null || dbResponse.Value == default || !dbResponse.Value.HasValue || dbResponse.Value.Value == default || dbResponse.Value == 0 || dbResponse.Value.Value == 0)
                        {
                            var message = (operation == OperationTypeEnum.Create ? CommonExceptionEnum.insert_item_failed : CommonExceptionEnum.update_item_failed).ToString();
                            LogManager.Current.Error(GetType().Name, "CreateOrUpdate(M model, OperationTypeEnum operation, bool getOutput = false)",
                                message, new ICareException(), new object[] { model, operation, getOutput });
                            throw new ICareException(message);
                        }

                        return dbResponse.Value.Value; //output renvoyé par le SGBD
                    }

                    return GetKey(context, entityToPersist);
                }
            }
            catch (Exception ex)
            {
                if (ex is ICareException) { throw ex; }

                // Erreur renvoyé par le SGBD
                if (ex is AggregateException)
                {
                    string concreteErrorMessage = ex.InnerException.Message;

                    LogManager.Current.Error(GetType().Name, "CreateOrUpdate(M model, OperationTypeEnum operation, bool getOutput = false)", concreteErrorMessage, ex, new object[] { model, operation, getOutput });
                    throw new ICareException(concreteErrorMessage);
                }

                String default_error = (operation == OperationTypeEnum.Create) ? ICareExceptionEnum.default_create_sub_err.ToString() : ICareExceptionEnum.default_update_sub_err.ToString();

                LogManager.Current.Error(GetType().Name, "CreateOrUpdate(M model, OperationTypeEnum operation, bool getOutput = false)", ex.Message, ex, new object[] { model, operation, getOutput });
                throw new ICareException(default_error);
            }
        }

        public int Delete(int idEntity, bool getOutput = false)
        {
            try
            {
                var dbResponse = (getOutput) ? new OutputParameter<short?>() : null;

                D entityToDelete;

                using (var context = new Db_ICareContext())
                {
                    entityToDelete = ContextualGetById(context, idEntity, false);

                    if (entityToDelete == null)
                    {
                        LogManager.Current.Error(GetType().Name, "Delete(int idEntity,bool getOutput = false)", ICareExceptionEnum.id_not_found.ToString(), null, new object[] { idEntity, getOutput });
                        throw new ICareException(ICareExceptionEnum.id_not_found.ToString());
                    }

                    DeleteOperation(context, entityToDelete, dbResponse);

                    context.SaveChanges();

                    if (!string.IsNullOrEmpty(_Cache)) ClearCache();

                    if (getOutput)
                    {
                        if (dbResponse == null || dbResponse.Value == default || !dbResponse.Value.HasValue || dbResponse.Value.Value == default || dbResponse.Value == 0 || dbResponse.Value.Value == 0)
                        {
                            LogManager.Current.Error(GetType().Name, "Delete(int idEntity, bool getOutput = false)",
                                "", new ICareException(), new object[] { idEntity, getOutput });
                            throw new ICareException("");
                        }

                        return dbResponse.Value.Value; //output renvoyé par le SGBD
                    }

                    return GetKey(context, entityToDelete);
                }
            }
            catch (Exception ex)
            {
                if (ex is ICareException) { throw ex; }
                LogManager.Current.Error(GetType().Name, "Delete(int idEntity, bool getOutput = false)", ex.Message, ex, new object[] { idEntity, getOutput });
                throw new ICareException(ICareExceptionEnum.default_delete_sub_err.ToString());
            }
        }

        #region [Abstract]

        public abstract bool IsActif(D data);

        public abstract bool IsExactly(D data, int idEntity);

        protected abstract int GetId(D model);

        
        /// <summary>
        /// Controle la présence et le type des attributs de l'entité avant qu'elle ne soit persisté en base
        /// </summary>
        /// <param name="entity"></param>
        public abstract void Validate(D entity);

        protected abstract void DeleteOperation(Db_ICareContext context, D entity, OutputParameter<short?> outputResult);

        #endregion

        #region [Utils]

        protected virtual void CreateOrUpdateOperation(Db_ICareContext context, D entity, OutputParameter<short?> outputResult, OperationTypeEnum operation)
        {
            if (operation == OperationTypeEnum.Create)
            {
                context.Set<D>().Add(entity);
            }
            else if (operation == OperationTypeEnum.Update)
            {
                context.Set<D>().Update(entity);
            }
        }

        protected virtual void DeleteOperation(Db_ICareContext context, D entity, OutputParameter<short?> outputResult)
        {
            context.Set<D>().Remove(entity);
        }

        #endregion

        void Test()
        {
			//IDbCommand cmnd = dbcon.CreateCommand();
			//cmnd.CommandText = "INSERT INTO my_table (id, val) VALUES (0, 5)";
			//cmnd.ExecuteNonQuery();

            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                var value = reader.GetInt32(1);

                Debug.Log("id: " + reader["id"].ToString());
                Debug.Log("val: " + reader["val"].ToString());
            }
        }
        */

        public abstract D DbDataToModel(IDataReader reader);

        public abstract string GetFields();

        /*
        string Codify(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";

            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);

            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                    newText.Append(' ');
                newText.Append(text[i]);
            }

            return newText.ToString();
        }
        */

    }
}
