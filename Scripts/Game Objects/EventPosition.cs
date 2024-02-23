using UnityEngine;

namespace Assets.Scripts.Game_Objects
{
    public class EventPosition : MonoBehaviour
    {
        [SerializeField] private EventPositionElement[] _datas;

        private void Start()
        {
            foreach (EventPositionElement d in _datas)
            {
                if (MainGameManager._storyEventManager._scenario.Exists(x => x == d.EventName)
                    || MainGameManager._dialogStatesManager.AnyHaveSaid(d.EventName)
                    || MainGameManager._inventoryManager._items.Exists(x => x.NameCode == d.EventName))
                {
                    transform.localPosition = d.Position;

                    if (d.Position == Vector3.zero)
                        Destroy(gameObject);
                    
                    return;
                }
            }
        }

    }

    [System.Serializable]
    public struct EventPositionElement
    {
        public Vector3 Position;
        public string EventName;
    }
}
