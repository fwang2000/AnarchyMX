using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ArtifactControlScript : MonoBehaviour
{

    [SerializeField]
    private GameObject ArtifactCanvas;
    private GameObject ArtifactButton;

    private GameObject Beacon;
    private Color openBeaconColor = new Color(0, 252, 255);
    private Color activeBeaconColor = new Color(1, 0, 0.108f);

    private GameObject PlayerCapsule;

    private bool artifactUsed = false;
    private bool timerIsRunning = true;
    private float timeRemaining = 10.0f;


    private void Awake()
    {
        Beacon = transform.Find("Beacon").gameObject;
        ArtifactButton = ArtifactCanvas.transform.Find("ArtifactButton").gameObject;
    }

    private void Start()
    {
        ArtifactCanvas.SetActive(false);
        Beacon.GetComponent<Renderer>().material.color = openBeaconColor;
        Debug.Log(Beacon.GetComponent<Renderer>().material.color);
    }

    private void Update()
    {
        CheckPlayerDistance();
    }

    private void CheckPlayerDistance()
    {
        if (PlayerCapsule)
        {
            float distance = Vector3.Distance(PlayerCapsule.transform.position, transform.position);
            ArtifactCanvas.SetActive(distance <= 5.0f);
        }
        else
        {
            PlayerCapsule = PlayerController.LocalPlayerGameObject;
        }
    }

    public void OnTakeArtifactButtonClicked()
    {
        GetComponent<PhotonView>().RPC("TakeArtifact", RpcTarget.AllBuffered, true);
    }

    [PunRPC]
    void TakeArtifact(bool isArtifactTaken)
    {
        ArtifactButton.GetComponent<Button>().interactable = false;
        Beacon.GetComponent<Renderer>().material.color = activeBeaconColor;
    }
}
