using System;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    private void LateUpdate()
    {
        Vector3 rot = transform.eulerAngles;
        rot.z = 0f;
        transform.eulerAngles = rot;
    }
}
