using Assets.Scripts.Game_Objects.Inheritable;
using System;
using System.Collections.Generic;

public class Door : FacingInteracting
{
    //public string keyUidString;
    //public Guid? keyUid;

    //public List<AliveEntity> enemies = new List<AliveEntity>();
    //int enemyCountOnStart;

    //public Button button;

    void Start()
    {
        //keyUid = (keyUidString == string.Empty) ? null : new Guid(keyUidString);

        /*
        if (enemies.Count > 0)
        {
            enemyCountOnStart = enemies.Count;
        }
        */
    }

    /*
    new void FixedUpdate()
    {
        if (enemyCountOnStart > 0
            && enemies.Count == 0)
        {
            DestroyImmediate(this.gameObject);
        }
    }
    */

    bool ConditionsOfTypesAreCompleted()
    {
        /*
        if (keyUidString != string.Empty
            && !FindFirstObjectByType<Player>().inventory.Items.Exists(item => item.NameCode == "key" && item.Uid == keyUid))
        {
            return false;
        }
        */

        /*
        if (button != null)
        {
            return false;
        }
        */

        return true;
    }

    protected override void OnInteract()
    {
        if (ConditionsOfTypesAreCompleted())
        {
            //remove key from inventory
            exitSignal?.Raise();
            DestroyImmediate(this.gameObject);
        }
        else
        {
            // show message of need conditions
        }
    }
}
