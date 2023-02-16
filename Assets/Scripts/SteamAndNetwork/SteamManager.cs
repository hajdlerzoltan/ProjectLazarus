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
using System.Linq;

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

		SteamMatchmaking.OnLobbyCreated += OnLobbyCreated;
		SteamMatchmaking.OnLobbyEntered += OnLobbyEntered;
		SteamMatchmaking.OnLobbyMemberJoined += OnLobbyMemberJoined;
		SteamMatchmaking.OnLobbyMemberLeave += OnLobbyMemberLeave;

	}


	private void Update()
	{
		//SteamClient.RunCallbacks();
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

	//Callback on the SteamAPI when someone join to the lobby
	private void OnLobbyMemberJoined(Lobby lobby, Friend friend)
	{
		MainMenuLogics.Instance.SwitchMainToLobby();
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
		lobby.SetPrivate();
		//lobby.SetPublic();
		lobby.SetData("Game", "ProjectLazarus");
		currentLobby = lobby;
	}

	public void JoinLobby() 
	{
		MainMenuLogics.Instance.SwitchMainToLobby();
		Debug.Log("JoinLobby callback");
	}

	public void LeaveLobby() 
	{
		currentLobby.Leave();
	}

	public async Task<Lobby[]> GetOpenLobbys()
	{
		Lobby[] lobbys = await SteamMatchmaking.LobbyList.WithKeyValue("Game", "ProjectLazarus").RequestAsync();
		//Lobby[] lobbys = await Task.Run(() => SteamMatchmaking.LobbyList.RequestAsync());
		List<Task<Lobby>> tasks = new List<Task<Lobby>>();
		if (lobbys != null)
		{
			foreach (var item in lobbys)
			{

				tasks.Add(Task.Run(() => item.RefreshAsync()));
			}
		}

		var result = await Task.WhenAll(tasks);
		return result;
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
