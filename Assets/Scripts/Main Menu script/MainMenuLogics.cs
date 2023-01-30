using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuLogics : MonoBehaviour
{
    [SerializeField]
    Button singleplayerButton;
	[SerializeField]
	Button joinViaCodeButton;
	[SerializeField]
	Button CreateLobbyButton;
	[SerializeField]
	Button BackToMainMenuButton;
	[SerializeField]
	TMP_InputField lobbyCodeInput;
	[SerializeField]
	GameObject LobbyMenu;
	[SerializeField]
	GameObject MainMenu;
	SteamIntegration steam;

	public static MainMenuLogics Instance { get; private set; } = null;

	// Start is called before the first frame update
	void Start()
    {
        joinViaCodeButton.interactable = false;

		lobbyCodeInput.onValueChanged.AddListener(delegate { DisableButtonsWithoutNickname(); });

		CreateLobbyButton.onClick.AddListener(delegate { SwitchMainToLobby(); });
		BackToMainMenuButton.onClick.AddListener(SwitchLobbyToMainMenu);
		singleplayerButton.onClick.AddListener(StartSingleplayerGame);

		SteamIntegration.Instance.GetOpenLobbys();
	}


	// Update is called once per frame
	void Update()
    {
        
    }

    public void DisableButtonsWithoutNickname() 
    {
		if (lobbyCodeInput.text != "")
		{
			joinViaCodeButton.interactable = true;
		}
		else
		{
			joinViaCodeButton.interactable = false;
		}


	}
	public void SwitchMainToLobby() 
	{
		LobbyMenu.gameObject.SetActive(true);
		MainMenu.gameObject.SetActive(false);

	}
	void SwitchLobbyToMainMenu()
	{
		MainMenu.gameObject.SetActive(true);
		LobbyMenu.gameObject.SetActive(false);
		SteamIntegration.Instance.Disconnect();

	}

	public void StartSingleplayerGame()
	{
		NetworkManager.Singleton.StartHost();
		NetworkManager.Singleton.SceneManager.LoadScene("testEnv", LoadSceneMode.Single);
	}

	public void StartGameFromLobby() 
	{
		if (NetworkManager.Singleton.IsHost)
		{
			NetworkManager.Singleton.StartHost();
			NetworkManager.Singleton.SceneManager.LoadScene("testEnv", LoadSceneMode.Single);
		}
		else if (NetworkManager.Singleton.IsClient)
		{
			NetworkManager.Singleton.StartClient();
		}
	}
	
	//private async void FillLobbyMembersData() 
	//{
	//	var lol = GetSteamAvatar();
	//	await Task.WhenAll(lol);
	//	GameObject playerImage = GameObject.Find("PlayerImage");
	//	playerImage.GetComponent<Image>().sprite = lol.Result?.Convert();
	//}

	//public async Task<Image?> GetSteamAvatar()
	//{
	//	try
	//	{
	//		// Get Avatar using await
	//		return await SteamFriends.GetLargeAvatarAsync(SteamClient.SteamId);
	//	}
	//	catch (Exception e)
	//	{
	//		// If something goes wrong, log it
	//		Debug.Log(e);
	//		return null;
	//	}
	//}
}
