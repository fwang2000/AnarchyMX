using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject spawnpoints;

    private string prefabName = "PlayerPrefab";

    public void InstantiatePlayer(string prefabColor)
    {
        PhotonNetwork.Instantiate(prefabColor + prefabName, SpawnpointController.singletonInstance.spawnpoints[PhotonNetwork.LocalPlayer.GetPlayerNumber()].position, Quaternion.identity);
    }
}
