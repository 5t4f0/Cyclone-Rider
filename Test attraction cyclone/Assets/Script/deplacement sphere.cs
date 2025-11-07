using UnityEngine;

public class deplacementsphere : MonoBehaviour
{
    [SerializeField]private Rigidbody rb;
    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(0, 0, 5f);
        rb.angularVelocity = new Vector3(0, 15f, 0);
    }
}
