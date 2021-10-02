using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static GameObject mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        SetCamera();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetCamera()
    {
        mainCamera = GameObject.Find("Main Camera");
        mainCamera.GetComponent<CameraFollowPlayer>().SetPlayer(this.gameObject);
    }
}
