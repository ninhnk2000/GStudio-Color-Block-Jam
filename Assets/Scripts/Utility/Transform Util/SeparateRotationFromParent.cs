using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeparateRotationFromParent : MonoBehaviour
{
    private Quaternion _lastRotation;

    private void Awake()
    {
        _lastRotation = transform.rotation;
    }

    void Update()
    {
        transform.rotation = _lastRotation;
    }
}
