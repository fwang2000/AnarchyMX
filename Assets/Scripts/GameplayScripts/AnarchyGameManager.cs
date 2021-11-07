using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System;

public class AnarchyGameManager : MonoBehaviourPunCallbacks
{
    public static AnarchyGameManager singletonInstance;

    public TextMeshProUGUI timerText;

    [SerializeField] private GameObject playerSpawner;
    [SerializeField] private StartTimer startTimer;

    void Awake()
    {
        singletonInstance = this;
    }

    public override void OnEnable()
    {
        base.OnEnable();
    }

    private void Start()
    {
        Hashtable props = new Hashtable
        {
            { AnarchyGame.PLAYER_LOADED_LEVEL, true }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        playerSpawner.GetComponent<PlayerManager>().InstantiatePlayer((string)PhotonNetwork.LocalPlayer.CustomProperties[AnarchyGame.PLAYER_COLOR]);
    }

    public override void OnDisable()
    {
        base.OnDisable();
    }

    #region PUN CALLBACKS
    public override void OnDisconnected(DisconnectCause cause)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LobbyScene");
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        int startTimestamp;
        bool startTimeIsSet = startTimer.TryGetStartTime(out startTimestamp);

        if (changedProps.ContainsKey(AnarchyGame.PLAYER_LOADED_LEVEL))
        {
            if (AllPlayersLoaded())
            {
                if (!startTimeIsSet)
                {
                    startTimer.SetStartTime();
                }
                else
                {
                    timerText.text = "Waiting for other players...";
                }
            }
        }
    }

    private bool AllPlayersLoaded()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            object playerLoaded;

            if (player.CustomProperties.TryGetValue(AnarchyGame.PLAYER_LOADED_LEVEL, out playerLoaded))
            {
                if ((bool)playerLoaded)
                {
                    continue;
                }
            }

            return false;
        }

        return true;
    }
    #endregion
}
