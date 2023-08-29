using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventZone : MonoBehaviour
{
    public bool TriggerOnce = true;

    bool EventTriggered;

    void Start()
    {
        
    }

    protected new void OnTriggerEnter2D(Collider2D collider)
    {
        if (TriggerOnce && EventTriggered) return;

        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            
        }
    }

}