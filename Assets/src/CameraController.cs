using UnityEngine;

public class CameraController : MonoBehaviour {
    public float moveSpeed = 5f;
    public float turnSpeed = 2f;
    public float verticalMoveSpeed = 3f;

    void Update() {
        // Translation
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float upDown = 0f;

        if (Input.GetKey(KeyCode.Space)) {
            upDown = 1f;
        } else if (Input.GetKey(KeyCode.LeftShift)) {
            upDown = -1f;
        }

        Vector3 moveDirection = new Vector3(horizontal, upDown, vertical).normalized;
        Vector3 moveAmount = moveDirection * moveSpeed * Time.deltaTime;
        transform.Translate(moveAmount);

        // Rotation
        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up * mouseX * turnSpeed);

        // Ensure the camera doesn't roll
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0f);
    }
}
