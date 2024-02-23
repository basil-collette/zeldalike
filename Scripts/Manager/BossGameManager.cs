using UnityEngine;

namespace Assets.Scripts
{
    public class BossGameManager : MonoBehaviour
	{
		[SerializeField] private GameObject _door;
		[SerializeField] private GameObject _gentleLittleOne;
		[SerializeField] private GameObject _roseMother;

		void Start()
		{
			if (MainGameManager._storyEventManager._scenario.Exists(x => x == "defeated_rose_mother"))
            {
				MainGameManager._soundManager.StopMusic();
				MainGameManager._soundManager.PlayMusic("peacefull");

				_gentleLittleOne.SetActive(true);

				_roseMother.SetActive(false);
				Destroy(_roseMother);

				_door.SetActive(false);
				Destroy(_door);
			}
		}

	}
}
