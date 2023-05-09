using System.Data;

namespace Assets.Database.Model.Design
{
    public class BaseDbData
    {
        public int Id;
        public string NameLibelle;
        public string NameCode;
        public bool Actif;

        public BaseDbData(IDataReader reader)
        {
            Id = int.Parse(reader["id"].ToString());
            NameLibelle = reader["name_libelle"].ToString();
            NameCode = reader["name_code"].ToString();
            Actif = bool.Parse(reader["actif"].ToString());
        }

    }
}
