using Steamworks.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyListingManager : MonoBehaviour
{
	public static LobbyListingManager Instance { get; set; }

	public GameObject lobbyDataPrefab;

	public List<GameObject> lobbyList = new List<GameObject>();

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

	public void ShowLobbies(Lobby[] steamLobbies) 
	{

	}

	public void DestroyLobbies()
	{
		foreach (GameObject lobbies in lobbyList)
		{
			Destroy(lobbies);
		}
		lobbyList.Clear();
	}
}
