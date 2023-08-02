using System;
using System.Data;

namespace Assets.Database.Model.Design
{
    [Serializable]
    public class BaseDbData
    {
        public int Id;
        public Guid? Uid;
        public string NameLibelle;
        public string NameCode;
        public bool Actif;

        public BaseDbData(IDataReader reader, Guid? uid = null)
        {
            Id = int.Parse(reader["id"].ToString());
            Uid = uid;
            NameLibelle = reader["name_libelle"].ToString();
            NameCode = reader["name_code"].ToString();
            Actif = bool.Parse(reader["actif"].ToString());
        }

        public BaseDbData() : base()
        {

        }

    }
}
