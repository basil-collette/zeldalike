using Assets.Scripts.Game_Objects.Inheritable;
using System.Collections.Generic;
using UnityEngine;

public class LogTreeGrow : Interacting
{
    public bool hasGrown;

    protected override void OnInterfact()
    {
        throw new System.NotImplementedException();
    }

    void Start()
    {
        hasGrown = true;
    }

}
