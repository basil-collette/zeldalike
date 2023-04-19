using UnityEngine;

public class SceneTransitor : MonoBehaviour
{
    public string sceneToLoad;
    public bool needPreload = false;
    public Vector2 playerNextPosition;
    public VectorValue playerPositionStorage;
    public SceneLoadManager _sceneManager;

    void Start()
    {
        if (needPreload)
        {
            _sceneManager.PreloadScene(sceneToLoad);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            playerPositionStorage.initalValue = playerNextPosition;

            if (needPreload)
            {
                _sceneManager.UsePreloadedScene(sceneToLoad);
            }
            else
            {
                _sceneManager.SwitchScene(sceneToLoad);
            }
        }
    }

}
