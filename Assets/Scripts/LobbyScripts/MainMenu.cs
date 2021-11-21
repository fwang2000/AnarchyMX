using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class MainMenu : MonoBehaviourPunCallbacks
{
    #region PanelHeaders
    [Header("Login Panel")]
    public GameObject LoginPanel;
    public InputField PlayerNameInput;

    [Header("Selection Panel")]
    public GameObject SelectionPanel;

    [Header("Change Name Panel")]
    public GameObject ChangeNamePanel;

    [Header("Create Room Panel")]
    public GameObject CreateRoomPanel;
    public InputField CreateRoomNameInput;
    public InputField CreateRoomPasswordInput;
    public Toggle CreateRoomPasswordEnabled;

    [Header("Room List Panel")]
    public GameObject RoomListPanel;
    public GameObject RoomListContent;
    public GameObject RoomListEntryPrefab;
    #endregion

    private const string GameVersion = "1.0";
    private const int MaxPlayersPerRoom = 8;

    private Dictionary<string, RoomInfo> cachedRoomList;
    private Dictionary<string, GameObject> roomListEntries;


    #region LobbyStartFunctions
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        cachedRoomList = new Dictionary<string, RoomInfo>();
        roomListEntries = new Dictionary<string, GameObject>();
        StartLobbyScene();
    }

    private void StartLobbyScene()
    {
        if (PhotonNetwork.IsConnected)
        {
            SetActivePanel(SelectionPanel.name);
        }
        else
        {
            SetActivePanel(LoginPanel.name);
        }
    }
    #endregion

    #region PUN Callbacks
    public override void OnConnectedToMaster()
    {
        SetActivePanel(SelectionPanel.name);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearRoomListView();

        UpdateCachedRoomList(roomList);
        UpdateRoomListView();
    }

    public override void OnJoinedLobby()
    {
        cachedRoomList.Clear();
        ClearRoomListView();
    }

    public override void OnLeftLobby()
    {
        cachedRoomList.Clear();
        ClearRoomListView();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        SetActivePanel(SelectionPanel.name);
        Debug.Log($"Disconnected ue to: { cause }");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        SetActivePanel(SelectionPanel.name);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        SetActivePanel(SelectionPanel.name);
    }

    public override void OnJoinedRoom()
    {
        cachedRoomList.Clear();
        SceneManager.LoadScene(SceneManagerHelper.ActiveSceneBuildIndex + 1);
    }
    #endregion

    #region UI Callbacks
    public void OnBackButtonClicked()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
        SetActivePanel(SelectionPanel.name);
    }

    public void OnCreateRoomSettingsButtonClicked()
    {
        SetActivePanel(CreateRoomPanel.name);
    }

    public void OnCreateRoomButtonClicked()
    {
        string roomName = CreateRoomNameInput.text;
        string roomPassword = "";

        if (CreateRoomPasswordEnabled.isOn)
        {
            roomPassword = CreateRoomPasswordInput.text;
            if (string.IsNullOrWhiteSpace(roomPassword))
            {
                CreateRoomPasswordInput.placeholder.GetComponent<Text>().text = "Input Password";
                return;
            }
        }

        if (string.IsNullOrWhiteSpace(roomName))
        {
            roomName = PhotonNetwork.LocalPlayer.NickName + "'s room";
        }

        CreateRoom(roomName, roomPassword, CreateRoomPasswordEnabled.isOn);
    }

    public void OnLoginButtonClicked()
    {
        string playerName = PlayerNameInput.text;

        if (!string.IsNullOrWhiteSpace(playerName))
        {
            PhotonNetwork.LocalPlayer.NickName = playerName;
            PhotonNetwork.GameVersion = GameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            PlayerNameInput.placeholder.GetComponent<Text>().text = "Cannot Be Empty";
        }
    }

    public void OnChangeNamePanelButtonClicked()
    {
        SetActivePanel(ChangeNamePanel.name);
    }

    public void OnChangeNameBackButton()
    {
        SetActivePanel(SelectionPanel.name);
    }

    public void OnRoomListButtonClicked()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
        SetActivePanel(RoomListPanel.name);
    }

    public void OnHomeButtonClicked()
    {
        SetActivePanel(SelectionPanel.name);
    }
    #endregion

    #region Helpers
    private void SetActivePanel(string activePanel)
    {
        LoginPanel.SetActive(activePanel.Equals(LoginPanel.name));
        SelectionPanel.SetActive(activePanel.Equals(SelectionPanel.name));
        ChangeNamePanel.SetActive(activePanel.Equals(ChangeNamePanel.name));
        CreateRoomPanel.SetActive(activePanel.Equals(CreateRoomPanel.name));
        RoomListPanel.SetActive(activePanel.Equals(RoomListPanel.name));
    }
    private void CreateRoom(string roomName, string roomPassword, bool passwordEnabled)
    {
        string[] nicknames = new string[8] { "", "", "", "", "", "", "", "" };
        string roomID = "Room " + Random.Range(1000, 9999);

        PhotonNetwork.CreateRoom(roomID, new RoomOptions
        {
            MaxPlayers = MaxPlayersPerRoom,
            CustomRoomPropertiesForLobby = new string[3] { "roomName", "roomPassword", "passwordEnabled" },
            CustomRoomProperties = new Hashtable
                {
                    { "roomName", roomName.ToUpper().Trim() },
                    { "roomPassword", roomPassword.ToUpper().Trim() },
                    { "passwordEnabled", passwordEnabled },
                    { "moveSpeed", 10f },
                    { "roundTime", 5f },
                    { "curseTime", 60.0f},
                    { "artifactsDR", "LOW"},
                    { "nicknames", nicknames },
                    { AnarchyGame.MOVEMENT_ENABLED, true }
                },
            PlayerTtl = 0,
            EmptyRoomTtl = 0,
        });
    }

    private void ClearRoomListView()
    {
        foreach (GameObject entry in roomListEntries.Values)
        {
            Destroy(entry.gameObject);
        }

        roomListEntries.Clear();
    }

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            // Remove room from cached room list if it got closed, became invisible or was marked as removed
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
            {
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList.Remove(info.Name);
                }

                continue;
            }

            // Update cached room info
            if (cachedRoomList.ContainsKey(info.Name))
            {
                cachedRoomList[info.Name] = info;
            }
            // Add new room info to cache
            else
            {
                cachedRoomList.Add(info.Name, info);
            }
        }
    }

    private void UpdateRoomListView()
    {
        foreach (RoomInfo info in cachedRoomList.Values)
        {
            GameObject entry = Instantiate(RoomListEntryPrefab);
            entry.transform.SetParent(RoomListContent.transform);
            entry.transform.localScale = Vector3.one;
            Debug.Log((string)info.CustomProperties["roomName"]);
            Debug.Log((string)info.CustomProperties["roomPassword"]);
            Debug.Log((bool)info.CustomProperties["passwordEnabled"]);

            entry.GetComponent<RoomListEntry>().Initialize(
                (string)info.CustomProperties["roomName"],
                (string)info.CustomProperties["roomPassword"],
                (bool)info.CustomProperties["passwordEnabled"],
                info.Name, 
                (byte)info.PlayerCount, 
                info.MaxPlayers);

            roomListEntries.Add(info.Name, entry);
        }
    }
    #endregion
}