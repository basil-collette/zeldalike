using UnityEngine;

public class RoomTransitor : MonoBehaviour
{
    public Vector3 playerChange;
    public Vector2 maxPosition; //border Top Right
    public Vector2 minPosition; //border Bottom Left

    CameraMovement camera;

    void Start()
    {
        camera = Camera.main.GetComponent<CameraMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !collider.isTrigger)
        {
            camera.minPosition = minPosition;
            camera.maxPosition = maxPosition;

            collider.transform.position += playerChange;
        }
    }

    

}
