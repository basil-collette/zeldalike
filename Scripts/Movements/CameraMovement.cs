using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    public float smoothing = 0.2f;
    public bool fixedPos = false;
    public Vector2 maxPosition; //border Top Right
    public Vector2 minPosition; //border Bottom Left

    void LateUpdate()
    {
        if (fixedPos)
        {
            return;
        }

        if (transform.position != target.position)
        {
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

            targetPosition.x = Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x);

            targetPosition.y = Mathf.Clamp(targetPosition.y, minPosition.y, maxPosition.y);

            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);
        }
    }

}
