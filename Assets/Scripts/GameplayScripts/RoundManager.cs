using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RoundManager : MonoBehaviour
{
    public static RoundManager singletonInstance;

    void Awake()
    {
        singletonInstance = this;
    }

    private void Start()
    {
        AnarchyGame.FreezeAllPlayers(true);
    }
}
