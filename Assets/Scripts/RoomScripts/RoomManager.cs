using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject StartButton;
    private const int MaxPlayersPerRoom = 3;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    #region PUN Callbacks
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(SceneManagerHelper.ActiveSceneBuildIndex - 1);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount >= MaxPlayersPerRoom - 2)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                StartButton.GetComponent<Button>().interactable = true;
            }
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayersPerRoom)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount < MaxPlayersPerRoom - 2)
            {
                StartButton.GetComponent<Button>().interactable = false;
            }
            PhotonNetwork.DestroyPlayerObjects(otherPlayer);
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            StartButton.SetActive(true);
        }
    }
    #endregion

}
