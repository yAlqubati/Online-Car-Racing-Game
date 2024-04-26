using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject SecondPlayerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        // if it is the master client
        if(PhotonNetwork.IsMasterClient)
        {
            // spawn the player prefab
            PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 0, 0), Quaternion.identity);
        }
        else
        {
            // spawn the second player prefab
            PhotonNetwork.Instantiate(SecondPlayerPrefab.name, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }
}
