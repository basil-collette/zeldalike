using UnityEngine;

public class ExistIfCondition : MonoBehaviour
{
    [SerializeField] Condition[] _conditions;

    void Start()
    {
        bool mustShow = Condition.VerifyAll(_conditions);

        if (!mustShow)
        {
            Destroy(gameObject);
        }
    }

}