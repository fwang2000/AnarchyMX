using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private GameObject spawnpoints;

    private string prefabName;

    private GameObject playerPrefab;
    private GameObject playerModel;
    /*
    private void Start()
    {
        StartCoroutine("InstantiatePlayerPrefab");
    }
    */

    public void InstantiatePlayer(string prefabColor)
    {
        StartCoroutine("InstantiatePlayerPrefab", prefabColor);
    }

    private IEnumerator InstantiatePlayerPrefab(string prefabColor)
    {
        yield return new WaitUntil(() => PhotonNetwork.LocalPlayer.GetPlayerNumber() != -1);

        Vector3 spawnPosition = SpawnpointController.singletonInstance.spawnpoints[PhotonNetwork.LocalPlayer.GetPlayerNumber()].position + new Vector3(0, 2, 0);

        if (playerPrefab)
        {
            spawnPosition = playerPrefab.transform.position;
            PhotonNetwork.Destroy(playerPrefab);
        }

        prefabName = "CharacterModels/" + prefabColor + "PlayerModelVariant";
        playerPrefab = PhotonNetwork.Instantiate(prefabName, spawnPosition, Quaternion.identity);
        Camera.main.GetComponent<CameraFollowPlayer>().SetPlayer(playerPrefab);
    }
}
