using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System;

public class AnarchyGameManager : MonoBehaviourPunCallbacks
{
    public static AnarchyGameManager singletonInstance;

    public Text timerText;

    [SerializeField] private GameObject playerSpawner;

    void Awake()
    {
        singletonInstance = this;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        CountdownTimer.OnCountdownTimerHasExpired += OnCountdownTimerHasExpired;
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
        CountdownTimer.OnCountdownTimerHasExpired -= OnCountdownTimerHasExpired;
    }

    private void OnCountdownTimerHasExpired()
    {
        return;
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
        bool startTimeIsSet = CountdownTimer.TryGetStartTime(out startTimestamp);

        if (changedProps.ContainsKey(AnarchyGame.PLAYER_LOADED_LEVEL))
        {
            if (AllPlayersLoaded())
            {
                if (!startTimeIsSet)
                {
                    CountdownTimer.SetStartTime();
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
