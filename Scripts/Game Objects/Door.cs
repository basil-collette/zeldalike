using System;
using System.Collections.Generic;

public class Door : FacingInteractObject
{
    public bool open = false;

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

    protected override void OnFacingInterfact()
    {
        if (!open
            && ConditionsOfTypesAreCompleted())
        {
            open = true;
            DestroyImmediate(transform.gameObject);
        }
    }

    bool ConditionsOfTypesAreCompleted()
    {
        if (keyUidString != string.Empty
            && !FindFirstObjectByType<Player>().inventory.items.Exists(item => item.NameCode == "key" && item.Uid == keyUid))
        {
            return false;
        }

        if (enemyCountOnStart > 0
            && enemies.Count > 0)
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
