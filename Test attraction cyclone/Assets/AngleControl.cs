using System;
using UnityEngine;

public class AngleControl : MonoBehaviour
{
    private Rigidbody rb;
    public Déplacements Dep;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 flatVelocity = rb.linearVelocity;
        flatVelocity.y = 0f;
        if (flatVelocity.sqrMagnitude > 0.1f&& !(Dep.xinput==0))
        {
            Quaternion targetRotation = Quaternion.LookRotation(flatVelocity.normalized, Vector3.up);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * 5f));
            
        }
    }
}
