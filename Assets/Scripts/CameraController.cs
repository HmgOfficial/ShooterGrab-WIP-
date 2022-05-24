using UnityEngine;

public class CameraController : MonoBehaviour
{
    /// <summary>
    /// Saves the transform component of the target
    /// </summary>
    private Transform target;
    /// <summary>
    /// The speed of the camera
    /// </summary>
    public float smoothSpeed = 10f;
    /// <summary>
    /// The offset respect the target
    /// </summary>
    public Vector3 offset;

    private void Start()
    {
        target = PlayerManager.instance.transform;
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.gameMode == GameManager.GameMode.Game)
        {
            SmoothCamera();
        }
        
    }

    /// <summary>
    /// The camera is moved to the target with a smoothed movement
    /// </summary>
    public void SmoothCamera()
    {
        //The desired position
        Vector3 desiredPosition = target.position + offset;
        //Generates a smooth with the desired position and the position of the camera
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        //Moves camera to the smoothedPosition
        transform.position = smoothedPosition;

        //Look at the target (player)
        transform.LookAt(target);
        
    }
}
