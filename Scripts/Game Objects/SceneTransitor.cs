using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitor : MonoBehaviour
{
    public string sceneToLoad;
    public bool needPreload = false;
    public Vector2 playerNextPosition;
    public VectorValue playerPositionStorage;
    public SceneLoadManager _sceneManager;

    void Start()
    {
        var askedScene = SceneManager.GetSceneByName(sceneToLoad);
        if (askedScene.IsValid())
        {
            throw new InvalidOperationException("Cannot find the scene named : \"" + sceneToLoad + "\".");
        }

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
                _sceneManager.Test(sceneToLoad);
            }
        }
    }

}
