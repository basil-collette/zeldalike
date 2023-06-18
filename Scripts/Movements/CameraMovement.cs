using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    public CameraParameters cameraParams;

    void LateUpdate()
    {
        if (cameraParams.IsFixed || target == null)
            return;

        if (transform.position != target.position)
        {
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

            targetPosition.x = Mathf.Clamp(targetPosition.x, cameraParams.MinPos.x, cameraParams.MaxPos.x);

            targetPosition.y = Mathf.Clamp(targetPosition.y, cameraParams.MinPos.y, cameraParams.MaxPos.y);

            transform.position = Vector3.Lerp(transform.position, targetPosition, cameraParams.Smoothing);
        }
    }

}
