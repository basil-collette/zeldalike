using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Hit : MonoBehaviour
{
    [SerializeReference] public List<Effect> effects = new List<Effect>();

    protected virtual void Start()
    {
        //Given in inspector (just put the gameObject on property)
        //this.rigidBody = GetComponent<Rigidbody2D>();
    }

    protected virtual void OnHit(Hitable hited)
    {
        hited.Hit(gameObject, effects);
    }

}