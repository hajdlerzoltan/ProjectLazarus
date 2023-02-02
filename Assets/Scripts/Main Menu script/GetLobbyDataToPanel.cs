using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;
using Steamworks.Data;
using TMPro;

public class GetLobbyDataToPanel : MonoBehaviour
{
    public SteamId lobbyID;
    public string lobbyName;
    public TextMeshProUGUI lobbyNameText;

    public void SetLobbyData() 
    {
        if (lobbyName == "")
        {
            lobbyNameText.text = $"{SteamClient.Name}'s lobby";
        }
        else 
        {
            lobbyNameText.text = lobbyName;
        }
    }



    public void JoinLobby() 
    {
        SteamMatchmaking.JoinLobbyAsync(lobbyID);
    }
}
