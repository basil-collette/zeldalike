using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneTransitor : MonoBehaviour
{
    public string sceneToLoad;
    public Vector2 playerPosition;
    public VectorValue playerStorage;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            playerStorage.initalValue = playerPosition;
            SceneManager.LoadScene(sceneToLoad);
            //StartCoroutine(ResetSceneAsync());
        }
    }

    IEnumerator ResetSceneAsync()
    {
        /*
        if (SceneManager.sceneCount > 1)
        {
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync("LevelScene");
            while (!asyncUnload.isDone)
            {
                yield return null;
            }
            Resources.UnloadUnusedAssets();
        }
        */

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneToLoad));
        /*
        m_LevelBuilder.Build();

        m_Player = FindObjectOfType<Player>();
        this.boxes = FindObjectsOfType<Box>();
        this.m_Player = FindObjectOfType<Player>();
        */
    }

    /*
    public void NextLevel()
    {
        Build();
        StartCoroutine(ResetSceneAsync());
    }

    public void Build()
    {
        m_Level = GetComponent<Levels>().m_Levels[m_CurrentLevel];

        int startX = -m_Level.Width / 2;
        int x = startX;
        int y = -m_Level.Height / 2;

        foreach (var row in m_Level.m_Rows)
        {
            foreach (var ch in row)
            {
                LevelElement levelPrefab = GetPrefab(ch);
                if (levelPrefab != null)
                {
                    GameObject gameObject = Instantiate(levelPrefab.m_Prefab, new Vector3(x, y, 0), Quaternion.identity);

                    Renderer renderer = gameObject.GetComponent<Renderer>();
                    if (renderer != null) renderer.sortingOrder = levelPrefab.layerOrder;
                }
                x++;
            }
            y++;

            x = startX;
        }
    }

    public void ResetScene()
    {
        StartCoroutine(ResetSceneAsync());
    }
    */

}
