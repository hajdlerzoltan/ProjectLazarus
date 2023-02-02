using Steamworks;
using Steamworks.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
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

	List<Lobby> steamLobbies = new List<Lobby>();

	float updateDelay = 3f;

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

		StartCoroutine(UpdateLobbyUI(updateDelay));
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
		CheckPlayerIsInTheMainMenu();
		//InvokeRepeating("ShowOpenLobbys", 0, 3);
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
	public void SwitchLobbyToMainMenu()
	{
		MainMenu.gameObject.SetActive(true);
		LobbyMenu.gameObject.SetActive(false);
		SteamManager.Instance.LeaveLobby();

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

	async void ShowOpenLobbys() 
	{
		Lobby[] lobby = await SteamManager.Instance.GetOpenLobbys();
		if (CheckPlayerIsInTheMainMenu())
		{
			if (lobby == null)
			{
				Debug.Log("no lobby found");
			}
			else
			{
				foreach (var item in lobby)
				{
					steamLobbies.Add(item);
				}

				GameObject content = GameObject.Find("Content");

				foreach (var item in steamLobbies)
				{
					GameObject lobbyOnUI = Instantiate(LobbyPanel, content.transform);
					GameObject.Find("LobbyHostName").GetComponent<TextMeshProUGUI>().text = item.Owner.Name;
				}
				steamLobbies.Clear();
			}
		}

	}
	//Checking the player if he is in the menu or not
	bool CheckPlayerIsInTheMainMenu() 
	{
		GameObject mainmenu = GameObject.Find("MainMenu");
		if (!mainmenu.activeInHierarchy)
		{
			return false;
		}
		else
		{
			return true;
		}
	}

	IEnumerator UpdateLobbyUI(float delay) 
	{
		while (true)
		{
			ShowOpenLobbys();
			yield return new WaitForSecondsRealtime(delay);
		}
	}
}
