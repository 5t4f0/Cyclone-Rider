using UnityEngine;

public class CameraController : MonoBehaviour
{
       [Header("Références")]
    public Transform player;           // Le joueur à suivre
    public Rigidbody playerRb;         // Optionnel, pour anticiper le mouvement

    [Header("Paramètres de position")]
    public Vector3 offset = new Vector3(0, 3, -6); // Hauteur et recul de la caméra
    public float smoothPosition = 8f;              // Vitesse de suivi position
    public float smoothRotation = 8f;              // Vitesse de suivi rotation

    [Header("Rotation caméra (souris)")]
    public float sensitivityX = 3f;
    public float sensitivityY = 2f;
    public float minPitch = -20f;
    public float maxPitch = 60f;

    [Header("Zoom")]
    public float minDistance = 3f;
    public float maxDistance = 10f;
    public float zoomSpeed = 3f;

    private float yaw;    // rotation horizontale
    private float pitch;  // rotation verticale
    private float targetDistance;

    void Start()
    {
        if (playerRb == null && player != null)
            playerRb = player.GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        targetDistance = -offset.z;
    }

    void FixedUpdate()
    {
        if (!player) return;

        // 🎚️ Zoom à la molette
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        targetDistance -= scroll * zoomSpeed;
        targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);
        offset.z = -targetDistance;

        // 🖱️ Rotation libre à la souris
        yaw += Input.GetAxis("Mouse X") * sensitivityX;
        pitch -= Input.GetAxis("Mouse Y") * sensitivityY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // 💫 Calcul de la rotation orbitale autour du joueur
        Quaternion desiredRotation = Quaternion.Euler(pitch, yaw, 0);

        // 📏 Calcul de la position souhaitée
        Vector3 desiredPosition = player.position + desiredRotation * offset;

        // 🔄 Lissage
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothPosition);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * smoothRotation);
        
    }
}
