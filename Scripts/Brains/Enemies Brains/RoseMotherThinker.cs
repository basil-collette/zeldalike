using System.Collections;
using UnityEngine;

public class RoseMotherThinker : Bot
{
    public float openDuration;

    new void Start()
    {
        base.Start();

        StartCoroutine(testCo());
    }

    new void Update()
    {
        
    }

    IEnumerator testCo()
    {
        while(true)
        {
            yield return new WaitForSeconds(5f);

            GetComponent<Animator>().SetTrigger("open");

            yield return new WaitForSeconds(5f);

            GetComponent<Animator>().SetTrigger("close");
        }
    }

}