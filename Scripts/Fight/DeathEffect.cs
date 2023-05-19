using UnityEngine;

public class DeathEffect : MonoBehaviour
{

    public void Destroy()
    {
        Destroy(this.gameObject);
    }

}
