using Assets.Database.Model.Design;
using System;

namespace Assets.Database.Model.Repository
{
    public class ItemRepository<T, M> : BaseRepository<T, M>
        where T : ItemScriptable
        where M : Item
    {
        public ItemRepository() : base("Item") { }
        public ItemRepository(string folderName) : base(folderName) { }

        public override M DbDataToModel(T data)
        {
            return (M)Activator.CreateInstance(typeof(M), data);
        }

    }
}
