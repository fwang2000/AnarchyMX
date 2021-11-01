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
    #region CHARACTER PREVIEW ITEMS
    private Button selectButton;
    private GameObject characterTemplate;
    #endregion

    [SerializeField]
    private GameObject CharacterSelectPanel;
    private PlayerManager playerSpawner;
    private RoomTimer roomTimer;
    private GameObject UIPanel;
    private string currentColor;
    private bool characterPreviewSelected;

    private void Awake()
    {
        playerSpawner = GameObject.Find("PlayerSpawner").GetComponent<PlayerManager>();
        roomTimer = GameObject.Find("Timer").GetComponent<RoomTimer>();
        selectButton = GameObject.Find("SelectCharacterButton").GetComponent<Button>();
        characterTemplate = GameObject.Find("CharacterTemplate");
        UIPanel = GameObject.Find("UIPanel");
    }

    // Start is called before the first frame update
    void Start()
    {
        characterTemplate.SetActive(false);
        selectButton.interactable = false;
    }

    private string GetFirstAvailableColor()
    {
        foreach (Transform t in CharacterSelectPanel.transform)
        {
            if (t.gameObject.GetComponent<Button>().interactable)
            {
                return t.name;
            }
        }
        throw new System.Exception("No available character");
    }

    public void OnPlayerButtonClicked()
    {
        currentColor = EventSystem.current.currentSelectedGameObject.name;
        characterPreviewSelected = true;
        selectButton.interactable = true;
        SetPlayerPreview();
    }

    private void SetPlayerPreview()
    {
        string oldCharacterName = (string)PhotonNetwork.LocalPlayer.CustomProperties[AnarchyGame.PLAYER_COLOR];
        if (!string.IsNullOrEmpty(oldCharacterName))
        {
            GetComponent<PhotonView>().RPC("ActivateButton", RpcTarget.AllBuffered, oldCharacterName);
        }

        SetPlayerModelProp(currentColor);
        GetComponent<PhotonView>().RPC("DisableButton", RpcTarget.AllBuffered, currentColor);
    }

    public void OnSelectCharacterButtonClicked()
    {
        ReadyPlayer();
        roomTimer.DisableTimer(); 
        playerSpawner.InstantiatePlayer(currentColor);
        characterTemplate.SetActive(false);
        UIPanel.SetActive(false);
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
        foreach (Transform child in CharacterSelectPanel.transform)
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

    public void OnPointerEnter(GameObject button)
    {
        if (button.GetComponent<Button>().interactable)
        {
            if (!characterPreviewSelected)
            {
                characterTemplate.SetActive(true);
            }
            characterTemplate.GetComponent<Renderer>().material = (Material)Resources.Load("Materials/" + button.name);
        }
    }

    public void OnPointerExit()
    {
        if (!characterPreviewSelected)
        {
            characterTemplate.SetActive(false);
        }
        else
        {
            characterTemplate.GetComponent<Renderer>().material = (Material)Resources.Load("Materials/" + currentColor);
        }
    }
}
