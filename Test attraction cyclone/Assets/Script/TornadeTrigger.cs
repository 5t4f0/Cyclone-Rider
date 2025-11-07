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
            RS.Inertia = other.GetComponent<RotationScript>().Inertia + 10f;
            Dép.enabled = false;
            TorCon.enabled = true;
            speedo = rb.linearVelocity;
        } 
       
    }
        //GroundCheck: (pour qu'il n'active les déplacements qu'une foi au sol)
    private void LateUpdate()
    {
        // if (rb != null)
        // {
        //     Debug.DrawLine(rb.transform.position, speedo, Color.red);
        // }
        // else
        // {
        //     return;
        // }
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        if (isGrounded)
        {
            Dép.enabled = true;
        }
    }
        //Simili State Machine 2
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
