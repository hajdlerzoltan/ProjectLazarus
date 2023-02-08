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
using System.Threading.Tasks;
public class SteamManager : MonoBehaviour
{
	//singleton instance
	public static SteamManager Instance { get; set; } = null;
	public static Lobby[] SteamLobbies { get; set; } = null;

	public Lobby currentLobby;
	public string currentLobbyOwner;
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

	private void Start()
	{
		DontDestroyOnLoad(this);

		try
		{
			SteamClient.Init(480, true);
		}
		catch (Exception e)
		{
			Debug.Log(e);
		}

		SteamMatchmaking.OnLobbyCreated += OnLobbyCreated;
		SteamMatchmaking.OnLobbyEntered += OnLobbyEntered;
		SteamMatchmaking.OnLobbyMemberJoined += OnLobbyMemberJoined;
		SteamMatchmaking.OnLobbyMemberLeave += OnLobbyMemberLeave;

	}


	private void Update()
	{
		SteamClient.RunCallbacks();
	}

	private void OnDisable()
	{
		SteamClient.Shutdown();
	}

	private void OnApplicationQuit()
	{
		LeaveLobby();
		SteamClient.Shutdown();
	}

	#region SteamCallbacks

	//Callback on the SteamAPI when you create a lobby
	private void OnLobbyCreated(Result result, Lobby lobby)
	{
		if (result != Result.OK)
		{
			Debug.Log($"Failed to create the lobby: {result}");
		}
		else
		{
			Debug.Log("Lobby created!");

			//OnLobbyCreated.Invoke();
		}
		//Debug.Log("Lobby created!");
	}

	//Callback on the SteamAPI when someone entered the lobby
	private void OnLobbyEntered(Lobby lobby)
	{
		MainMenuLogics.Instance.SwitchMainToLobby();
		Debug.Log("client joined");
	}

	//Callback on the SteamAPI when someone join on you by the steam overlay
	private async void OnLobbyMemberJoined(Lobby lobby, Friend friend)
	{
		
		Debug.Log($"{friend.Name} joined...");
	}

	//Callback on the SteamAPI
	private void OnLobbyMemberLeave(Lobby lobby, Friend friend)
	{

		Debug.Log($"{friend.Name} Left...");
	}

	#endregion

	//Creating the game lobby on Steam
	public async void CreateLobby() 
	{
		Lobby lobby = (Lobby)await SteamMatchmaking.CreateLobbyAsync(4);

		lobby.SetJoinable(true);
		//lobby.SetPrivate();
		lobby.SetPublic();
		lobby.SetData("Game", "ProjectLazarus");
		currentLobby = lobby;
	}

	public void JoinLobby() 
	{
		
	}

	public void LeaveLobby() 
	{
		currentLobby.Leave();
	}

	public async Task<Lobby[]> GetOpenLobbys()
	{
		//Lobby[] lobbys = await SteamMatchmaking.LobbyList.WithKeyValue("Game", "ProjectLazarus").RequestAsync();
		Lobby[] lobbys = await SteamMatchmaking.LobbyList.RequestAsync();
		if (lobbys == null)
		{
			return lobbys;
		}
		else
		{
			foreach (var item in lobbys)
			{
				await item.RefreshAsync();
			}
			await Task.Yield();
			return lobbys;
		}
	}
	//only for debuging delete later
	public async void FindOpenGameLobbys() 
	{
		Lobby[] lobbys = await SteamMatchmaking.LobbyList.WithKeyValue("Game", "ProjectLazarus").RequestAsync();
		if (lobbys != null)
		{
			Debug.Log("found");
		}
		else
		{
			Debug.Log("not found");
		}
	}

}
