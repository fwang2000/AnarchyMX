using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    #region CHARACTER MOVEMENT
    private Rigidbody rb;
    private float moveSpeed;
    private float smoothTime;
    private Vector3 forward, right, upMovement, rightMovement;

    private bool grounded;
    private Vector3 smoothMoveVelocity;
    private Vector3 moveAmount;
    #endregion

    private PhotonView PV;
    public static GameObject LocalPlayerGameObject;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();

        smoothTime = 0.1f;
        moveSpeed = 20f;

        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);

        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;

        if (PV.IsMine)
        {
            PlayerController.LocalPlayerGameObject = this.gameObject;
        }
    }

    // Update is called once per frames
    void Update()
    {
        if (!PV.IsMine)
        {
            return;
        }

        Move();
    }

    private void Move()
    {
        rightMovement = right * Input.GetAxis("HorizontalKey");
        upMovement = forward * Input.GetAxis("VerticalKey");
        Vector3 moveDir = (rightMovement + upMovement).normalized;
        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir, ref smoothMoveVelocity, smoothTime);
    }

    public void SetGroundedState(bool isGrounded)
    {
        grounded = isGrounded;
    }

    private void FixedUpdate()
    {
        if (!PV.IsMine)
        {
            return;
        }

        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * moveSpeed * Time.fixedDeltaTime);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(rb.position);
            stream.SendNext(rb.rotation);
            stream.SendNext(rb.velocity);
        }    
        else
        {
            rb.position = (Vector3)stream.ReceiveNext();
            rb.rotation = (Quaternion)stream.ReceiveNext();
            rb.velocity = (Vector3)stream.ReceiveNext();

            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            rb.position += rb.velocity * lag;
        }
    }
}
