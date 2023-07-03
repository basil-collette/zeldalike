using Assets.Database.Model.Design;
using System.Data;
using UnityEngine;

[System.Serializable]
public class HoldableItem : Item
{
    public HoldableItem(IDataReader reader) : base(reader)
    {
        
    }

    public HoldableItem() : base()
    {

    }

}
