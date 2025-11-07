using UnityEngine;

public class RotationScript : MonoBehaviour
{
    public Transform target;            // L'objet autour duquel orbiter
    public float rotationSpeed = 10f;  // Vitesse angulaire en radians/sec
    public Vector3 up = Vector3.up;     // Axe "up"
    public float radius = 2f;           // Distance orbitale souhaitée

    private Rigidbody rb;

    public float speedY = 2f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.useGravity = false;

    }

    void FixedUpdate()
    {
        rb.useGravity = false;
        if (target == null) return;
        
        var rotationCenter = target.position;
        rotationCenter.y = rb.position.y;
        
        Vector3 relativePos = transform.position - rotationCenter;
        Vector3 direction = relativePos.normalized;

    
        Vector3 desiredPos = rotationCenter + direction * radius;
        Vector3 positionError = desiredPos - transform.position;
        float stiffness = 50f;
        rb.AddForce(positionError * stiffness);
        rb.AddForce(Vector3.up * speedY, ForceMode.Acceleration); 

        
        Vector3 tangent = Vector3.Cross(up, direction).normalized;

       
        Vector3 tangentForce = tangent * rb.mass * radius * rotationSpeed ;

        rb.AddForce(tangentForce);
        Vector3 flatVelocity = rb.linearVelocity;
        flatVelocity.y = 0f; // Ignore le mouvement vertical

        if (flatVelocity.sqrMagnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(flatVelocity.normalized, Vector3.up);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * 5f));
        }

        
        // Rigidbody targetRb = target.GetComponent<Rigidbody>();
        // if (targetRb != null)
        // {
        //     rb.linearVelocity += targetRb.linearVelocity;
        // }
    }
}
