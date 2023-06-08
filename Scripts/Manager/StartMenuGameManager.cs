using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuGameManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

}
