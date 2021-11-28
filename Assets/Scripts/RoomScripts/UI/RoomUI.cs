using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RoomUI : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject StartButton;
    [SerializeField]
    private GameObject CharacterSelect;

    private bool characterSelectActive = false;

    public bool characterSelected = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("ActivateStartButton");
    }

    IEnumerator ActivateStartButton()
    {
        yield return new WaitUntil(() => characterSelected);
        StartButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public void OnStartButtonClicked()
    {
        PhotonNetwork.LoadLevel("GameScene");
    }

    public void OnLeaveRoomButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void OnCharacterSelectButtonClicked()
    {
        CharacterSelect.SetActive(characterSelectActive = !characterSelectActive);
    }
}
