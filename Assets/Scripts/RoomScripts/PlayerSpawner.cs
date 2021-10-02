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

    private static GameObject playerPrefab;
    private static GameObject playerModel;

    private void Start()
    {
        StartCoroutine("InstantiatePlayerPrefab");
    }

    private IEnumerator InstantiatePlayerPrefab()
    {
        yield return new WaitUntil(() => PhotonNetwork.LocalPlayer.GetPlayerNumber() != -1);

        Vector3 spawnPosition = SpawnpointController.singletonInstance.spawnpoints[PhotonNetwork.LocalPlayer.GetPlayerNumber()].position + new Vector3(0, 2, 0);
        playerPrefab = PhotonNetwork.Instantiate(prefabName, spawnPosition, Quaternion.identity);
    }

    public void InstantiatePlayerModel(string prefabColor)
    {
        if (playerModel)
        {
            Destroy(playerModel);
        }
        playerModel = (GameObject)Instantiate(Resources.Load("CharacterModels/" + prefabColor + "PlayerModel"), playerPrefab.transform.position, Quaternion.identity, playerPrefab.transform);
    }
}
