using System;
using UnityEngine;

public class TornadeController : MonoBehaviour
{
    private float xinput;
    private RotationScript RS;
    private Rigidbody rb;
    public float Force; 
    private void Start()
    {
        RS = GetComponent<RotationScript>();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        xinput = Input.GetAxis("Horizontal");
        rb.AddForce(transform.right * xinput*Force, ForceMode.VelocityChange);
    }
}
