using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;

public class RoomButton : MonoBehaviour
{

    public TMP_Text roomNameText;
    private RoomInfo roomInfo;
    
    public void SetRoomInfoDetails(RoomInfo info)
    {
        roomInfo = info;
        roomNameText.text = info.Name;
    }

    public void OpenRoom()
    {
        Launcher.instance.JoinRoom(roomInfo);
    }
}
