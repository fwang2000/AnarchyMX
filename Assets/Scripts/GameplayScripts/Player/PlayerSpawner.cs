using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject spawnpoints;

    private string prefabName;

    private GameObject playerPrefab;

    public void InstantiatePlayer(string prefabColor)
    {
        StartCoroutine(InstantiatePlayerPrefab(prefabColor, false));
    }
    public void InstantiateCursedPlayer(string prefabColor)
    {
        StartCoroutine(InstantiatePlayerPrefab(prefabColor, true));
    }

    private IEnumerator InstantiatePlayerPrefab(string prefabColor, bool cursed)
    {
        yield return new WaitUntil(() => PhotonNetwork.LocalPlayer.GetPlayerNumber() != -1);

        Vector3 spawnPosition = SpawnpointController.singletonInstance.spawnpoints[PhotonNetwork.LocalPlayer.GetPlayerNumber()].position;
        prefabName = "CharacterModels/" + prefabColor + "PlayerModelVariant";
         

        if (playerPrefab)
        {
            if (cursed)
            {
                spawnPosition = Vector3.zero;
                prefabName = "CharacterModels/" + prefabColor + "PlayerModelCurseVariant";
            }
            else
            {
                spawnPosition = playerPrefab.transform.position;
            }
            PhotonNetwork.Destroy(playerPrefab);
        }

        playerPrefab = PhotonNetwork.Instantiate(prefabName, spawnPosition + new Vector3(0, 2, 0), Quaternion.identity);
        Camera.main.GetComponent<CameraFollowPlayer>().SetPlayer(playerPrefab);
    }

}
