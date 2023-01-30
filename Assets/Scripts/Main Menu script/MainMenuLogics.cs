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
    [SerializeField] Button singleplayerButton;
	[SerializeField] Button joinViaCodeButton;
	[SerializeField] Button CreateLobbyButton;
	[SerializeField] Button BackToMainMenuButton;
	[SerializeField] TMP_InputField lobbyCodeInput;
	[SerializeField] GameObject LobbyMenu;
	[SerializeField] GameObject MainMenu;
	[SerializeField] GameObject LobbyPanel;
	[SerializeField] Toggle IsLobbyPrivate;

	SteamIntegration steam;

	float updateDelay = 1.5f;

	public static MainMenuLogics Instance { get; private set; } = null;

	// Start is called before the first frame update
	void Start()
    {
        joinViaCodeButton.interactable = false;

		lobbyCodeInput.onValueChanged.AddListener(delegate { DisableButtonsWithoutNickname(); });

		CreateLobbyButton.onClick.AddListener(delegate { SwitchMainToLobby(); });
		BackToMainMenuButton.onClick.AddListener(SwitchLobbyToMainMenu);
		singleplayerButton.onClick.AddListener(StartSingleplayerGame);
		IsLobbyPrivate.onValueChanged.AddListener(ChangeLobbyPrivacy);


	}

	private void ChangeLobbyPrivacy(bool arg0)
	{
		if (IsLobbyPrivate.isOn)
		{
			SteamIntegration.Instance.CurrentLobby.Value.SetPrivate();
		}
		else
		{
			SteamIntegration.Instance.CurrentLobby.Value.SetPublic();
		}
	}


	// Update is called once per frame
	void Update()
    {
		StartCoroutine(UpdateLobbyUI());

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
	void SwitchMainToLobby() 
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

	void ShowOpenLobbys() 
	{
		if (SteamIntegration.Instance.lobbys != null)
		{
			var lobbys = SteamIntegration.Instance.lobbys;
			GameObject content = GameObject.Find("Content");


			foreach (var item in lobbys)
			{
				Instantiate(LobbyPanel,content.transform);
			}
		}
	}

	IEnumerator UpdateLobbyUI() 
	{
		yield return new WaitForSeconds(updateDelay);
		ShowOpenLobbys();
	}
}
