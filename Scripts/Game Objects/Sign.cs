using UnityEngine;

public class Sign : MonoBehaviour
{
    
    public string dialog;

    private GameObject dialogBox;

    private void Start()
    {
        dialogBox = transform.GetChild(0).GetChild(0).gameObject;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            OpenSign();
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            CloseSign();
        }
    }

    public void OpenSign()
    {
        dialogBox.SetActive(true);
    }

    public void CloseSign()
    {
        dialogBox.SetActive(false);
    }

}
