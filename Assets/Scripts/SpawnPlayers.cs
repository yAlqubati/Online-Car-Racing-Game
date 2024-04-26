using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
        PhotonNetwork.Instantiate(playerPrefab.name, pos, Quaternion.identity);
    }
}
