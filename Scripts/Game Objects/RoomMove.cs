using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RoomMove : MonoBehaviour
{
    //public Vector2 cameraChange; //Pas besoin grace aux minPos et maxPos
    public Vector3 playerChange;
    public Vector2 maxPosition; //border Top Right
    public Vector2 minPosition; //border Bottom Left
    public CameraMovement cameraMovement;
    public bool needText;
    public string placeName;
    public GameObject text;
    public Text placeText;

    CameraMovement camera;

    void Start()
    {
        camera = Camera.main.GetComponent<CameraMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            cameraMovement.minPosition = minPosition;
            cameraMovement.maxPosition = maxPosition;

            collider.transform.position += playerChange;

            if (needText)
            {
                StartCoroutine(PlaceNameCo());
            }
        }
    }

    private IEnumerator PlaceNameCo()
    {
        placeText.text = placeName;
        text.SetActive(true);

        yield return new WaitForSeconds(4f);
        text.SetActive(false);
    }

}
