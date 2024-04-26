using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject secondPlayerPrefab;
    
    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("Name: " + PhotonNetwork.LocalPlayer.NickName);
                PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 0, 0), Quaternion.identity);
            }
            else
            {
                Debug.Log("Name: " + PhotonNetwork.LocalPlayer.NickName);
                PhotonNetwork.Instantiate(secondPlayerPrefab.name, new Vector3(0, 0, 0), Quaternion.identity);
            }
        }
    }
}
