using UnityEngine;
using UnityEngine.UI;

public class Sign : FacingObject
{
    
    public string dialog;

    protected bool isShowed = false;
    private GameObject dialogBox;
    private Text dialogText;

    private void Start()
    {
        //dialogBox = FindGameObjectHelper.FindInactiveObjectByName("DialogBox");
        //dialogText = dialogBox.GetComponentInChildren<Text>();
    }

    void Update()
    {
        //
    }

    protected override void OnFacing()
    {
        if(!isShowed)
        {
            isShowed = true;

            //dialogBox.SetActive(true);
            //dialogText.text = dialog;
        }
    }

    protected override void OnQuitFacing()
    {
        //dialogBox.SetActive(false);
        isShowed = false;
    }

}
