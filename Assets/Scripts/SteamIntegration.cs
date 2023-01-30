using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Netcode.Transports.Facepunch;
using Steamworks;
using Steamworks.Data;
using System;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class SteamIntegration : MonoBehaviour
{


	public static SteamIntegration Instance { get; private set; } = null;

	private FacepunchTransport transport = null;

	public Lobby? CurrentLobby { get; set; } = null;

	Lobby[] lobby = null;


	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(Instance);
			return;
		}
	}


	// Start is called before the first frame update
	void Start()
	{
		transport = GetComponent<FacepunchTransport>();
		SteamMatchmaking.OnLobbyCreated += OnLobbyCreated;
		SteamMatchmaking.OnLobbyEntered += OnLobbyEntered;
		SteamMatchmaking.OnLobbyMemberJoined += OnLobbyJoin;
		SteamMatchmaking.OnLobbyMemberLeave += OnLobbyLeave;
		SteamMatchmaking.OnLobbyInvite += OnLobbyInvite;
		SteamMatchmaking.OnLobbyGameCreated += OnLobbyGameCreated;
		SteamFriends.OnGameLobbyJoinRequested += OnLobbyJoinRequest;

	}

	private void Update()
	{
		SteamClient.RunCallbacks();
		StartCoroutine("GetOpenLobbys");
	}

	private void OnDestroy()
	{
		SteamMatchmaking.OnLobbyCreated -= OnLobbyCreated;
		SteamMatchmaking.OnLobbyEntered -= OnLobbyEntered;
		SteamMatchmaking.OnLobbyMemberJoined -= OnLobbyJoin;
		SteamMatchmaking.OnLobbyMemberLeave -= OnLobbyLeave;
		SteamMatchmaking.OnLobbyInvite -= OnLobbyInvite;
		SteamMatchmaking.OnLobbyGameCreated -= OnLobbyGameCreated;
		SteamFriends.OnGameLobbyJoinRequested -= OnLobbyJoinRequest;
		NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedCallback;
		NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnectCallback;
		NetworkManager.Singleton.OnServerStarted -= OnServerStarted;

		Disconnect();
		SteamClient.Shutdown();
	}

	private void OnApplicationQuit()
	{

		SteamClient.Shutdown();
		Disconnect();
	}



	public async void StartHost(int maxMembers = 4)
	{
		NetworkManager.Singleton.StartHost();

		NetworkManager.Singleton.OnServerStarted += OnServerStarted;

		Debug.Log("Hosting is started!");

		await SteamMatchmaking.CreateLobbyAsync();
		lobby = await SteamMatchmaking.LobbyList.WithKeyValue("name", "TestLobby").RequestAsync();

		foreach (var item in lobby)
		{
			Debug.Log(item.Data.GetEnumerator());
		}
	}

	public async void Disconnect() 
	{
		CurrentLobby?.Leave();
		if (NetworkManager.Singleton == null)
		{
			return;
		}
		NetworkManager.Singleton.Shutdown();

		lobby = await SteamMatchmaking.LobbyList.RequestAsync();
		foreach (var item in lobby)
		{
			Debug.Log(item.Data.GetEnumerator());
		}
	}

	public void StartClient(SteamId id)
	{
		NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
		NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;

		transport.targetSteamId = id;

		if (NetworkManager.Singleton.StartClient())
		{
			Debug.Log("Client is started!");
		}
	}

	#region SteamCallbacks
	private void OnLobbyJoinRequest(Lobby lobby, SteamId id)
	{
		StartClient(id);
	}

	private void OnLobbyGameCreated(Lobby lobby, uint ip, ushort port, SteamId id)
	{
		
	}

	private void OnLobbyInvite(Friend friend, Lobby lobby)
	{
		Debug.Log($"you have been invited by {friend.Name}");
	}

	private void OnLobbyLeave(Lobby lobby, Friend friend)
	{
		
	}

	private void OnLobbyJoin(Lobby lobby, Friend friend)
	{
		Debug.Log($"Client connected by himself: {friend.Id}");
		StartClient(friend.Id);
	}

	private void OnLobbyEntered(Lobby lobby)
	{
		if (NetworkManager.Singleton.IsHost)
		{
			return;
		}
		else if (NetworkManager.Singleton.IsClient)
		{
			MainMenuLogics.Instance.SwitchMainToLobby();
			StartClient(lobby.Id);
		}

	}

	private void OnLobbyCreated(Result result, Lobby lobby)
	{
		if (result != Result.OK)
		{
			Debug.Log($"Cant Create a lobby! --- {result}");
		}

		lobby.SetFriendsOnly();
		lobby.SetData("name", "TestLobby");
		lobby.SetJoinable(true);

		Debug.Log("Lobby created");
	}
	#endregion

	#region NetworkCallbacks
	public void OnServerStarted()
	{
		NetworkManager.Singleton.StartHost();
		Debug.Log("Server started");
		NetworkManager.Singleton.SceneManager.LoadScene("testEnv", LoadSceneMode.Single);
	}

	private void OnClientDisconnectCallback(ulong clientID)
	{
		NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedCallback;
		NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnectCallback;
		Debug.Log($"Client disconnected: {clientID}");
	}

	private void OnClientConnectedCallback(ulong clientID)
	{
		Debug.Log($"Client connected: {clientID}");
	}
	#endregion

	public async void GetOpenLobbys() 
	{
		lobby = await SteamMatchmaking.LobbyList.WithKeyValue("name", "TestLobby").RequestAsync();
		if (lobby == null)
		{
			Debug.Log("No lobby found");
		}
		else
		{
			foreach (var item in lobby)
			{
				Debug.Log(item);
			}
		}
		
	}
}
