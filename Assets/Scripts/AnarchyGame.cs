using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class AnarchyGame : MonoBehaviour
{
    public const string PLAYER_READY = "IsPlayerReady";
    public const string PLAYER_LOADED_LEVEL = "IsPlayerLoadedLevel";
    public const string PLAYER_COLOR = "Player Color";
    public const string PLAYER_CURSED = "Player Cursed";

    public const string START_TIMER_PROP = "StartTime";
    public const string ROUND_TIMER_PROP = "RoundTime";
    public const string CURSE_TIMER_PROP = "CurseTime";

    public const string MOVEMENT_ENABLED = "MovementEnabled";

    public static void FreezeAllPlayers(bool isMovementAllowed)
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            Hashtable props = new Hashtable {
                {
                    AnarchyGame.MOVEMENT_ENABLED, !isMovementAllowed
                }
            };
            player.SetCustomProperties(props);
        }
    }
}
