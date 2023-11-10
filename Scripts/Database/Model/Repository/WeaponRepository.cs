using Assets.Database.Model.Design;

namespace Assets.Database.Model.Repository
{
    public sealed class WeaponRepository : ItemRepository<WeaponScriptable, Weapon>
    {
        public WeaponRepository() : base ("Weapon") { }

        public override Weapon DbDataToModel(WeaponScriptable data)
        {
            return new Weapon(data);
        }

    }
}
