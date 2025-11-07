using UnityEngine;

public class Vol : MonoBehaviour
{

    [SerializeField] private Rigidbody rb;
    [SerializeField] private float maxFlySpeed = 20f;
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float rotationSpeed = 60f; // degrés par seconde pour pitch/yaw avec touches
    [SerializeField] private float pitchLimit = 80f;    // limite pitch
    [SerializeField] private float strafeSpeed = 10f;   // vitesse de translation latérale (strife)
    [SerializeField] private float mouseStrafeSensitivity = 2f; // sensibilité souris pour strafe

    private float currentSpeed = 0f;
    private float pitch = 0f;
    private float yaw = 0f;

    private void OnEnable()
    {
        rb.useGravity = false;
        currentSpeed = 0f;
        Vector3 euler = transform.eulerAngles;
        pitch = euler.x;
        yaw = euler.y;

        // Optionnel : verrouiller le curseur pour capter la souris proprement
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        rb.useGravity = true;
        rb.linearVelocity = Vector3.zero;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void FixedUpdate()
    {
        // Accélération progressive vers vitesse max
        currentSpeed = Mathf.MoveTowards(currentSpeed, maxFlySpeed, acceleration * Time.fixedDeltaTime);

        // Inputs clavier pitch / yaw
        float pitchInput = 0f;
        float yawInput = 0f;
        if (Input.GetKey(KeyCode.W)) pitchInput = 1f;
        if (Input.GetKey(KeyCode.S)) pitchInput = -1f;
        if (Input.GetKey(KeyCode.A)) yawInput = -1f;
        if (Input.GetKey(KeyCode.D)) yawInput = 1f;

        // Appliquer pitch/yaw
        pitch += pitchInput * rotationSpeed * Time.fixedDeltaTime;
        yaw += yawInput * rotationSpeed * Time.fixedDeltaTime;

        pitch = Mathf.Clamp(pitch, -pitchLimit, pitchLimit);

        Quaternion targetRotation = Quaternion.Euler(pitch, yaw, 0f);
        transform.rotation = targetRotation;

        // Strafe latéral contrôlé par la souris (axe X)
        float mouseX = Input.GetAxis("Mouse X"); // déplacement horizontal souris
        Vector3 strafeDirection = transform.right * mouseX * strafeSpeed;

        // Vitesse avant constante
        Vector3 forwardVelocity = transform.forward * currentSpeed;

        // Combine déplacement forward + strafe
        Vector3 finalVelocity = forwardVelocity + strafeDirection;

        rb.linearVelocity = finalVelocity;
    }
}
