using Assets.Scripts.Game_Objects.Inheritable;

public class Shop : Interacting
{
    protected override void OnInteract()
    {
        FindAnyObjectByType<PauseManager>().ShowPausedInterface("ShopScene", null, false, false);
    }

}