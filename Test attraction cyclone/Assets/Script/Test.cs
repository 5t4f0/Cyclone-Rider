using UnityEngine;

public class Test : MonoBehaviour
{
  
    public Transform Oeil;
    public float force = 100f;

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb == null) return;

        Vector3 direction = (Oeil.position - other.transform.position).normalized;
        rb.AddForce(direction * force, ForceMode.Force);

        Debug.DrawRay(other.transform.position, direction * 2, Color.green);
        Debug.Log("Objet attiré : " + other.name);
    }
}

