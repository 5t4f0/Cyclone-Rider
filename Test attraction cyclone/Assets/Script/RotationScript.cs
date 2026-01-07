using UnityEngine;

public class RotationScript : MonoBehaviour
    
{
    [Header("Cible")]
    public Transform target;          // L'objet autour duquel orbiter

    [Header("Paramètres orbitaux")]
    public float rotationSpeed = 10f; // Vitesse angulaire
    public float radius = -15f;         // Distance orbitale
    public Vector3 up = Vector3.up;   // Axe vertical de rotation

    [Header("Forces")]
    public float stiffness = 25f;     // Force de “tirage” vers le cercle
    public float verticalRestrict = 2;  // parametrage de réglage de l'inertie
    public float tangentialMultiplier = 1f; // Multiplicateur sur la force tangentielle
    public float Inertia = 0f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

     
    }

    void FixedUpdate()
    {
        if (target == null) return;

        rb.useGravity = false;
        
        var rotationCenter = SetRotationCenter();
        
        Vector3  ToCenter = transform.position - rotationCenter;
        Vector3 TocenterN =  ToCenter.normalized;
        
        AddTangentForce(ToCenter);
        
        AddLasso(rotationCenter, TocenterN);
        
        AddUpAndFrontForce();

        rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, 15f);
    }

    private void AddUpAndFrontForce()
    {
        Vector3 horizontalForward = transform.forward;
        horizontalForward.y = 0f;
        horizontalForward.Normalize();
        Vector3 totalForce = horizontalForward * Inertia + Vector3.up * (Inertia / verticalRestrict) ;
        rb.AddForce(totalForce, ForceMode.Acceleration);
    }

    private void AddLasso(Vector3 rotationCenter, Vector3 TocenterN)
    {
        Vector3 desiredPos = rotationCenter - TocenterN * radius;
        Vector3 ToDesired = desiredPos - rb.position;
        rb.AddForce( ToDesired * stiffness);
    }

    private void AddTangentForce(Vector3 ToCenter)
    {
        Vector3 tangent = Vector3.Cross(up,  ToCenter).normalized;
        Vector3 tangentForce = tangent * rb.mass * radius * rotationSpeed * tangentialMultiplier;
        rb.AddForce(tangentForce);
    }

    private Vector3 SetRotationCenter()
    {
        Vector3 rotationCenter = target.position;
        rotationCenter.y = rb.position.y;
        return rotationCenter;
    }
}
