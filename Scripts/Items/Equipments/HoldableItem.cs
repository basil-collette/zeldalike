using Assets.Database.Model.Design;
using System.Data;
using UnityEngine;

[System.Serializable]
public class HoldableItem : Item
{

    public HoldableItem(IDataReader reader) : base(reader)
    {
        PostInstanciation();
    }

    public new static HoldableItem InstanciateFromJsonString(string json)
    {
        HoldableItem holdableItem = JsonUtility.FromJson<HoldableItem>(json);

        holdableItem.PostInstanciation();

        return holdableItem;
    }

    public new void PostInstanciation()
    {
        base.PostInstanciation();
    }

}
