using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Hit : MonoBehaviour
{
    [SerializeReference]
    public List<Effect> effects = new List<Effect>();

    public Rigidbody2D rigidBody;

    protected virtual void Start()
    {
        //Given in inspector (just put the gameObject on property)
        //this.rigidBody = GetComponent<Rigidbody2D>();
    }

    protected virtual void OnHit(Hitable hited)
    {
        hited.Hit(this.rigidBody.position, effects);
    }

}