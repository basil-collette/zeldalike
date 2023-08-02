using UnityEngine;

public class Plant : MonoBehaviour
{

    void Update()
    {
        
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            //make player half opacity and indetectable
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            //make player full opacity and detectable
        }
    }

}
