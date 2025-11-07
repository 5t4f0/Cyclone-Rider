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

        // Centre de rotation horizontal
        Vector3 rotationCenter = target.position;
        rotationCenter.y = rb.position.y;
        
        // Position relative et direction
        Vector3  ToCenter = transform.position - rotationCenter;
        Vector3 TocenterN =  ToCenter.normalized;
        
        // Force tangentielle
        Vector3 tangent = Vector3.Cross(up,  ToCenter).normalized;
        Vector3 tangentForce = tangent * rb.mass * radius * rotationSpeed * tangentialMultiplier;
        rb.AddForce(tangentForce);
        
        // Position cible et force corrective (permet de tourner)
        Vector3 desiredPos = rotationCenter - TocenterN * radius;
        Vector3 ToDesired = desiredPos - rb.position;
        rb.AddForce( ToDesired * stiffness);
        
        // Force qui va vers le haut et vers l'horizontal (mises ensemble car sinon elles s'annulent)
        Vector3 horizontalForward = transform.forward;
        horizontalForward.y = 0f;
        horizontalForward.Normalize();
        Vector3 totalForce = horizontalForward * Inertia + Vector3.up * (Inertia / verticalRestrict) ;
        rb.AddForce(totalForce, ForceMode.Acceleration);
        

        // 🔄 Rotation du joueur dans la direction de déplacement
        // Vector3 flatVelocity = rb.linearVelocity;
        // // flatVelocity.y = 0f;
        // if (flatVelocity.sqrMagnitude > 0.1f)
        // {
        //     Quaternion targetRotation = Quaternion.LookRotation(flatVelocity.normalized, Vector3.up);
        //     rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * 5f));
        //     
        // }
        // Debug.DrawRay(transform.position,rb.linearVelocity,Color.magenta);

        // limitateur de vitesse
        rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, 15f);
    }
}
