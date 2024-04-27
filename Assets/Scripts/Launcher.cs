using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using UnityEngine.UI;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using System.Threading.Tasks;
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
    public RoomButton roomButtonPrefab;
    public List<RoomButton> roomButtons = new List<RoomButton>();
    public TMP_Text playerNameLabel;
    private List<TMP_Text> allPlayerNames = new List<TMP_Text>();
    public GameObject startGameButton;
    public GameObject roomTestButton;
    public TMP_InputField nameInput;
    private bool hasSetNickName = false;
    public GameObject nameScreen;
    // the game scene
    public string levelToPlay;


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
    async void Start()
    {
        await UnityServices.InitializeAsync();
       await SignInAnonymously();
        PhotonNetwork.AutomaticallySyncScene = true;
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
        nameScreen.SetActive(false);
    }
    async Task SignInAnonymously()
    {
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in as: " + AuthenticationService.Instance.PlayerId);
        };
        AuthenticationService.Instance.SignInFailed += s =>
        {
            // Take some action here...
            Debug.Log(s);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
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

        PhotonNetwork.NickName = "Player" + Random.Range(1000, 10000);

        if(!hasSetNickName)
        {
            CloseMenuItems();
            nameScreen.SetActive(true);

            if(PlayerPrefs.HasKey("NickName"))
            {
                nameInput.text = PlayerPrefs.GetString("NickName");
            }
        }

        else
        {
            PhotonNetwork.NickName = PlayerPrefs.GetString("NickName");
        }
    }

    public void CreateRoom()
    {
        if(!PhotonNetwork.IsConnected)
        {
            Debug.Log("Not connected to server");
            return;
        }

        if(string.IsNullOrEmpty(roomNameInput.text))
        {
            return;
        }

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;

        PhotonNetwork.CreateRoom(roomNameInput.text, options);
        CloseMenuItems();
        loadingScreen.SetActive(true);
        loadingText.text = "Creating room...";
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room created successfully");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create room: " + message);
    }

    public override void OnJoinedRoom()
    {
        CloseMenuItems();
        roomScreen.SetActive(true);
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        ListPlayers();
        if(PhotonNetwork.IsMasterClient)
        {
            startGameButton.SetActive(true);
        }

        else
        {
            startGameButton.SetActive(false);
        }
    }

    public void ListPlayers()
    {
        foreach (TMP_Text player in allPlayerNames)
        {
            Destroy(player.gameObject);
        }

        allPlayerNames.Clear();

        Player[] players = PhotonNetwork.PlayerList;

        foreach (Player player in players)
        {
            TMP_Text newPlayer = Instantiate(playerNameLabel, playerNameLabel.transform.parent);
            newPlayer.text = player.NickName;
            newPlayer.gameObject.SetActive(true);
            allPlayerNames.Add(newPlayer);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        TMP_Text playerLabel = Instantiate(playerNameLabel, playerNameLabel.transform.parent);
        playerLabel.text = newPlayer.NickName;
        playerLabel.gameObject.SetActive(true);
        allPlayerNames.Add(playerLabel);

        if(PhotonNetwork.IsMasterClient)
        {
            startGameButton.SetActive(true);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ListPlayers();
        if(PhotonNetwork.IsMasterClient)
        {
            startGameButton.SetActive(true);
        }
    }

    public void SetNickName()
    {
        if(string.IsNullOrEmpty(nameInput.text))
        {
            return;
        }

        PhotonNetwork.NickName = nameInput.text;
        hasSetNickName = true;

        CloseMenuItems();
        menuButtons.SetActive(true);
    }

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // Start the game for all players
            PhotonNetwork.LoadLevel(levelToPlay);
        }
    }


    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            startGameButton.SetActive(true);
        }

        else
        {
            startGameButton.SetActive(false);
        }
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

    

    public void OpenRoomBrowser()
    {
        CloseMenuItems();
        roomBrowserScreen.SetActive(true);
    }

    public void OpenCreateRoomMenu()
    {
        CloseMenuItems();
        createRoomMenu.SetActive(true);
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

    public void CloseBtn()
    {
        CloseMenuItems();
        menuButtons.SetActive(true);
    }


    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Room length: " + roomList.Count);
        // Clear existing room buttons
        foreach (RoomButton button in roomButtons)
        {
            Destroy(button.gameObject);
        }
        roomButtons.Clear();

        roomButtonPrefab.gameObject.SetActive(false);

        for(int i = 0; i < roomList.Count; i++)
        {
            if(roomList[i].PlayerCount != roomList[i].MaxPlayers && !roomList[i].RemovedFromList)
            {
                RoomButton newButton = Instantiate(roomButtonPrefab, roomButtonPrefab.transform.parent);
                newButton.SetRoomInfoDetails(roomList[i]);
                newButton.gameObject.SetActive(true);

                roomButtons.Add(newButton);
            }
        }
    }


    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name); // Make sure the room name matches exactly with the name of the room you want to join
        CloseMenuItems();
        loadingScreen.SetActive(true);
        loadingText.text = "Joining room...";
    }



    public void QuitGame()
    {
        Application.Quit();
    }
    

}
