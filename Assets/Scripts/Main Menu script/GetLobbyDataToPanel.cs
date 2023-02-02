using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;
using Steamworks.Data;
using TMPro;

public class GetLobbyDataToPanel : MonoBehaviour
{
	public static GetLobbyDataToPanel Instance { get; set; }

	public SteamId lobbyID;
    public string lobbyName;
    public TextMeshProUGUI lobbyNameText;

    public void SetLobbyData(string lobbyOwner) 
    {
        lobbyNameText.text = lobbyOwner;
    }



    public void JoinLobby() 
    {
        SteamMatchmaking.JoinLobbyAsync(lobbyID);
    }
}
