using UnityEngine;

public class ChangementState : MonoBehaviour
{
    [SerializeField] private MonoBehaviour groundScript;
    [SerializeField] private MonoBehaviour flyScript;
    [SerializeField] private KeyCode switchKey = KeyCode.LeftShift;
    private bool isFlying = false;

    void Start()
    {
        // Démarre avec le mode sol actif
        SetFlyMode(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(switchKey))
        {
            isFlying = !isFlying;
            SetFlyMode(isFlying);
        }
    }

    private void SetFlyMode(bool fly)
    {
        if (groundScript != null) groundScript.enabled = !fly;
        if (flyScript != null) flyScript.enabled = fly;
    }
}
