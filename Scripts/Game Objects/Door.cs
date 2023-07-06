using System;
using System.Collections.Generic;

public class Door : FacingInteractObject
{
    public string keyUidString;
    public Guid? keyUid;

    public List<AliveEntity> enemies = new List<AliveEntity>();
    int enemyCountOnStart;

    //public Button button;

    void Start()
    {
        keyUid = (keyUidString == string.Empty) ? null : new Guid(keyUidString);

        if (enemies.Count > 0)
        {
            enemyCountOnStart = enemies.Count;
        }
    }

    new void FixedUpdate()
    {
        if (enemyCountOnStart > 0
            && enemies.Count == 0)
        {
            DestroyImmediate(this.gameObject);
        }
    }

    protected override void OnFacingInterfact()
    {
        if (ConditionsOfTypesAreCompleted())
        {
            DestroyImmediate(this.gameObject);
        }
        else
        {
            // show message of need conditions
        }
    }

    bool ConditionsOfTypesAreCompleted()
    {
        if (keyUidString != string.Empty
            && !FindFirstObjectByType<Player>().inventory.Items.Exists(item => item.NameCode == "key" && item.Uid == keyUid))
        {
            return false;
        }

        /*
        if (button != null)
        {
            return false;
        }
        */

        return true;
    }

    protected override void OnFacing()
    {
        throw new NotImplementedException();
    }

    protected override void OnQuitFacing()
    {
        throw new NotImplementedException();
    }

}
