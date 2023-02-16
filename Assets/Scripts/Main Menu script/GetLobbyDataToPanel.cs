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

	public void SetLobbyData(string lobbyOwner,int maxMember,int currentMembers, SteamId lobbyId) 
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
        lobbyID = lobbyId;

    }



    public void JoinLobby() 
    {
		//MainMenuLogics.Instance.SwitchMainToLobby();
		SteamMatchmaking.JoinLobbyAsync(lobbyID);
	}
}
