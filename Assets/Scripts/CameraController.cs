using UnityEngine;

public class CameraController : MonoBehaviour {

    [HideInInspector]
    public static Transform target;

    [Header("Play Mode")]
    public Vector3 offset;
    public float smoothSpeed = 0.125f;

    [Header("Edit Mode")]
    public float turnSpeed = 5f;
    public float zoomSpeed = 5f;
    public float smoothLookAtSpeed = 5f;

    void FixedUpdate()
    {
        if (State.isPlaying)
        {
            Vector3 reqiredPosition = target.position + offset;
            Vector3 lerpedPosition = Vector3.Lerp(transform.position, reqiredPosition, smoothSpeed);
            transform.position = lerpedPosition;

            transform.LookAt(target);
        }
        else
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            float z = Input.GetAxis("Mouse ScrollWheel");

            Vector3 reqiredMovement = new Vector3(x * turnSpeed, y * turnSpeed, z * zoomSpeed);
            transform.Translate(reqiredMovement);

            /*
            if (Input.GetKeyDown(KeyCode.Alpha4)) x = -turnSpeed;
            if (Input.GetKeyDown(KeyCode.Alpha6)) x = turnSpeed;
            if (Input.GetKeyDown(KeyCode.Alpha8)) y = turnSpeed;
            if (Input.GetKeyDown(KeyCode.Alpha2)) y = -turnSpeed;

            Vector3 requiredRotation = new Vector3(x, y, 0);
            transform.Rotate(requiredRotation);

            if (!requiredRotation.Equals(Vector3.zero))
            {*/

            if (GameObjectsManager.lastSelectedObject != null)
            {
                Quaternion targetRotation = Quaternion.LookRotation(GameObjectsManager.lastSelectedObject.transform.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothLookAtSpeed * Time.deltaTime);
            }

            /*}*/
        }
    }

}
