using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    #region CHARACTER MOVEMENT
    private Rigidbody rb;
    private float moveSpeed;
    private float smoothTime;
    private Vector3 forward, right, upMovement, rightMovement;

    private bool grounded;
    private bool movementEnabled;
    private Vector3 smoothMoveVelocity;
    private Vector3 moveAmount;
    #endregion
    #region CURSE
    private CurseManager curseManager;
    #endregion

    private int playerID;
    private PhotonView PV;
    public static GameObject LocalPlayerGameObject;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();

        if (!PV.IsMine)
        {
            return;
        }

        // curseManager = GameObject.Find("CurseManager").GetComponent<CurseManager>();
        PlayerController.LocalPlayerGameObject = this.gameObject;
    }

    private void Start()
    {
        playerID = PhotonNetwork.LocalPlayer.ActorNumber;

        smoothTime = 0.1f;
        moveSpeed = 20f;
        movementEnabled = (bool)PhotonNetwork.CurrentRoom.CustomProperties[AnarchyGame.MOVEMENT_ENABLED];

        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);

        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
    }

    // Update is called once per frames
    void Update()
    {
        if (!PV.IsMine)
        {
            return;
        }

        if (!movementEnabled)
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

        if (!movementEnabled)
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

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Player")
        {
            GameObject collidedPlayer = hit.gameObject;
            int otherPlayerId = collidedPlayer.GetComponent<PlayerController>().GetPlayerID();

            if ((bool)PhotonNetwork.LocalPlayer.CustomProperties[AnarchyGame.PLAYER_CURSED])
            {
                Hashtable oldCurseProps = new Hashtable
                {
                    { AnarchyGame.PLAYER_CURSED, false }
                };

                PhotonNetwork.LocalPlayer.SetCustomProperties(oldCurseProps);

                // curseManager.CursePlayer(otherPlayerId);
            }
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (targetPlayer.ActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
        {
            return;
        }

        if (changedProps.ContainsKey(AnarchyGame.MOVEMENT_ENABLED))
        {
            movementEnabled = (bool)changedProps[AnarchyGame.MOVEMENT_ENABLED];
        }
    }

    #region GET/SET
    public int GetPlayerID()
    {
        return playerID;
    }
    #endregion
}
