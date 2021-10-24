using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using UnityEngine.EventSystems;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class CharacterSelect : MonoBehaviour
{
    [SerializeField]
    private GameObject CharacterSelectButton;
    private PlayerManager playerSpawner;

    private RoomTimer roomTimer;

    // Start is called before the first frame update
    void Start()
    {
        playerSpawner = GameObject.Find("PlayerSpawner").GetComponent<PlayerManager>();
        roomTimer = GameObject.Find("Timer").GetComponent<RoomTimer>();
    }

    public void OnPlayerButtonClicked()
    {
        string oldCharacterName = (string)PhotonNetwork.LocalPlayer.CustomProperties[AnarchyGame.PLAYER_COLOR];
        if (!string.IsNullOrEmpty(oldCharacterName))
        {
            GetComponent<PhotonView>().RPC("ActivateButton", RpcTarget.AllBuffered, oldCharacterName);
        }
        else
        {
            CharacterSelectButton.SetActive(true);
            ReadyPlayer();
            roomTimer.DisableTimer();
        }

        string characterColor = EventSystem.current.currentSelectedGameObject.name;
        playerSpawner.InstantiatePlayer(characterColor);
        SetPlayerModelProp(characterColor);
        GetComponent<PhotonView>().RPC("DisableButton", RpcTarget.AllBuffered, characterColor);

        transform.gameObject.SetActive(false);
    }

    [PunRPC]
    private void DisableButton(string name)
    {
        Button currButton = GetCharacterButton(name);
        if (currButton)
        {
            currButton.interactable = false;
        }
    }

    [PunRPC]
    private void ActivateButton(string name)
    {
        Button currButton = GetCharacterButton(name);
        if (currButton)
        {
            currButton.interactable = true;
        }
    }

    private Button GetCharacterButton(string name)
    {
        foreach (Transform child in transform)
        {
            if (child.name == name)
            {
                return child.gameObject.GetComponent<Button>();
            }
        }
        Debug.LogError("cannot find button - returned null");
        return null;
    }

    private void SetPlayerModelProp(string characterName)
    {
        Hashtable props = new Hashtable();
        props.Add(AnarchyGame.PLAYER_COLOR, characterName);
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    private void ReadyPlayer()
    {
        Hashtable props = new Hashtable
        {
            { AnarchyGame.PLAYER_READY, true }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }
}
