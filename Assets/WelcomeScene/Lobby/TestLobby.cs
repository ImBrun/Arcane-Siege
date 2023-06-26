using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using TMPro;

public class TestLobby : MonoBehaviour
{
    public Relay TestRelay;
    public TMP_Text LobbyIdText;
    public TMP_InputField CodeInput;
    public TMP_InputField NameInput;
    public TMP_Text Connected_Player_Name1;
    public TMP_Text Connected_Player_Name2;
    public TMP_Text Connected_Player_Name3;
    public GameObject errorPanel;
    public GameObject menuPanel;
    public GameObject StartButton;
    public UserNameInputScript NameCodeScript;
    public TMP_Text ErrorText;
    public TMP_Text UID;
    [SerializeField]
    Behaviour[] compsToHide;
    [SerializeField]
    Behaviour[] compsToShow;
    private Lobby hostLobby;
    private Lobby joinedLobby;
    private float heartbeatTimer;
    private float playerRefreshTimer;


    private async void Start() {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log("Signed in as " + AuthenticationService.Instance.PlayerId);
            UID.text = AuthenticationService.Instance.PlayerId;
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private void Update() {
        HandleLobbyHeartbeat();
        HandlePlayerListRefresh();
    }

    private async void HandleLobbyHeartbeat() {
        if (hostLobby != null ) {
            heartbeatTimer -= Time.deltaTime;
            if (heartbeatTimer < 0f) {
                float heartbeatTimerMax = 15;
                heartbeatTimer = heartbeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }
    }

    private async void HandlePlayerListRefresh() {
        if (joinedLobby != null) {
            playerRefreshTimer -= Time.deltaTime;
            if(playerRefreshTimer < 0f) {
                float playerRefreshTimerMax = 1.1f;
                playerRefreshTimer = playerRefreshTimerMax;

                Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
                joinedLobby = lobby;
                Debug.Log(joinedLobby.Players.Count);
                PrintPlayers(lobby);
            }

            if(joinedLobby.Data["KEY_START_GAME"].Value != "0") {
                if(!IsLobbyHost()) {
                    TestRelay.JoinRelay(joinedLobby.Data["KEY_START_GAME"].Value);
                }
                menuPanel.SetActive(false);
                joinedLobby = null;
            }
        }
    }

    public async void CreateLobby() {
        try {
            string lobbyName = "MyLobby";
            int maxPlayers = 3;
            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions {
                IsPrivate = true,
                Player = GetPlayer(),
                Data = new Dictionary<string, DataObject> {
                    {"GameMode", new DataObject(DataObject.VisibilityOptions.Public, "GetFucked", DataObject.IndexOptions.S1)},
                    { "KEY_START_GAME", new DataObject(DataObject.VisibilityOptions.Member, "0")}
                }
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createLobbyOptions);

            hostLobby = lobby;
            joinedLobby = hostLobby;

            StartButton.SetActive(true);

            LobbyIdText.text = "Lobby code: " + (lobby.LobbyCode.ToString());

            Debug.Log("Created Lobby! " + lobby.Name + " " + lobby.MaxPlayers + " " + lobby.Id + " " + lobby.LobbyCode);
        } catch (LobbyServiceException e) {
            Debug.Log(e);
        }

    }

    public async void ListLobbies() {
        try {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions {
                Count = 10,
                Filters = new List<QueryFilter> {
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
                },
                Order = new List<QueryOrder> {
                    new QueryOrder(false, QueryOrder.FieldOptions.Created)
                }
            };
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();
            foreach (Lobby lobby in queryResponse.Results) {
                Debug.Log(lobby.Name + " " + lobby.MaxPlayers);
            }
        } catch (LobbyServiceException e) {
            Debug.Log(e);
        }
    }

    public async void JoinLobby() {
        try {
            bool codeBool = NameCodeScript.CheckCode(), nameBool = NameCodeScript.CheckName();
            if (codeBool && nameBool) {
                JoinButtonFct();
                JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions {
                    Player = GetPlayer()
                };

                joinedLobby = await Lobbies.Instance.JoinLobbyByCodeAsync(CodeInput.text, joinLobbyByCodeOptions);
                
                Debug.Log("Joined");
            }
        } catch (LobbyServiceException e) {
            if(e.Message.Contains("lobby not found")){
                CodeInput.image.color = new Color(1f, 0f, 0f, 0.4f);
                errorPanel.SetActive(true);
                ErrorText.text = "Lobby not found!";
            }
            else if(e.Message.Contains("invalid character")){
                CodeInput.image.color = new Color(1f, 0f, 0f, 0.4f);
                errorPanel.SetActive(true);
                ErrorText.text = "Invalid code!";
            }
            JoinButtonFctFail();
            Debug.Log(e);
        }
    }

    public void PrintPlayers(Lobby lobby) {
        Connected_Player_Name1.text = lobby.Players[0].Data["PlayerName"].Value;
        if(lobby.Players.Count==2) {
            Connected_Player_Name2.text = lobby.Players[1].Data["PlayerName"].Value;
            Connected_Player_Name2.gameObject.SetActive(true);
        }
        else if(lobby.Players.Count==3) {
            Connected_Player_Name3.text = lobby.Players[2].Data["PlayerName"].Value;
            Connected_Player_Name3.gameObject.SetActive(true);
        }
        else {
            Connected_Player_Name2.text = "";
            Connected_Player_Name2.gameObject.SetActive(false);
            Connected_Player_Name3.text = "";
            Connected_Player_Name3.gameObject.SetActive(false);
        }

    }

    public async void LeaveLobby() {
        try {
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
            hostLobby = null;
            joinedLobby = null;
        } catch (LobbyServiceException e) {
            Debug.Log(e);
        }
    }

    private bool IsLobbyHost() {
        if(AuthenticationService.Instance.PlayerId == joinedLobby.HostId) 
        {
            return true;
        }
        return false;
    }

    private Player GetPlayer() {
        return new Player() {
                    Data = new Dictionary<string, PlayerDataObject> {
                        { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, NameInput.text)}
                    }
                };
    }


    public async void StartGame() {
        if(IsLobbyHost()) {
            try {
                string relayCode = await TestRelay.CreateRelay();

                Lobby lobby = await Lobbies.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions {
                    Data = new Dictionary<string, DataObject> {
                        { "KEY_START_GAME", new DataObject(DataObject.VisibilityOptions.Member, relayCode) }
                    }
                });

                joinedLobby = lobby;
            } catch (LobbyServiceException e) {
                Debug.Log(e);
            }
        }
    }

    public void JoinButtonFct() {
        for(int i = 0; i<compsToHide.Length; i++) {
            compsToHide[i].gameObject.SetActive(false);
        }
        for(int i = 0; i<compsToShow.Length; i++) {
            compsToShow[i].gameObject.SetActive(true);
        }
    }

    public void JoinButtonFctFail() {
        for(int i = 0; i<compsToHide.Length; i++) {
            compsToHide[i].gameObject.SetActive(true);
        }
        for(int i = 0; i<compsToShow.Length; i++) {
            compsToShow[i].gameObject.SetActive(false);
        }
    }

}
