using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    private GameObject player;
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (transform != null && player != null)
        {
            transform.position = player.transform.position + offset;
        }
    }

    public void SetPlayer(GameObject mainPlayer)
    {
        player = mainPlayer;
    }
}
