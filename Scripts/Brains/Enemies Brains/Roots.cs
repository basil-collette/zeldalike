using UnityEngine;

public class Roots : MonoBehaviour
{
    public void EndRoot()
    {
        GetComponentInParent<RoseMotherThinker>().StartCycle();
    }
}