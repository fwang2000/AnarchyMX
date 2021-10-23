using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NametagManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TextMeshProUGUI nickname;

    // Start is called before the first frame update
    void Start()
    {
        SetName();
    }

    private void SetName()
    {
        nickname.text = photonView.Owner.NickName;
    }

    private void MakeNicknameUnique()
    {
        int duplicate = 0;

        string[] nicknames = (string[])PhotonNetwork.CurrentRoom.CustomProperties["nicknames"];

        while (nicknames.Contains(PhotonNetwork.LocalPlayer.NickName))
        {
            ++duplicate;
            if (duplicate == 1)
            {
                PhotonNetwork.LocalPlayer.NickName = PhotonNetwork.LocalPlayer.NickName + " (" + duplicate + ")";
            }
            else
            {
                string currNickname = PhotonNetwork.LocalPlayer.NickName;
                PhotonNetwork.LocalPlayer.NickName = currNickname.Substring(0, currNickname.Length - 4) + " (" + duplicate + ")";
            }
        }

        nicknames[PhotonNetwork.LocalPlayer.GetPlayerNumber()] = PhotonNetwork.LocalPlayer.NickName;

        Hashtable setValue = new Hashtable();
        setValue.Add("nicknames", nicknames);

        PhotonNetwork.CurrentRoom.SetCustomProperties(setValue);
    }
}
