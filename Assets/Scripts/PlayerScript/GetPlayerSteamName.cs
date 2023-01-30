using Steamworks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GetPlayerSteamName : MonoBehaviour
{
	public static GetPlayerSteamName Instance { get; private set; } = null;
	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			GetComponent<TextMeshPro>().text = SteamClient.Name;
		}
		else
		{
			Destroy(Instance);
			return;
		}
	}
	// Start is called before the first frame update
	void Start()
    {
        
    }

}
