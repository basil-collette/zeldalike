using UnityEngine;
using UnityEngine.UI;

public class SignInteract : FacingInteractObject
{
    public GameObject dialogBox;
    public Text dialogText;
    public string dialog;
    public Text debugText;

    protected override void OnQuitFacing()
    {
        dialogBox.SetActive(false);
    }

    protected override void OnFacingInterfact()
    {
        if (dialogBox.activeInHierarchy)
        {
            dialogBox.SetActive(false);
        }
        else
        {
            dialogBox.SetActive(true);
            dialogText.text = dialog;
        }
    }

}
