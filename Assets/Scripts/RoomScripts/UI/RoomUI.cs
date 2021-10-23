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

    // Start is called before the first frame update
    void Start()
    {
        ActivateStartButton();
    }

    private void ActivateStartButton()
    {
        StartButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public void OnStartButtonClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("GameScene");
        }
    }

    public void OnCharacterSelectButtonClicked()
    {
        CharacterSelect.SetActive(characterSelectActive = !characterSelectActive);
    }
}
