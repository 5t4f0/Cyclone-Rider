using UnityEngine;

public class CameraController : MonoBehaviour
{
      [Header("Références")]
    public Transform player;            // Le joueur à suivre
    public Rigidbody playerRb;          // Pour récupérer la direction du mouvement

    [Header("Paramètres de caméra")]
    public Vector3 offset = new Vector3(0, 3, -6); // Position relative par rapport à la direction du joueur
    public float positionSmoothness = 5f;          // Lissage de déplacement
    public float rotationSmoothness = 3f;          // Lissage de rotation
    public float followAhead = 3f;                 // Distance à anticiper vers l’avant

    [Header("Contrôle de la souris (optionnel)")]
    public float mouseSensitivity = 3f;
    private float yaw;  // rotation horizontale
    private float pitch; // rotation verticale
    public float minPitch = -10f;
    public float maxPitch = 45f;

    void Start()
    {
        if (playerRb == null && player != null)
            playerRb = player.GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (!player) return;

        // 🖱️ Rotation libre via la souris
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // 🧭 Direction de référence : la direction du joueur ou de sa vitesse
        Vector3 forward = player.forward;
        if (playerRb && playerRb.linearVelocity.sqrMagnitude > 0.5f)
        {
            Vector3 vel = playerRb.linearVelocity;
            vel.y = 0;
            if (vel.sqrMagnitude > 0.1f)
                forward = vel.normalized;
        }

        // 💫 Position "idéale" de la caméra
        Vector3 targetPos = player.position 
                            - forward * offset.z 
                            + Vector3.up * offset.y 
                            + forward * followAhead * 0.5f; // petit décalage vers l’avant

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * positionSmoothness);

        // 🧱 Orientation caméra (selon la souris, pas le joueur)
        Quaternion lookDir = Quaternion.Euler(pitch, yaw, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookDir, Time.deltaTime * rotationSmoothness);
    }
}
