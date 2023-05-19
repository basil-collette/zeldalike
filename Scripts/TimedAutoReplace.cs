using System.Collections;
using UnityEngine;

public class TimedAutoReplace : MonoBehaviour
{
    public float timer;
    public GameObject prefab;

    void Start()
    {
        StartCoroutine(TimerAutoReplaceCo());
    }

    IEnumerator TimerAutoReplaceCo()
    {
        yield return new WaitForSeconds(timer);

        this.gameObject.SetActive(false);
        Instantiate(prefab, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

}
