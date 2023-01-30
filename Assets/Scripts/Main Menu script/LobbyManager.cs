using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    private static Lobby hostLobby;
    private static Lobby joinedLobby;
    private float lobbyTimer = 15.0f;
    private float lobbyUpdateTimer = 1.1f;
    private static string playerName = string.Empty;
	public static LobbyManager Instance { get; private set; }
	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			Instance = this;
		}
	}
	// Start is called before the first frame update
	//async void Start()
 //   {
 //       await UnityServices.InitializeAsync();

 //       AuthenticationService.Instance.SignedIn += () => { Debug.Log($"Signed in {AuthenticationService.Instance.PlayerId}"); };

 //       await AuthenticationService.Instance.SignInAnonymouslyAsync();

 //       GameObject.Find("ID").GetComponent<TextMeshProUGUI>().text = AuthenticationService.Instance.PlayerId;

	//}

    // Update is called once per frame
    void Update()
    {
        KeepLobbyAliveTimer();
		UpdateLobbyAsync();

	}

    public async void CreateLobby() 
    {
        try
        {
			string lobbyName = $"Lobby{Random.Range(1, 100)}";
			int maxPlayer = 4;

            CreateLobbyOptions lobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = false,
                Player = GetPlayer()
			};

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayer,lobbyOptions);
            hostLobby = lobby;
            joinedLobby = hostLobby;

            Debug.Log($"Lobby created {lobby.Name}:{maxPlayer}");
		}
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            throw;
        }
        
    }

    public async void JoinLobby() 
    {
		string code = GameObject.FindGameObjectWithTag("LobbyCode").GetComponent<TMP_InputField>().text;
		try
		{
            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions
            {
				Player = GetPlayer()
			};
			Lobby lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(code, joinLobbyByCodeOptions);
            joinedLobby= lobby;
		}
		catch (LobbyServiceException e)
		{
			Debug.Log(e);
			throw;
		}
	}

    public async void LeaveLobby()
    {
        try
        {
			await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
		}
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            throw;
        }
    }

    public async void ListLobbies()
    {
        try
        {
			QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

			foreach (Lobby lobby in queryResponse.Results)
			{
				Debug.Log($"{lobby.Name}:maxPlayer {lobby.MaxPlayers}");
                
			}
			GameObject.Find("Results").GetComponent<TextMeshProUGUI>().text = queryResponse.Results.Count.ToString();

			//delet this later
			GameObject.Find("code").GetComponent<TextMeshProUGUI>().text = hostLobby.LobbyCode;
		}
        catch (LobbyServiceException e)
        {
			Debug.Log(e);
			throw;
        }
        
    }

    private async void KeepLobbyAliveTimer() 
    {
        if (hostLobby != null)
        {
            lobbyTimer -= Time.deltaTime;
            if (lobbyTimer < 0f)
            {
                float maxtime = 15f;
                lobbyTimer = maxtime;
				await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
			}
		}
	}

	private async void UpdateLobbyAsync()
	{
        if (joinedLobby !=null)
        {
            lobbyUpdateTimer-= Time.deltaTime;
            if (lobbyUpdateTimer < 0f)
            {
                float maxtimer = 1.1f;
                lobbyUpdateTimer= maxtimer;
				Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
				joinedLobby = lobby;
			}
        }
	}

	private static Player GetPlayer() 
    {
		playerName = GameObject.Find("PlayerNameInput").GetComponent<TMP_InputField>().text;
		return new Player
        {
            Data = new Dictionary<string, PlayerDataObject>
            {
                {"PlayerName",new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member,playerName)},
                {"PlayerID",new PlayerDataObject(PlayerDataObject.VisibilityOptions.Private,AuthenticationService.Instance.PlayerId)}
            }
        };

	}

    private async void HostMigration() 
    {
		try
		{

			Lobby lobby = await LobbyService.Instance.UpdateLobbyAsync(hostLobby.Id,new UpdateLobbyOptions {HostId = joinedLobby.Players[1].Id });
			joinedLobby = hostLobby;
		}
		catch (LobbyServiceException e)
		{
			Debug.Log(e);
			throw;
		}
	}

    public void LoadLevel() 
    {
		//NetworkManager.Singleton.SceneManager.LoadScene("TestEnv", LoadSceneMode.Additive);

		//RelayTest.Instance.CreateRelay();
		//SceneManager.LoadScene("TestEnv");
		//NetworkManager.Singleton.StartHost();
		
	}

   // public async void StartGame()
   // {
   //     if (IsServer)
   //     {
   //         try
   //         {
   //             //RelayTest.Instance.CreateRelay();
   //             string relaycode = await RelayTest.Instance.CreateRelay();
			//}
   //         catch (System.Exception)
   //         {

   //             throw;
   //         }
   //     }
   // }


}
