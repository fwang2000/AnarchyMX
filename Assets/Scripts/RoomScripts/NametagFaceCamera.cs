using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NametagFaceCamera : MonoBehaviour
{
    private Transform mainCameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        mainCameraTransform = Camera.main.transform;
        SetNametagLookAtCamera();
    }

    private void SetNametagLookAtCamera()
    {
        transform.LookAt(transform.position + mainCameraTransform.rotation * Vector3.forward, mainCameraTransform.rotation * Vector3.up);
    }
}
