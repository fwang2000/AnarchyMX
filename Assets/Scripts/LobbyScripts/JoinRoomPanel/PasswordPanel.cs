using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class PasswordPanel : MonoBehaviour
{
    private string roomPassword;
    private string roomName;

    public TMP_InputField passwordInput;

    public void SetPasswordPanel(string password, string roomName)
    {
        roomPassword = password;
        this.roomName = roomName;
    }

    public void OnEnterButtonClicked()
    {
        if (roomPassword == passwordInput.text.Trim().ToUpper())
        {
            JoinRoom();
        }
        else
        {
            passwordInput.placeholder.GetComponent<TextMeshProUGUI>().text = "Wrong Password";
            passwordInput.text = string.Empty;
        }
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
