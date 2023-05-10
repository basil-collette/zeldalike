using System.Data;

namespace Assets.Database.Model.Design
{
    public class BaseDbData
    {
        public int Id;
        public bool Actif;

        public BaseDbData(IDataReader reader)
        {
            Id = int.Parse(reader["id"].ToString());
            Actif = bool.Parse(reader["actif"].ToString());
        }

    }
}
