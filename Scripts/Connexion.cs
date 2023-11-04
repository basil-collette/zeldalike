using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Connexion : MonoBehaviour
{
    [SerializeField] InputField identifierInput;
    [SerializeField] InputField passwordInput;

    public void TryConnect()
    {
        Dictionary<string, string> data = new Dictionary<string, string>() {
            { "identifier", identifierInput.text },
            { "password", passwordInput.text }
        };

        StartCoroutine(ConnexionRequest(data));
    }

    IEnumerator ConnexionRequest(Dictionary<string, string> data)
    {
        const string URL = "https://www.basilcollette.com/chappy/zeldoconnect";

        using (UnityWebRequest webRequest = UnityWebRequest.Post(URL, data))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(webRequest.error);
                MainGameManager._toastManager.Add(new Toast("Echec de connexion!", ToastType.Error));
            }
            else
            {
                FindGameObjectHelper.FindByName("Main Game Manager").GetComponent<MainGameManager>().StartGame();
            }

            /*
            string[] pages = URL.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.Log("first");
                    //Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.Log("protocol");
                    //Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log("Connexion succeed");
                    //Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
                default:
                    Debug.Log("Connexion failed");
                    break;
            }
            */
        }
    }

}
