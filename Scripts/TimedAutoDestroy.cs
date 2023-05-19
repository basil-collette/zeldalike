using System.Collections;
using UnityEngine;

public class TimedAutoDestroy : MonoBehaviour
{
    public float timer;

    void Start()
    {
        StartCoroutine(TimerAutoDestroyCo());
    }

    IEnumerator TimerAutoDestroyCo()
    {
        yield return new WaitForSeconds(timer);

        Destroy(this.gameObject);
    }

}
