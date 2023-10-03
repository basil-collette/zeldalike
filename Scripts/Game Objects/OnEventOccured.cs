using UnityEngine;

public class OnEventOccured : MonoBehaviour
{
    [SerializeField] string eventNameCode;
    [SerializeField] bool Not;

    void Start()
    {
        bool mustShow = EventOccured();
        mustShow = (Not) ? !mustShow : mustShow;

        if (!mustShow)
        {
            Destroy(gameObject);
        }
    }

    bool EventOccured()
    {
        return MainGameManager._storyEventManager._scenario.Exists(x => x == eventNameCode);
    }

}