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
    private PlayerSpawner playerSpawner;
    private string characterProp = "character";

    // Start is called before the first frame update
    void Start()
    {
        playerSpawner = GameObject.Find("PlayerSpawner").GetComponent<PlayerSpawner>();
    }

    public void OnPlayerButtonClicked()
    {
        string oldCharacterName = (string)PhotonNetwork.LocalPlayer.CustomProperties[characterProp];
        if (string.IsNullOrEmpty(oldCharacterName))
        {
            GetComponent<PhotonView>().RPC("ActivateButton", RpcTarget.AllBuffered, oldCharacterName);
        }

        string characterColor = EventSystem.current.currentSelectedGameObject.name;
        playerSpawner.InstantiatePlayerModel(characterColor);
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
        Hashtable playerProps = new Hashtable();
        playerProps.Add(characterProp, characterName);
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);
    }
}
