using System;
using Unity.VisualScripting;
using UnityEngine;

public class TornadeTrigger : MonoBehaviour
{
        // Prepa Mise en cache
    private RotationScript RS;
    private Rigidbody rb;
    [SerializeField] private Déplacements Dép;
    private TornadeController TorCon;
    private Vector3 speedo;
    
        //Prepa GroundCheck
    private bool isGrounded;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [Header("Paramètres du player")]
    public float stiffness = 25f;
    public float verticalForce = 2f;
    public float rotationSpeed = 10f;
    public float tangentialMultiplier = 1f;
    public float radius = -15f;
    public float Inertia = 0f;
    public float verticalRestrict = 2;

    [Header("Références")]
    public Transform tornadoCenter;

    //Mises en cache:
    private void OnTriggerEnter(Collider other)
    {
       RS = other.GetComponent<RotationScript>();
       rb = other.GetComponent<Rigidbody>();
       Dép = other.GetComponent<Déplacements>();
       TorCon  = other.GetComponent<TornadeController>();
    }

    private void OnTriggerStay(Collider other)
    {
        //Simili State Machine:
        if (other.CompareTag("Player"))
        {
            RS.target = transform;
            RS.enabled = true;
            Inertia = Inertia + 10f;
            Dép.enabled = false;
            TorCon.enabled = true;
            speedo = rb.linearVelocity;
        }

        if (!other.CompareTag("Player")) return;
        {
            var rotationScript = other.GetComponent<RotationScript>();
            rotationScript.target = tornadoCenter;
            rotationScript.stiffness = stiffness;
            rotationScript.rotationSpeed = rotationSpeed;
            rotationScript.tangentialMultiplier = tangentialMultiplier;
            rotationScript.radius = radius;
            rotationScript.enabled = true;
            rotationScript.Inertia = Inertia;
            rotationScript.verticalRestrict = verticalRestrict;
        }
    }
        
    private void LateUpdate()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        if (isGrounded)
        {
            Dép.enabled = true;
        }
    }
        
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            rb.linearVelocity = new Vector3(0f, 0f, 0f);
            TorCon.enabled = false;
            rb.useGravity = true;
            RS.enabled = false;
            RS.Inertia = 0;
            
            rb.linearVelocity = speedo;
        } 
    }
}
