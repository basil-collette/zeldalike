using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RoomTransitor : MonoBehaviour
{
    public Vector3 playerChange;
    public Vector2 maxPosition; //border Top Right
    public Vector2 minPosition; //border Bottom Left
    public bool needText;
    public string placeName;

    CameraMovement camera;
    GameObject textBox;
    Text placeText;

    void Start()
    {
        camera = Camera.main.GetComponent<CameraMovement>();
        textBox = FindGameObjectHelper.FindInactiveObjectByName("PlaceName");
        placeText = textBox.GetComponentInChildren<Text>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            camera.minPosition = minPosition;
            camera.maxPosition = maxPosition;

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
        textBox.SetActive(true);

        yield return new WaitForSeconds(4f);
        textBox.SetActive(false);
    }

}
