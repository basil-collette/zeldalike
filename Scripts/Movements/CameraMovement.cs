using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform Target;
    public CameraParameters CameraParams;

    void LateUpdate()
    {
        if (CameraParams.IsFixed || Target == null)
            return;

        if (transform.position != Target.position)
        {
            Vector3 targetPosition = new Vector3(Target.position.x, Target.position.y, transform.position.z);

            targetPosition.x = Mathf.Clamp(targetPosition.x, CameraParams.MinPos.x, CameraParams.MaxPos.x);

            targetPosition.y = Mathf.Clamp(targetPosition.y, CameraParams.MinPos.y, CameraParams.MaxPos.y);

            transform.position = Vector3.Lerp(transform.position, targetPosition, CameraParams.Smoothing);
        }
    }

    public void SetParams(CameraParameters cameraParams, Transform target)
    {
        CameraParams = cameraParams;
        Target = target;

        if (CameraParams.IsFixed)
        {
            transform.position = new Vector3(CameraParams.MinPos.x, CameraParams.MinPos.y, -10);
        }
        else
        {
            transform.position = new Vector3(Target.position.x, Target.position.y, -10);
        }
    }

}
