using System.Runtime.CompilerServices;
using UnityEngine;

public class AttractionScript : MonoBehaviour
{
    [Header("Réglages")]
    public GameObject Oeil; // Centre de la tornade
    [SerializeField] private float attractionForce = 5f;
    [SerializeField] private float rotationForce = 3f;
    [SerializeField] private string excludedTag = "Tornade"; // Objets à ignorer

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Tornade")) return;

        Rigidbody rb = other.attachedRigidbody;
        if (rb == null) return;

        Vector3 toCenter = Oeil.transform.position - other.transform.position;
        Vector3 directionToCenter = toCenter.normalized;

        // Calcul de la direction tangentielle (perpendiculaire à la direction vers le centre)
        Vector3 tangentDirection = Vector3.Cross(Vector3.up, directionToCenter).normalized;

        // Force vers le centre
        rb.AddForce(directionToCenter * attractionForce, ForceMode.VelocityChange);

        // Force tangentielle pour faire tourner autour
        rb.AddForce(tangentDirection * rotationForce, ForceMode.VelocityChange);

        // Debug dans la scène
        Debug.DrawRay(other.transform.position, directionToCenter * 2, Color.blue);   // vers centre
        Debug.DrawRay(other.transform.position, tangentDirection * 2, Color.yellow);  // rotation
    }
}
