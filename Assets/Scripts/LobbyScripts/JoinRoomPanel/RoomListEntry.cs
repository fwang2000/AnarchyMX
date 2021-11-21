using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class RoomListEntry : MonoBehaviour
{
    public TextMeshProUGUI RoomNameText;
    public TextMeshProUGUI RoomIDText;
    public TextMeshProUGUI RoomPlayersText;
    public Button JoinRoomButton;

    public GameObject PasswordPanelPrefab;
    private string roomName;
    private string roomPassword;
    private bool passwordEnabled;

    public void Start()
    {
        JoinRoomButton.onClick.AddListener(() =>
        {
            if (passwordEnabled)
            {
                Instantiate(PasswordPanelPrefab, transform.parent);
                PasswordPanelPrefab.GetComponent<PasswordPanel>().SetPasswordPanel(roomPassword, roomName);
                JoinRoomButton.interactable = false;
            }
            else
            {
                JoinRoom();
            }
        });
    }

    public void Initialize(string _roomName, string _roomPassword, bool _passwordEnabled, string id, byte currentPlayers, byte maxPlayers)
    {
        roomName = id;
        roomPassword = _roomPassword;
        passwordEnabled = _passwordEnabled;

        RoomNameText.text = _roomName;
        RoomIDText.text = id.Substring(5);
        RoomPlayersText.text = currentPlayers + " / " + maxPlayers;
        JoinRoomButton.GetComponentInChildren<TextMeshProUGUI>().text = "JOIN";
    }

    private void JoinRoom()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }

        PhotonNetwork.JoinRoom(roomName);
    }
}
