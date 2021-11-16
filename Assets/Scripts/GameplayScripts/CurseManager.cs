using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;

public class CurseManager : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        CurseRandomPlayer();
    }

    public void CurseRandomPlayer()
    {
        Hashtable props = new Hashtable
        {
            { AnarchyGame.PLAYER_CURSED, true }
        };

        int cursedIndex = Random.Range(0, PhotonNetwork.CurrentRoom.PlayerCount);
        PhotonNetwork.PlayerList[cursedIndex].SetCustomProperties(props);
    }

    public void CursePlayer(int actorNumber)
    {
        Hashtable newCurseProps = new Hashtable
        {
            { AnarchyGame.PLAYER_CURSED, true }
        };

        Player cursedPlayer = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber);
        cursedPlayer.SetCustomProperties(newCurseProps);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if ((bool)otherPlayer.CustomProperties[AnarchyGame.PLAYER_CURSED])
        {
            CurseRandomPlayer();
        }
    }
}
