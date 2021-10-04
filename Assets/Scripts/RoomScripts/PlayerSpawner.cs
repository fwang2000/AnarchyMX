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

    private GameObject playerPrefab;
    private GameObject playerModel;

    private void Start()
    {
        StartCoroutine("InstantiatePlayerPrefab");
    }

    private IEnumerator InstantiatePlayerPrefab()
    {
        yield return new WaitUntil(() => PhotonNetwork.LocalPlayer.GetPlayerNumber() != -1);

        Vector3 spawnPosition = SpawnpointController.singletonInstance.spawnpoints[PhotonNetwork.LocalPlayer.GetPlayerNumber()].position + new Vector3(0, 2, 0);
        playerPrefab = PhotonNetwork.Instantiate(prefabName, spawnPosition, Quaternion.identity);
        Camera.main.GetComponent<CameraFollowPlayer>().SetPlayer(playerPrefab);
    }

    public void InstantiatePlayerModel(string prefabColor)
    {
        if (playerModel)
        {
            Destroy(playerModel);
        }
        playerModel = PhotonNetwork.Instantiate("CharacterModels/" + prefabColor + "PlayerModel", playerPrefab.transform.position, Quaternion.identity);
        playerModel.transform.parent = playerPrefab.transform;
        GetComponent<PhotonView>().RPC("ActivateNametag", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void ActivateNametag()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            player.transform.Find("Nametag").gameObject.SetActive(true);
        }
    }
}
