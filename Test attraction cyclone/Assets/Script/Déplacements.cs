using UnityEngine;

public class Déplacements : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 7f;
    [SerializeField] private float inertiaDamp = 0.98f; // 1 = pas de ralentissement, <1 = inertie
    [SerializeField] private float RotationSpeed = 5f;
    [SerializeField] private Transform _camera;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;

    private bool isGrounded;
    private bool jumpRequest;
    public float xinput;
    public float yinput;

    private void Update()
    {
        {
            xinput = Input.GetAxis("Horizontal");
            yinput = Input.GetAxis("Vertical");

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                jumpRequest = true;
            }
        }
    }

    private void FixedUpdate()
    {
        Vector3 camForward = _camera.forward;
        Vector3 camRight = _camera.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 inputDir = (camForward * yinput + camRight * xinput).normalized;

        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        float dot = Vector3.Dot(horizontalVelocity.normalized, inputDir);

        float acceleration = speed * Time.fixedDeltaTime;
        float brakingForce = acceleration * 2f; // tu peux ajuster ce facteur

        if (inputDir.magnitude > 0.1f)
        {
            if (dot < -0.1f) // direction opposée
            {
                // Applique une force de freinage au lieu de forcer la nouvelle direction
                rb.linearVelocity = Vector3.MoveTowards(rb.linearVelocity, Vector3.zero, brakingForce);
            }
            else
            {
                // Applique la vitesse dans la direction actuelle (accélération normale)
                Vector3 targetVelocity = inputDir * speed;
                rb.linearVelocity = Vector3.MoveTowards(rb.linearVelocity, targetVelocity, acceleration);
            }
        }
        else
        {
            // Pas d’input : inertie naturelle
            rb.linearVelocity = new Vector3(rb.linearVelocity.x * 0.95f, rb.linearVelocity.y, rb.linearVelocity.z * 0.95f);
        }

      
        rb.angularVelocity = new Vector3(0, xinput, 0);
        // Vérifie si on est au sol
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        if (jumpRequest&&isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); // reset Y pour un saut cohérent
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpRequest = false;
        }
    }
}
