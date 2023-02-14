using Steamworks;
using Steamworks.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Image = Steamworks.Data.Image;

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
	[SerializeField] GameObject mainmenu;
	Lobby[] lobby;

	float updateDelay = 3f;

	public static MainMenuLogics Instance { get; private set; } = null;

	// Start is called before the first frame update
	async void Start()
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
			//Steamm.Instance.CurrentLobby.Value.SetPrivate();
		}
		else
		{
			//SteamIntegration.Instance.CurrentLobby.Value.SetPublic();
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
		if (CheckPlayerIsInTheMainMenu())
		{
			lobby = await SteamManager.Instance.GetOpenLobbys();

			GameObject[] steamLobbyInstances = GameObject.FindGameObjectsWithTag("OpenSteamLobby");
			if (steamLobbyInstances != null)
			{
				foreach (var item in steamLobbyInstances)
				{
					Destroy(item);
				}
			}

			if (lobby == null)
			{
				Debug.Log("no lobby found");
			}
			else
			{

				GameObject content = GameObject.Find("Content");

				foreach (var item in lobby)
				{
					GameObject lobbyPanle = Instantiate(LobbyPanel, content.transform);
					lobbyPanle.GetComponent<GetLobbyDataToPanel>().SetLobbyData(item.Owner.Name,item.MaxMembers,item.MemberCount);
					var avatar = GetAvatar(item.Owner.Id);
					await Task.WhenAll(avatar);
					lobbyPanle.GetComponentInChildren<RawImage>().texture = avatar.Result?.Covert();
					lobbyPanle.gameObject.transform.SetParent(content.transform, false);
				}				
			}
		}

	}
	//Checking the player if he is in the menu or not
	bool CheckPlayerIsInTheMainMenu() 
	{
		if (!mainmenu.activeInHierarchy)
		{
			return false;
		}
		else
		{
			return true;
		}
	}

	//IEnumerator UpdateLobbyUI(float delay) 
	//{
	//	while (true)
	//	{
	//		ShowOpenLobbys();
	//		yield return new WaitForSecondsRealtime(delay);
	//	}
	//}

	IEnumerator UpdateLobbyUI(float delay)
	{
		while (true)
		{
			ShowOpenLobbys();
			yield return new WaitForSecondsRealtime(delay);
		}
	}


	private static async Task<Image?> GetAvatar(SteamId steamid)
	{
		try
		{
			// Get Avatar using await
			return await SteamFriends.GetLargeAvatarAsync(steamid);
		}
		catch (Exception e)
		{
			// If something goes wrong, log it
			Debug.Log(e);
			return null;
		}
	}



}
public static class Enxtension
{
	public static Texture2D Covert(this Image image)
	{
		// Create a new Texture2D
		var avatar = new Texture2D((int)image.Width, (int)image.Height, TextureFormat.ARGB32, false);

		// Set filter type, or else its really blury
		avatar.filterMode = FilterMode.Trilinear;

		// Flip image
		for (int x = 0; x < image.Width; x++)
		{
			for (int y = 0; y < image.Height; y++)
			{
				var p = image.GetPixel(x, y);
				avatar.SetPixel(x, (int)image.Height - y, new UnityEngine.Color(p.r / 255.0f, p.g / 255.0f, p.b / 255.0f, p.a / 255.0f));
			}
		}

		avatar.Apply();
		return avatar;
	}

	public static async Task<Lobby> RefreshAsync(this Lobby lobby)
	{
		TaskCompletionSource<bool> resultWaiter = new TaskCompletionSource<bool>();
		bool isOwnerGotName = false;
		Action<Lobby> eventHandler = (Lobby queriedLobby) =>
		{
			if (lobby.Id != queriedLobby.Id) return;
			resultWaiter.SetResult(true);
		};

		SteamMatchmaking.OnLobbyDataChanged += eventHandler;
		lobby.Refresh();
		while (isOwnerGotName == false)
		{
			if (lobby.Owner.Name !="")
			{
				isOwnerGotName = true;
			}
		}
		
		var result = await resultWaiter.Task;
		SteamMatchmaking.OnLobbyDataChanged -= eventHandler;
		await Task.Yield();
		return lobby;
	}
}
