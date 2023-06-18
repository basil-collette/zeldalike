using UnityEngine;

public class RoomTransitor : MonoBehaviour
{
    public Vector3 playerChange;
    public Vector2 maxPosition; //border Top Right
    public Vector2 minPosition; //border Bottom Left

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            GetComponent<CameraMovement>().cameraParams.MinPos = minPosition;
            GetComponent<CameraMovement>().cameraParams.MinPos = maxPosition;

            collider.transform.position += playerChange;
        }
    }

    

}
