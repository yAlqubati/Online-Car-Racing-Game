using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher instance;
    public GameObject loadingScreen;
    public GameObject menuButtons;
    public GameObject createRoomMenu;
    public GameObject roomScreen;
    public GameObject errorScreen;
    public GameObject roomBrowserScreen;
    public TMP_Text loadingText;
    public TMP_InputField roomNameInput;
    public TMP_Text roomNameText;
    public TMP_Text errorText;
    // public RoomButton roomButtonPrefab;
    public List<RoomButton> roomButtons = new List<RoomButton>();

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        else
        {
            Destroy(this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        CloseMenuItems();
        loadingScreen.SetActive(true);
        loadingText.text = "Connecting to server...";
        PhotonNetwork.ConnectUsingSettings();
    }


    public void CloseMenuItems()
    {
        loadingScreen.SetActive(false);
        menuButtons.SetActive(false);
        createRoomMenu.SetActive(false);
        roomScreen.SetActive(false);
        errorScreen.SetActive(false);
        roomBrowserScreen.SetActive(false);
    }

    

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Connected to master server");
        loadingText.text = "Connected to server. Joining lobby...";
        PhotonNetwork.JoinLobby();
        loadingText.text = "Joined lobby.";
    }

    public override void OnJoinedLobby()
    {
        loadingText.text = "Joined lobby.";
        CloseMenuItems();
        menuButtons.SetActive(true);
    }

    public void CreateRoom()
    {
        if(string.IsNullOrEmpty(roomNameInput.text))
        {
            return;
        }

        // to be continued
    }

    public override void OnJoinedRoom()
    {
        CloseMenuItems();
        roomScreen.SetActive(true);
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        CloseMenuItems();
        loadingScreen.SetActive(true);
        loadingText.text = "Leaving room...";
    }

    public override void OnLeftRoom()
    {
        CloseMenuItems();
        menuButtons.SetActive(true);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorScreen.SetActive(true);
        errorText.text = "Failed to create room: " + message;
    }

    public void OpenRoomBrowser()
    {
        CloseMenuItems();
        roomBrowserScreen.SetActive(true);
    }

    public void CloseRoomBrowser()
    {
        CloseMenuItems();
        menuButtons.SetActive(true);
    }

    public void OpenFindRoomMenu()
    {
        CloseMenuItems();
        roomBrowserScreen.SetActive(true);
    }
}
