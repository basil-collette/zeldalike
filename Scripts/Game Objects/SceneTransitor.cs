using UnityEngine;

public class SceneTransitor : MonoBehaviour
{
    public Vector2 playerNextPosition;
    public Vector2 playerNextDirection;
    public VectorValue playerPositionStorage;
    public VectorValue playerDirectionStorage;
    public TargetScene targetScene;

    ScenesManager sceneManager;

    void Start()
    {
        sceneManager = FindAnyObjectByType<ScenesManager>();

        if (targetScene.needPreload)
        {
            sceneManager.PreloadScene(targetScene);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            playerPositionStorage.initalValue = playerNextPosition;
            playerDirectionStorage.initalValue = playerNextDirection;

            sceneManager.SwitchScene(targetScene);
        }
    }

}
