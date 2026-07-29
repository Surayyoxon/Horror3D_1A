using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 200f;

    public Transform playerBody;

    float xRotation = 0f;

    void Update()
    {
        // YANGI: Menyu ochiq bo'lsa yoki o'yin pauzada bo'lsa (Time.timeScale == 0) kamerani burmaymiz!
        if (Time.timeScale == 0f)
            return;

        float mouseX =
            Input.GetAxis("Mouse X") *
            mouseSensitivity *
            Time.deltaTime;

        float mouseY =
            Input.GetAxis("Mouse Y") *
            mouseSensitivity *
            Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation =
            Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
    }
}