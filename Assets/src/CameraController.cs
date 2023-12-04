using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 2f;
    public float verticalLookSpeed = 2f;
    public float minLookAngle = -80f; // Adjust as needed
    public float maxLookAngle = 80f;  // Adjust as needed

    private float rotationX = 0f;

    void Update()
    {
        // Translation
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float upDown = 0f;

        if (Input.GetKey(KeyCode.Space))
        {
            upDown = 1f;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            upDown = -1f;
        }

        Vector3 moveDirection = new Vector3(horizontal, upDown, vertical).normalized;
        Vector3 moveAmount = moveDirection * moveSpeed * Time.deltaTime;
        transform.Translate(moveAmount);

        // Rotation
        float mouseX = Input.GetAxis("Mouse X") * turnSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * verticalLookSpeed;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, minLookAngle, maxLookAngle);

        transform.rotation = Quaternion.Euler(rotationX, transform.rotation.eulerAngles.y + mouseX, 0f);
    }
}
