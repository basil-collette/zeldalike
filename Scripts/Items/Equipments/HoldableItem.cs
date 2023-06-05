using Assets.Database.Model.Design;
using System;
using System.Data;

[Serializable]
public class HoldableItem : Item
{
    public HoldableItem(IDataReader reader) : base(reader)
    {
        
    }

    public HoldableItem() : base()
    {

    }

}
