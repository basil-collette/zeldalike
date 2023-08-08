using UnityEngine;

public class Plant : MonoBehaviour
{

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            //var size = collider.GetComponent<BoxCollider2D>().size;
            //var contacts = collider.GetContacts(GetComponent<Collider2D>().GetContacts());

            collider.GetComponent<SpriteRenderer>().color = new Color(0.1f, 0.1f, 0.1f, 0.6f);
            //make player indetectable : add SenseEnum.see in his sense list "impercetibles"
        }
    }

    protected void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            collider.GetComponent<SpriteRenderer>().color = new Color(0.1f, 0.1f, 0.1f, 0.6f);
            //make player indetectable : add see in his sense list "impercetibles"
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            collider.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            //make player detectable
        }
    }

}
