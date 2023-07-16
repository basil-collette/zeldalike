using UnityEngine;

public abstract class ObjectGetterHelper : MonoBehaviour
{
    //RESOURCES
    public static Inventory Inventory => Resources.Load<Inventory>("ScriptableObjects/Player/Inventory/Inventory");
    public static PlayerQuest PlayerQuest => Resources.Load<PlayerQuest>("ScriptableObjects/Player/Quest/PlayerQuest");

    //SCENE
    public static Player Player => FindAnyObjectByType<Player>();
}