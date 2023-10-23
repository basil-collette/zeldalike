using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    public GameObject rollSmokeEffect;

    void CreateDashEffect()
    {
        Instantiate(rollSmokeEffect, transform.position, Quaternion.identity);
    }

}
