using Assets.Scripts.Game_Objects.Inheritable;
using System.Collections.Generic;
using UnityEngine;

public class LogTreeGrow : Interacting
{
    public bool hasGrown;

    void Start()
    {
        hasGrown = true;
    }

    protected override void OnInterfact()
    {
        if (true) // replace by is east object in use correct object
        {

        }
    }

    protected override void OnApproaching()
    {
        //
    }

    protected override void OnQuit()
    {
        //
    }

}
