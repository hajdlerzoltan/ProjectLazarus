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
    public TextMeshProUGUI FreeSlotsText;

	public void SetLobbyData(string lobbyOwner,int maxMember,int currentMembers) 
    {
        if (lobbyOwner == "")
        {
            lobbyNameText.text = "Empty Name";
        }
        else
        {
            lobbyNameText.text = lobbyOwner;
        }

        int freeslots = maxMember - currentMembers;
        FreeSlotsText.text = freeslots.ToString();

    }



    public void JoinLobby() 
    {
        SteamMatchmaking.JoinLobbyAsync(lobbyID);
    }
}
